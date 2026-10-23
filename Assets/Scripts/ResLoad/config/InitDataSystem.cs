using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IInitDataSystem : ISystem { }

    public class InitDataSystem : AbstractSystem, IInitDataSystem
    {
        private IConfigModel mConfigModel;
        private IResModel mResModel;
        protected override void OnInit()
        {
            mConfigModel = this.GetModel<IConfigModel>();
            mResModel=this.GetModel<IResModel>();

            InitSpawnEnemyData();
            InitSprite();
            InitBuildData();
            InitItemData();  
            InitRoleData();
        }
        private void InitSpawnEnemyData()
        {
            var model = this.GetModel<ISpawnEnemyModel>();
            var table = mConfigModel.GetTable(ConfigConstants.SpawnEnemy);
            foreach (var row in table.GetAllRows())
            {
                if (!int.TryParse(row.GetValueOrDefault("Id"), out int id)) continue;

                var data = new SpawnEnemyData { Id = id };
                data.Name = row.GetValueOrDefault("Name", "");
                
                data.Des = row.GetValueOrDefault("Des", "");
                data.SceneName = row.GetValueOrDefault("SceneName", "");
                data.EnemyId=ParseInt(row,"enemyId");
                model.AddSpawnEnemy(data);
            }
        }
        private void InitSprite()
        {
            mResModel.AddAtlasPath(AssetPathDef.AtlasArmor);
            mResModel.AddAtlasPath(AssetPathDef.AtlasHelmet);
            mResModel.AddAtlasPath (AssetPathDef.AtlasBoot);
            mResModel.AddAtlasPath(AssetPathDef.AtlasShield);
            mResModel.AddAtlasPath(AssetPathDef.AtlasWeapon);
            mResModel.AddAtlasPath(AssetPathDef.AtlasSupplies);
            mResModel.AddAtlasPath(AssetPathDef.AtlasRole);
        }
        private void InitBuildData()
        {
            var buildModel = this.GetModel<IBuildModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Town);
            if (table == null) { Debug.LogError("Town 表缺失"); return; }
            foreach (var row in table.GetAllRows())
            {
                if (!int.TryParse(row.GetValueOrDefault("Id"), out int id)) continue;

                var data = new BuildData { Id = id };
                data.Name = row.GetValueOrDefault("Name", "");
                data.Des = row.GetValueOrDefault("Des", "");
                buildModel.AddBuildData(data);
            }
        }
        private void InitRoleData()
        {
            var roleModel = this.GetModel<IRoleConfigModel>();

            foreach (RoleType type in System.Enum.GetValues(typeof(RoleType)))
            {
                string tableKey = type == RoleType.Hero ? ConfigConstants.Hero : ConfigConstants.Enemy;
                var table = mConfigModel.GetTable(tableKey);
                if (table == null) { Debug.LogWarning($"[InitDataSystem] {type} 表缺失"); continue; }

                foreach (var row in table.GetAllRows())
                {
                    if (!int.TryParse(row.GetValueOrDefault("Id"), out int id)) continue;
                    roleModel.AddRole(type, new RoleConfigData
                    {
                        Id = id,
                        Name = row.GetValueOrDefault("Name", ""),
                        Icon = row.GetValueOrDefault("Icon", ""),
                        Model = row.GetValueOrDefault("Model", ""),
                        Info = row.GetValueOrDefault("Info", ""),

                        // ==================== 基础属性范围 ====================
                        HpMin = ParseInt(row, "HpMin"),
                        HpMax = ParseInt(row, "HpMax"),
                        AtkMin = ParseInt(row, "AtkMin"),
                        AtkMax = ParseInt(row, "AtkMax"),
                        DefMin = ParseInt(row, "DefMin"),
                        DefMax = ParseInt(row, "DefMax"),
                        StepMin = ParseInt(row, "StepMin"),
                        StepMax = ParseInt(row, "StepMax"),
                        CritMin = ParseInt(row, "CritMin"),
                        CritMax = ParseInt(row, "CritMax"),
                        AvdMin = ParseInt(row, "AvdMin"),
                        AvdMax = ParseInt(row, "AvdMax"),
                    });
                }
            }
        }
        #region Item
        private void InitItemData()
        {
            var itemModel = this.GetModel<IItemConfigModel>();

            // 遍历枚举里的每一个类型
            foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
            {
                string tableKey = GetTableKey(type);
                if (string.IsNullOrEmpty(tableKey)) continue;

                var table = mConfigModel.GetTable(tableKey);
                int count = 0;
                foreach (var row in table.GetAllRows())
                {
                    var data = ParseItem(type, row);
                    if (data == null) continue;

                    itemModel.AddItem(type, data);
                    count++;
                }
            }
        }

        /// <summary>枚举 → 配置表 key</summary>
        private string GetTableKey(ItemType type)
        {
            switch (type)
            {
                case ItemType.Weapon: return ConfigConstants.Weapon;
                case ItemType.Shield: return ConfigConstants.Shield;
                case ItemType.Helmet: return ConfigConstants.Helmet;
                case ItemType.Armor: return ConfigConstants.Armor;
                case ItemType.Boot: return ConfigConstants.Boot;
                case ItemType.Supplie: return ConfigConstants.Supplie;
            }
            return null;
        }

        private ItemConfigData ParseItem(ItemType type, Dictionary<string, string> row)
        {
            if (!int.TryParse(row.GetValueOrDefault("Id"), out int id))
            {
                Debug.LogWarning($"[InitDataSystem] {type} 有非法 Id 的行，跳过");
                return null;
            }

            return new ItemConfigData
            {
                Id = id,
                Type = type,
                Name = row.GetValueOrDefault("Name", ""),
                Icon = row.GetValueOrDefault("Icon", ""),
                Price = ParseInt(row, "Price"),
                Slot = row.GetValueOrDefault("Slot", ""),
                Level = ParseInt(row, "Level"),
            };
        }
        #endregion


        private int ParseInt(Dictionary<string, string> row, string key)
            => int.TryParse(row.GetValueOrDefault(key), out var v) ? v : 0;
    }
}
