using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ISaveSystem:ISystem
    {
        void SaveGame();
        bool LoadGame();
    }
    public class SaveSystem : AbstractSystem, ISaveSystem
    {
        public void SaveGame()
        {
            var saveData = new SaveData();

            var playerModel = this.GetModel<IPlayerModel>();
            if (playerModel != null)
            {
                saveData.PlayerX = playerModel.Position.Value.x;
                saveData.PlayerY = playerModel.Position.Value.y;
            }


            // --- 收集大地图敌人 ---
            var enemyIconModel = this.GetModel<IEnemyIconModel>();
            if (enemyIconModel != null)
            {
                foreach (var enemy in enemyIconModel.Enemies)
                {
                    saveData.MapEnemies.Add(enemy.ToSaveData()); // 用扩展方法转换
                }
            }

            // --- 收集刷怪计时器 ---
            var spawnModel = this.GetModel<ISpawnEnemyModel>();
            if (spawnModel != null)
            {
                foreach (var kv in spawnModel.SpawnTimers)
                {
                    saveData.SpawnTimers.Add(new SpawnTimerSaveData { SpawnId = kv.Key, Timer = kv.Value });
                }
            }

            // --- 收集玩家队伍数据
            var heroModel = this.GetModel<IHeroModel>();
            if (heroModel != null)
            {
                saveData.CurrentHeroId = heroModel.curHeroId.Value;
                foreach (var kv in heroModel.mHero)
                {
                    saveData.Heroes.Add(kv.Value.ToSaveData());
                }
            }

            // ★ 保存敌人队伍数据
            var enemyModel = this.GetModel<IEnemyModel>();
            if (enemyModel != null)
            {
                foreach (var kv in enemyModel.Roles)
                {
                    var group = new EnemyGroupSaveData { EnemyId = kv.Key };
                    foreach (var role in kv.Value)
                    {
                        group.Roles.Add(role.ToSaveData());
                    }
                    saveData.Enemies.Add(group);
                }
            }

            // --- 背包数据 ---
            var bagModel = this.GetModel<IBagModel>();
            if (bagModel != null)
            {
                foreach (var item in bagModel.Items)
                {
                    var s = item.ToSaveData();
                    if (s != null) saveData.BagItems.Add(s);
                }
            }

            // --- 商店 ---
            var shopModel = this.GetModel<IShopModel>();
            if (shopModel != null)
            {
                foreach (var kv in shopModel.Items)
                {
                    var group = new ShopSaveData
                    {
                        ShopId = kv.Key,
                        LastRefreshTime = shopModel.GetFreshTime(kv.Key)
                    };
                    foreach (var item in kv.Value)
                    {
                        var s = item.ToSaveData();
                        if (s != null) group.Items.Add(s);
                    }
                    saveData.Shops.Add(group);
                }
            }

            // --- 酒馆 ---
            var tavernModel = this.GetModel<ISillyTavernModel>();
            if (tavernModel != null)
            {
                foreach (var kv in tavernModel.mHeros)
                {
                    var group = new SillyTavernSaveData
                    {
                        TavernId = kv.Key,
                        LastRefreshTime = tavernModel.GetFreshTime(kv.Key)
                    };
                    foreach (var role in kv.Value)
                    {
                        group.Heroes.Add(role.ToSaveData());
                    }
                    saveData.Taverns.Add(group);
                }
            }
            Debug.Log("<color=green>[SaveSystem] 存档成功</color>");
            JsonMgr.Instance.SaveData(saveData, "MainSave", JsonType.LitJson);
        }

        public bool LoadGame()
        {
            var saveData = JsonMgr.Instance.LoadData<SaveData>("MainSave", JsonType.LitJson);
            if (saveData == null) return false;


            var playerModel = this.GetModel<IPlayerModel>();
            if (playerModel != null)
            {
                playerModel.Position.Value = new Vector2(saveData.PlayerX, saveData.PlayerY);
                playerModel.HasLoadedPosition = true;
            }
            // --- 恢复大地图敌人 ---
            var enemyIconModel = this.GetModel<IEnemyIconModel>();
            if (enemyIconModel != null && saveData.MapEnemies != null)
            {
                enemyIconModel.ClearAll();
                int maxId = 0;

                foreach (var data in saveData.MapEnemies)
                {
                    var enemy = data.FromSaveData();

                    // ★ 正确做法：调用 Model 里的添加方法
                    enemyIconModel.AddEnemyData(enemy);

                    if (enemy.Id > maxId) maxId = enemy.Id;
                }
                // 恢复游标（确保 Model 里有公开的 NextId 属性）
                enemyIconModel.NextId = maxId + 1;
            }

            // --- 恢复刷怪计时器 ---
            var spawnModel = this.GetModel<ISpawnEnemyModel>();
            if (spawnModel != null && saveData.SpawnTimers != null)
            {
                foreach (var data in saveData.SpawnTimers)
                {
                    if (spawnModel.SpawnTimers.ContainsKey(data.SpawnId))
                        spawnModel.SpawnTimers[data.SpawnId] = data.Timer;
                }
            }

            var heroModel = this.GetModel<IHeroModel>();
            if (heroModel != null)
            {
                // 清空当前内存
                heroModel.mHero.Clear();

                foreach (var roleSave in saveData.Heroes)
                {
                    var role = roleSave.FromSaveData();
                    heroModel.mHero[role.Id] = role;
                }
                heroModel.curHeroId.Value = saveData.CurrentHeroId;
            }

            // ★ 恢复敌人队伍数据
            var enemyModel = this.GetModel<IEnemyModel>();
            if (enemyModel != null)
            {
                enemyModel.Roles.Clear();

                if (saveData.Enemies != null)
                {
                    foreach (var group in saveData.Enemies)
                    {
                        var enemyList = new List<RoleInfo>();
                        if (group.Roles != null)
                        {
                            foreach (var roleSave in group.Roles)
                            {
                                Debug.Log(group.Roles.Count);
                                enemyList.Add(roleSave.FromSaveData());
                            }
                        }
                        enemyModel.Roles[group.EnemyId] = enemyList;
                    }
                }
            }

            // 恢复背包
            var bagModel = this.GetModel<IBagModel>();
            if (bagModel != null)
            {
                var items = new List<ItemInfo>();
                if (saveData.BagItems != null)
                {
                    foreach (var s in saveData.BagItems)
                    {
                        var item = s.FromSaveData();
                        if (item != null) items.Add(item);
                    }
                }
                bagModel.Refresh(items); // 设 area 为 Bag
            }

            // --- 商店 ---
            var shopModel = this.GetModel<IShopModel>();
            if (shopModel != null)
            {
                shopModel.Items.Clear();
                shopModel.LastRefreshTime.Clear();

                if (saveData.Shops != null)
                {
                    foreach (var s in saveData.Shops)
                    {
                        var list = new BindableList<ItemInfo>();
                        if (s.Items != null)
                        {
                            foreach (var itemSave in s.Items)
                            {
                                var item = itemSave.FromSaveData();
                                if (item != null)
                                {
                                    item.area = AreaType.Shop;   
                                    list.Add(item);
                                }
                            }
                        }
                        shopModel.Items[s.ShopId] = list;
                        shopModel.SetFreshTime(s.ShopId, s.LastRefreshTime);
                    }
                }
            }

            // --- 酒馆 ---
            var tavernModel = this.GetModel<ISillyTavernModel>();
            if (tavernModel != null)
            {
                tavernModel.mHeros.Clear();
                tavernModel.LastRefreshTime.Clear();

                if (saveData.Taverns != null)
                {
                    foreach (var t in saveData.Taverns)
                    {
                        var list = new BindableList<RoleInfo>();
                        if (t.Heroes != null)
                        {
                            foreach (var r in t.Heroes)
                                list.Add(r.FromSaveData());
                        }
                        tavernModel.mHeros[t.TavernId] = list;
                        tavernModel.SetFreshTime(t.TavernId, t.LastRefreshTime);
                    }
                }
            }
                this.SendEvent(new GameLoadedEvent());
            Debug.Log("<color=green>[SaveSystem] 读档成功</color>");
            return true;
        }

        protected override void OnInit()
        {
            
        }
    }
}
