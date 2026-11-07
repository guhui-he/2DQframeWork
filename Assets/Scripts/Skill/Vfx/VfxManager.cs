using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class VfxManager : ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        private IVfxModel model;

        // 按 prefab path 分池：不同 VFX prefab 结构可能不同，不能混用一个池
        private readonly Dictionary<string, ComponentPool<Effect>> _effectPools = new();
        private readonly Dictionary<string, ComponentPool<LaserEffect>> _laserPools = new();

        // 纹理缓存，避免重复 LoadTexture2D
        private readonly Dictionary<string, Texture2D> _texCache = new();

        void Start()
        {
            model = this.GetModel<IVfxModel>();
            this.RegisterEvent<PlayVfxEvent>(OnPlayVfx).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<SpawnProjectileEvent>(OnSpawnProjectile).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        // ==================== 原地特效（施法、AOE、命中） ====================
        void OnPlayVfx(PlayVfxEvent e)
        {
            var vfx = model.GetVfxConfig(e.VfxId);
            if (vfx == null) return;

            var pool = GetEffectPool(vfx);
            if (pool == null) return;

            var effect = pool.Allocate();
            effect.transform.SetPositionAndRotation(ResolveVfxPosition(e, vfx), Quaternion.identity);

            effect.Play(LoadTex(vfx.Path), vfx.TileX, vfx.TileY, vfx.Timer, false);
            StartCoroutine(RecycleAfter(pool, effect, vfx.Timer + 0.5f));
        }

        // ==================== 飞行物 & 射线 ====================
        void OnSpawnProjectile(SpawnProjectileEvent e)
        {
            var vfx = model.GetVfxConfig(e.VfxId);
            if (vfx == null) return;

            var caster = GetUnit(e.FromId);
            if (caster == null) return;

            if (!BlockViewRegistry.TryGetWorldPosition(caster.Pos, out var startWp)) return;
            if (!BlockViewRegistry.TryGetWorldPosition(e.ToPos, out var endWp)) return;

            Vector3 startPos = new Vector3(startWp.x, startWp.y, -1)
                               + new Vector3(vfx.OffsetX, vfx.OffsetY, 0);
            Vector3 endPos = new Vector3(endWp.x, endWp.y, -1);
            Vector3 dir = (endPos - startPos).normalized;

            // 射线类型：一次性播放、不移动
            if (IsLaserVfx(vfx))
            {
                SpawnLaser(vfx, startPos, dir, e.Range);
                return;
            }

            SpawnFlyingEffect(vfx, startPos, endPos, dir);
        }

        void SpawnLaser(VfxConfig vfx, Vector3 startPos, Vector3 dir, float range)
        {
            var pool = GetLaserPool(vfx);
            if (pool == null) return;

            var laser = pool.Allocate();
            laser.transform.position = startPos;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            laser.transform.rotation = Quaternion.Euler(0, 0, angle);

            laser.Play(LoadTex(vfx.Path), vfx.TileX, vfx.TileY, vfx.Timer, range);
            StartCoroutine(RecycleAfter(pool, laser, vfx.Timer + 0.2f));
        }

        void SpawnFlyingEffect(VfxConfig vfx, Vector3 startPos, Vector3 endPos, Vector3 dir)
        {
            var pool = GetEffectPool(vfx);
            if (pool == null) return;

            var effect = pool.Allocate();
            effect.transform.position = startPos;

            float safeSpeed = Mathf.Max(vfx.Speed, 0.1f);
            float distance = Vector3.Distance(startPos, endPos);
            float flyDuration = distance / safeSpeed;

            effect.Play(LoadTex(vfx.Path), vfx.TileX, vfx.TileY, flyDuration, true);

            float flyAngle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            effect.transform.rotation = Quaternion.Euler(0, 0, flyAngle);

            StartCoroutine(MoveProjectile(pool, effect, startPos, endPos, safeSpeed));
        }

        IEnumerator MoveProjectile(
            ComponentPool<Effect> pool, Effect effect,
            Vector3 start, Vector3 end, float speed)
        {
            float distance = Vector3.Distance(start, end);
            float duration = Mathf.Max(distance / speed, 0.2f);
            float timer = 0f;

            while (timer < duration && IsAlive(effect))
            {
                timer += Time.deltaTime;
                effect.transform.position = Vector3.Lerp(start, end, timer / duration);
                yield return null;
            }

            if (IsAlive(effect))
            {
                effect.transform.position = end;
                pool.Recycle(effect);
            }
        }

        IEnumerator RecycleAfter<T>(ComponentPool<T> pool, T comp, float delay) where T : Component
        {
            yield return new WaitForSeconds(delay);
            if (IsAlive(comp)) pool.Recycle(comp);
        }

        // ==================== 池管理 ====================
        ComponentPool<Effect> GetEffectPool(VfxConfig vfx)
        {
            if (_effectPools.TryGetValue(vfx.PrefabPath, out var pool)) return pool;

            var prefab = this.GetUtility<IPrefabLoaderUtility>().LoadPrefab(vfx.PrefabPath);
            pool = new ComponentPool<Effect>(prefab, transform);
            _effectPools[vfx.PrefabPath] = pool;
            return pool;
        }

        ComponentPool<LaserEffect> GetLaserPool(VfxConfig vfx)
        {
            if (_laserPools.TryGetValue(vfx.PrefabPath, out var pool)) return pool;

            var prefab = this.GetUtility<IPrefabLoaderUtility>().LoadPrefab(vfx.PrefabPath);
            pool = new ComponentPool<LaserEffect>(prefab, transform);
            _laserPools[vfx.PrefabPath] = pool;
            return pool;
        }

        bool IsLaserVfx(VfxConfig vfx)
        {
            var prefab = this.GetUtility<IPrefabLoaderUtility>().LoadPrefab(vfx.PrefabPath);
            return prefab != null && prefab.GetComponent<LaserEffect>() != null;
        }

        // ==================== 工具 ====================
        Texture2D LoadTex(string path)
        {
            if (!_texCache.TryGetValue(path, out var tex) || tex == null)
            {
                tex = this.GetSystem<IResSystem>().LoadTexture2D(path);
                _texCache[path] = tex;
            }
            return tex;
        }

        BattleUnitData GetUnit(int unitId)
        {
            if (unitId == 0) return null;
            foreach (var u in this.GetModel<IBattleModel>().AllUnits)
                if (u.UnitId == unitId) return u;
            return null;
        }

        Vector3 ResolveVfxPosition(PlayVfxEvent e, VfxConfig vfx)
        {
            Vector3 spawnPos = Vector3.zero;
            if (e.TargetId != 0)
            {
                var target = GetUnit(e.TargetId);
                if (target != null && BlockViewRegistry.TryGetWorldPosition(target.Pos, out var wp))
                    spawnPos = new Vector3(wp.x, wp.y, -1);
            }
            else
            {
                if (BlockViewRegistry.TryGetWorldPosition(e.TargetPos, out var wp))
                    spawnPos = new Vector3(wp.x, wp.y, -1);
            }
            return spawnPos + new Vector3(vfx.OffsetX, vfx.OffsetY, 0);
        }

        static bool IsAlive(Component c) => c != null && c.gameObject.activeSelf;
    }
}
