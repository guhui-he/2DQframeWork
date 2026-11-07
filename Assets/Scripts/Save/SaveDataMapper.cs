using System.Collections.Generic;
using Game;
using UnityEngine;


namespace Game
{
    
    public static class RoleSaveMapper
    {
        public static PartySaveData ToSaveData(this PartyInfo data)
        {
            return new PartySaveData
            {
                PartyId = data.PartyId,
                PartyConfigId = data.Config.Id, 
                Type = (int)data.Type,
                OwnerId = data.OwnerId,
                Faction=(int)data.Faction,
                RolePool = data.RolePool,
                HasSpawnedInitialMembers = data.HasSpawnedInitialMembers,
                CreatTimer = data.CreatTimer,
                State = (int)data.State,
                StateTimer = data.StateTimer,
                BornX = data.BornPosition.x,
                BornY = data.BornPosition.y,
                BornZ = data.BornPosition.z,
                CurrentX = data.CurrentPosition.Value.x,
                CurrentY = data.CurrentPosition.Value.y,
                //CurrentZ = data.CurrentPosition.Value.z,
                TargetX = data.TargetPosition.Value.x,
                TargetY = data.TargetPosition.Value.y,
                //TargetZ = data.TargetPosition.Value.z
            };
        }

        public static PartyInfo FromSaveData(this PartySaveData saveData)
        {
            var config = GameApp.Interface.GetModel<IPartyConfigModel>().GetConfig(saveData.PartyConfigId);
            var data = new PartyInfo(
                saveData.PartyId,
                config,               
                saveData.OwnerId,
                (Faction)saveData.Faction,
                saveData.RolePool,
                new Vector3(saveData.BornX, saveData.BornY, saveData.BornZ)
            );

            data.HasSpawnedInitialMembers = saveData.HasSpawnedInitialMembers;
            data.CreatTimer = saveData.CreatTimer;
            data.State = (PartyState)saveData.State;
            data.StateTimer = saveData.StateTimer;
            data.CurrentPosition.Value = new Vector3(saveData.CurrentX, saveData.CurrentY, saveData.CurrentZ);
            data.TargetPosition.Value = new Vector3(saveData.TargetX, saveData.TargetY, saveData.TargetZ);

            return data;
        }
        public static SettlementSaveData ToSaveData(this SettlementInfo data)
        {
            return new SettlementSaveData
            {
                Id = data.Id,
                Type = (int)data.Type,
                PosX = data.Pos.x,
                PosY = data.Pos.y,
                RegionId = data.RegionId,
                BuildId = data.BuildId,
                SpawnTimer = data.SpawnTimer,
                SpawnInterval = data.SpawnInterval
            };
        }

        public static SettlementInfo FromSaveData(this SettlementSaveData saveData)
        {
            var cityConfigModel = GameApp.Interface.GetModel<ICityConfigModel>();
            var cityConfig = cityConfigModel?.GetCityConfig(saveData.BuildId);

            return new SettlementInfo
            {
                Id = saveData.Id,
                Type = (SettlementType)saveData.Type,
                Pos = new Vector2Int(saveData.PosX, saveData.PosY),
                RegionId = saveData.RegionId,
                BuildId = saveData.BuildId,
                SpawnTimer = saveData.SpawnTimer,
                SpawnInterval = saveData.SpawnInterval,
                config = cityConfig // ★ 重新赋予配置表数据
            };
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
                CurHp = role.CurHp,
                isBuy = role.isBuy,
                Level = role.Level,
                Curexp = role.Curexp,
                PendingPoints = role.PendingPoints.Value,
                SkillPoints = role.SkillPoints.Value,
                
            };
            foreach(var kv in role.stat)
                save.Stats.Add(new AffixSaveData { Stat=(int)kv.Stat,Value = kv.Value});
            // 存天赋
            foreach (var kv in role.Talents)
                save.Talents.Add(new IntIntPair { Key = (int)kv.Key, Value = kv.Value });
            // 存技能
            for (int i = 0; i < role.mSkillIds.Count; i++)
                save.SkillIds.Add(role.mSkillIds[i]);
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
                        itemSave.Affixes.Add(new AffixSaveData
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
                CurHp = save.CurHp,
                isBuy = save.isBuy,
                Level = save.Level,
                Curexp = save.Curexp,              
            };
            role.PendingPoints.Value= save.PendingPoints;
            role.SkillPoints.Value= save.SkillPoints;
            if (save.Stats != null)
            {
                foreach(var pair in save.Stats) 
                    role.stat.Add(new StatAffix((AffixStatType)pair.Stat, pair.Value));
            }
            // 恢复天赋
            if (save.Talents != null)
            {
                foreach (var pair in save.Talents)
                    role.Talents[(AffixStatType)pair.Key] = pair.Value;
            }
            // 恢复技能
            if(save.SkillIds != null)
            {
                for (var i = 0; i < save.SkillIds.Count; i++)
                    role.mSkillIds.Add(save.SkillIds[i]);   
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
                                item.Affixes.Add(new StatAffix((AffixStatType)affixSave.Stat, affixSave.Value));
                        }

                        role.Equipment.SetEquipped(itemType, item);
                    }
                }
            }
            return role;
        }
    }
}
