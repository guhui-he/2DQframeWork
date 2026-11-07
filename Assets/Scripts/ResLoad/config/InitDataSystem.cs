using System;
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
            mResModel = this.GetModel<IResModel>();
            InitRegionConfig();
            InitSprite();
            InitUIPanel();
            InitGamePlayEvent();
            InitDialogue();
            InitBuildData();
            InitFactionRelationData();
            InitPartyData();
            InitEffectData();
            InitVfxData();
            InitBuffData();
            InitSkillData();
            InitItemData();
            InitRoleData();
        }
        private void InitRegionConfig()
        {
            var model=this.GetModel<IRegionConfigModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Region);
            foreach (var row in table.GetAllRows())
            {
                model.AddRegion(ConfigParsers.ParseRegion(row));
            }
        }
        private void InitSprite()
        {
            mResModel.AddAtlasPath(AssetPathDef.AtlasArmor);
            mResModel.AddAtlasPath(AssetPathDef.AtlasHelmet);
            mResModel.AddAtlasPath(AssetPathDef.AtlasBoot);
            mResModel.AddAtlasPath(AssetPathDef.AtlasShield);
            mResModel.AddAtlasPath(AssetPathDef.AtlasWeapon);
            mResModel.AddAtlasPath(AssetPathDef.AtlasSupplies);
            mResModel.AddAtlasPath(AssetPathDef.AtlasRole);
        }
        private void InitUIPanel()
        {
            var uipanelModel = this.GetModel<IUIPanelConfigModel>();
            var table = mConfigModel.GetTable(ConfigConstants.UIPanel);
            foreach (var row in table.GetAllRows())
            {
                uipanelModel.AddConfig(ConfigParsers.ParseUIPanel(row));
            }
        }
        private void InitGamePlayEvent()
        {
            var eventModel=this.GetModel<IGamePlayEventModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Event);
            if (table == null) { Debug.LogError("City 表缺失"); return; }

            foreach (var row in table.GetAllRows())
            {
                if (!int.TryParse(row.GetValueOrDefault("Id"), out _)) continue;
                eventModel.AddConfig(ConfigParsers.ParseEvent(row));
            }
        }
        private void InitDialogue()
        {
            var dialogueModel = this.GetModel<IDialogueConfigModel>();
            var table=mConfigModel.GetTable(ConfigConstants.Dialogue);
            foreach (var row in table.GetAllRows())
            {
                dialogueModel.AddConfig(ConfigParsers.ParseDialogue(row));
            }
        }
        private void InitBuildData()
        {
            var buildModel = this.GetModel<ICityConfigModel>();
            var table = mConfigModel.GetTable(ConfigConstants.City);
            Debug.Log("city");
            if (table == null) { Debug.LogError("City 表缺失"); return; }

            foreach (var row in table.GetAllRows())
            {
                if (!int.TryParse(row.GetValueOrDefault("Id"), out _)) continue;
                buildModel.AddCityConfig(ConfigParsers.ParseBuild(row));
            }
        }
        private void InitFactionRelationData()
        {
            var relationConfigModel = this.GetModel<IRelationConfigModel>();
            var relationModel = this.GetModel<IRelationModel>();
            var table = mConfigModel.GetTable(ConfigConstants.FactionRelation);
            foreach (var row in table.GetAllRows())
            {
                relationConfigModel.AddConfig(ConfigParsers.ParseRelation(row));
            }
            relationModel.ResetToDefault(relationConfigModel);
        }
        private void InitPartyData()
        {
            var partyModel=this.GetModel<IPartyConfigModel>();
            var table=mConfigModel.GetTable(ConfigConstants.Party);
            foreach(var row in table.GetAllRows())
            {
                partyModel.AddConfig(ConfigParsers.ParseParty(row));
            }
        }
        private void InitRoleData()
        {
            var roleModel = this.GetModel<IRoleConfigModel>();
            var table=mConfigModel.GetTable(ConfigConstants.Role);
           foreach (var row in table.GetAllRows())
             {
             if (!int.TryParse(row.GetValueOrDefault("Id"), out _)) continue;
             roleModel.AddRole(ConfigParsers.ParseRole(row));
             }
            
        }

        private void InitEffectData()
        {
            var effectModel = this.GetModel<IEffectConfigModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Effect);
            if (table == null) { Debug.LogError("Effect 表缺失"); return; }

            foreach (var row in table.GetAllRows())
            {
                effectModel.AddEffect(ConfigParsers.ParseEffect(row));
            }
        }

        private void InitBuffData()
        {
            var model = this.GetModel<IBuffModel>();
            var table=mConfigModel.GetTable(ConfigConstants.Buff);
            foreach( var row in table.GetAllRows())
            {
                model.AddBuffConfig(ConfigParsers.ParseBuff(row));
            }
        }
        private void InitVfxData()
        {
            var model = this.GetModel<IVfxModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Vfx);
            if (table == null) { Debug.LogError("Vfx 表缺失"); return; }

            foreach (var row in table.GetAllRows())
            {
                model.AddVfxConfig(ConfigParsers.ParseVfx(row));
            }
        }

        private void InitSkillData()
        {
            var skillModel = this.GetModel<ISkillModel>();
            var effectModel = this.GetModel<IEffectConfigModel>();
            var table = mConfigModel.GetTable(ConfigConstants.Skill);
            if (table == null) { Debug.LogError("Skill 表缺失"); return; }

            foreach (var row in table.GetAllRows())
            {
                // 借用扩展方法校验 Id，避免解析崩溃
                if (!int.TryParse(row.GetValueOrDefault("Id"), out _)) continue;

                var skill = ConfigParsers.ParseSkill(row, effectModel);
                skillModel.AddSkill(new SkillInfo(skill));
            }
        }

        #region Item
        private void InitItemData()
        {
            var itemModel = this.GetModel<IItemConfigModel>();

            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                string tableKey = GetTableKey(type);
                if (string.IsNullOrEmpty(tableKey)) continue;

                var table = mConfigModel.GetTable(tableKey);
                if (table == null) continue;

                foreach (var row in table.GetAllRows())
                {
                    if (!int.TryParse(row.GetValueOrDefault("Id"), out _)) continue;
                    itemModel.AddItem(type, ConfigParsers.ParseItem(row, type));
                }
            }
        }

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
                default: return null;
            }
        }
        #endregion
    }
}
