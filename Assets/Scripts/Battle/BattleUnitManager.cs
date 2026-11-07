using QFramework;
using Spine.Unity;
using Spine.Unity.Editor;
using UnityEngine;

namespace Game
{
    public partial class BattleUnitManager :ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        void Start()
        {
            // 监听单位生成事件
            this.RegisterEvent<UnitSpawnedEvent>(OnUnitSpawned)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnUnitSpawned(UnitSpawnedEvent e)
        {
            var loader = this.GetUtility<IPrefabLoaderUtility>();
            var obj = loader.Instantiate(e.ModelPath);
            if (obj == null) return;
            if (BlockViewRegistry.TryGetWorldPosition(e.Pos, out var wp))
            {
                obj.transform.position = new Vector3(wp.x, wp.y, -1);
            }
            var view = obj.GetComponent<BattleUnitView>();
            if (view != null)
            {
                if (view.ani.AnimationState != null)
                {
                    view.ani.AnimationState.ClearTracks();
                }
                // 3. 替换 SkeletonDataAsset 并重新初始化
                view.ani.skeletonDataAsset = this.GetSystem<IResSystem>().GetSkeleton(e.AniPath);
                view.ani.Initialize(true);

                // 4. 播放新动画
                view.ani.AnimationState.SetAnimation(0, "Idle", true);
                view.UnitId = e.UnitId;
            }
        }
    }
}
