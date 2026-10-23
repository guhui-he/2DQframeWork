using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRoleFactorySystem : ISystem
    {
        List<RoleInfo> CreateRoles(RoleType type, int count, bool buy = false);
        RoleInfo CreateRole(RoleType type,bool buy=false);
        RoleInfo CreateRole(RoleType type, int id, bool buy = false);
        RoleInfo CreateRole(RoleType type, int id, int level, bool buy = false);
    }

    public class RoleFactorySystem : AbstractSystem, IRoleFactorySystem
    {
        private IRoleConfigModel mRoleModel;
        private TalentConfig mTalentConfig;

        protected override void OnInit()
        {
            mRoleModel = this.GetModel<IRoleConfigModel>();

            var loader = ResLoader.Allocate();
            mTalentConfig = loader.LoadSync<TalentConfig>("TalentConfig");
        }

        public RoleInfo CreateRole(RoleType type, bool buy = false)
        {            
            var idx=Random.Range(0,mRoleModel.GetRoles(type).Count);
            foreach (var kv in mRoleModel.GetRoles(type))
            {
                if (idx-- == 0) 
                    return CreateRole(type,kv.Value.Id);
            }
            return null;
        }

        public  List<RoleInfo> CreateRoles(RoleType type, int count, bool buy = false)
        {
            var roles = new List<RoleInfo>();
            for (int i = 0; i < count; i++)
            {
                roles.Add(CreateRole(type));
            }
            return roles;
        }
        public RoleInfo CreateRole(RoleType type, int id, bool buy = false)
            => CreateRole(type, id, level: 1,buy);

        public RoleInfo CreateRole(RoleType type, int id, int level,bool buy=false)
        {
            var config = mRoleModel.GetRole(type, id);

            var data = new RoleInfo
            {
                Id = IdGenerator.Next(),
                Name = config.Name,
                Info = config.Info,
                Icon = config.Icon,
                Model = config.Model,
                Level = level,
                isBuy = buy,
                AtkRange = 1,
                Curexp = 0,
                PendingPoints = 3 * (level - 1),
            };

            // 1. 随机基础属性
            RollBaseStats(data, config);

            // 2. 随机天赋
            data.Talents = RollTalents();

            // 3 随机装备
            data.Equipment=this.GetSystem<IEquipmentFactorySystem>().CreateRandom(data.Equipment);

            data.CurHp = data.MaxHp;

            return data;
        }

        // ==================== 基础属性 ====================

        private void RollBaseStats(RoleInfo data, RoleConfigData config)
        {
            data.MaxHp = Random.Range(config.HpMin, config.HpMax + 1);
            data.Atk = Random.Range(config.AtkMin, config.AtkMax + 1);
            data.Def = Random.Range(config.DefMin, config.DefMax + 1);
            data.Step = Random.Range(config.StepMin, config.StepMax + 1);
            data.Crit = Random.Range(config.CritMin, config.CritMax + 1);
            data.Avd = Random.Range(config.AvdMin, config.AvdMax + 1);
        }

        // ==================== 天赋 ====================

        private Dictionary<StatType, int> RollTalents()
        {
            var talents = new Dictionary<StatType, int>();
            var pool = new List<StatType>((StatType[])System.Enum.GetValues(typeof(StatType)));

            // 全部先给低星
            foreach (var s in pool)
                talents[s] = Random.Range(mTalentConfig.LowStarRange.x, mTalentConfig.LowStarRange.y + 1);

            // 挑 N 个提升为高星
            int highCount =Tools.Roll(0,Mathf.Min(mTalentConfig.HighStarCount, pool.Count),0.1f);
            for (int i = 0; i < highCount; i++)
            {
                int idx = Random.Range(0, pool.Count);
                var stat = pool[idx];
                pool.RemoveAt(idx);

                talents[stat] = Tools.Roll(
                    mTalentConfig.HighStarRange.x,
                    mTalentConfig.HighStarRange.y + 1,0.1f);
            }
            return talents;
        }

    }
}
