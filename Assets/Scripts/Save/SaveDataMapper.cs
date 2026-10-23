using System.Collections.Generic;
using Game;
using UnityEngine;


namespace Game
{
    
    public static class RoleSaveMapper
    {
        public static MapEnemySaveData ToSaveData(this EnemyIconData data)
        {
            return new MapEnemySaveData
            {
                Id = data.Id,
                EnemyId = data.EnemyId,
                SpawnId = data.SpawnId,
                CreatTimer=data.CreatTimer,
                HasSpawnedInitialMembers = data.HasSpawnedInitialMembers,
                BornX = data.BornPosition.x,
                BornY = data.BornPosition.y,
                BornZ = data.BornPosition.z,
                TargetX = data.TargetPosition.Value.x,
                TargetY = data.TargetPosition.Value.y,
                TargetZ = data.TargetPosition.Value.z,
                CurrentX = data.CurrentPosition.Value.x,
                CurrentY = data.CurrentPosition.Value.y,
                CurrentZ = data.CurrentPosition.Value.z,
                LastKnownPlayerX = data.LastKnownPlayerPos.Value.x,
                LastKnownPlayerY = data.LastKnownPlayerPos.Value.y,
                LastKnownPlayerZ = data.LastKnownPlayerPos.Value.z,
                PerceptionResult = (int)data.PerceptionResult.Value,
                MoveSpeed = data.MoveSpeed
            };
        }

        public static EnemyIconData FromSaveData(this MapEnemySaveData saveData)
        {
            var data = new EnemyIconData(
                saveData.Id, saveData.EnemyId, saveData.SpawnId,
                new Vector3(saveData.BornX, saveData.BornY, saveData.BornZ),
                saveData.MoveSpeed
            );
            data.CreatTimer = saveData.CreatTimer;
            data.HasSpawnedInitialMembers= saveData.HasSpawnedInitialMembers;
            data.TargetPosition.Value = new Vector3(saveData.TargetX, saveData.TargetY, saveData.TargetZ);
            data.CurrentPosition.Value = new Vector3(saveData.CurrentX, saveData.CurrentY, saveData.CurrentZ);
            data.LastKnownPlayerPos.Value = new Vector3(saveData.LastKnownPlayerX, saveData.LastKnownPlayerY, saveData.LastKnownPlayerZ);
            data.PerceptionResult.Value = (PerceptionType)saveData.PerceptionResult;

            return data;
        }
        public static RoleInfoSaveData ToSaveData(this RoleInfo role)
        {
            var save = new RoleInfoSaveData
            {
                Id = role.Id,
                Name = role.Name,
                Info = role.Info,
                Icon = role.Icon,
                Model = role.Model,
                MaxHp = role.MaxHp,
                CurHp = role.CurHp,
                Atk = role.Atk,
                Def = role.Def,
                Step = role.Step,
                Crit = role.Crit,
                Avd = role.Avd,
                AtkRange = role.AtkRange,
                isBuy = role.isBuy,
                Level = role.Level,
                Curexp = role.Curexp,
                PendingPoints = role.PendingPoints
            };

            // 存天赋
            foreach (var kv in role.Talents)
                save.Talents.Add(new IntIntPair { Key = (int)kv.Key, Value = kv.Value });

            // 存装备
            foreach (var kv in role.Equipment.Slots)
            {
                var item = kv.Value.Value;
                if (item != null)
                {
                    var itemSave = new ItemInfoSaveData
                    {
                        ConfigId = item.itemData.Id,
                        Area = (int)item.area
                    };
                    foreach (var affix in item.Affixes)
                    {
                        itemSave.Affixes.Add(new ItemAffixSaveData
                        {
                            Stat = (int)affix.Stat,
                            Value = affix.Value
                        });
                    }
                    save.Equipments.Add(new IntItemPair { Key = (int)kv.Key, Value = itemSave });
                }
            }
            return save;
        }

        public static RoleInfo FromSaveData(this RoleInfoSaveData save)
        {
            var role = new RoleInfo
            {
                Id = save.Id,
                Name = save.Name,
                Info = save.Info,
                Icon = save.Icon,
                Model = save.Model,
                MaxHp = save.MaxHp,
                CurHp = save.CurHp,
                Atk = save.Atk,
                Def = save.Def,
                Step = save.Step,
                Crit = save.Crit,
                Avd = save.Avd,
                AtkRange = save.AtkRange,
                isBuy = save.isBuy,
                Level = save.Level,
                Curexp = save.Curexp,
                PendingPoints = save.PendingPoints
            };

            // 恢复天赋
            if (save.Talents != null)
            {
                foreach (var pair in save.Talents)
                    role.Talents[(StatType)pair.Key] = pair.Value;
            }

            // 恢复装备
            if (save.Equipments != null)
            {
                foreach (var pair in save.Equipments)
                {
                    var itemType = (ItemType)pair.Key;
                    var itemSave = pair.Value;

                    var configData = GameApp.Interface.GetModel<IItemConfigModel>().GetItem(itemSave.ConfigId);
                    if (configData != null)
                    {
                        var item = new ItemInfo();
                        item.SetBaseData(configData);
                        item.area = (AreaType)itemSave.Area;

                        if (itemSave.Affixes != null)
                        {
                            foreach (var affixSave in itemSave.Affixes)
                                item.Affixes.Add(new ItemAffix((AffixStatType)affixSave.Stat, affixSave.Value));
                        }

                        role.Equipment.SetEquipped(itemType, item);
                    }
                }
            }
            return role;
        }
    }
}
