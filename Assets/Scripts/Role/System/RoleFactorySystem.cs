using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRoleFactorySystem : ISystem
    {
        List<RoleInfo> CreateRoles(int count, bool buy = false);
        RoleInfo CreateRole(bool buy=false);
        RoleInfo CreateRole(int id, bool buy = false);
        RoleInfo CreateRole(int id, int level, bool buy = false);
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

        public RoleInfo CreateRole(bool buy = false)
        {            
            var idx=Random.Range(0,mRoleModel.GetRoles().Count);
            foreach (var kv in mRoleModel.GetRoles())
            {
                if (idx-- == 0) 
                    return CreateRole(kv.Value.Id);
            }
            return null;
        }

        public  List<RoleInfo> CreateRoles(int count, bool buy = false)
        {
            var roles = new List<RoleInfo>();
            for (int i = 0; i < count; i++)
            {
                roles.Add(CreateRole());
            }
            return roles;
        }
        public RoleInfo CreateRole(int id, bool buy = false)
            => CreateRole(id, level: 1,buy);

        public RoleInfo CreateRole(int id, int level,bool buy=false)
        {
            var config = mRoleModel.GetRole(id);

            var data = new RoleInfo
            {
                Id = IdGenerator.Next(),
                Name = config.Name,
                Info = config.Info,
                Icon = config.Icon,
                Model = config.Model,
                Ani=config.Ani,
                Level = level,
                isBuy = buy,
                Curexp = 0, 
            };
            data.PendingPoints.Value= 3 * (level - 1);
            // 1. 随机基础属性
            RollBaseStats(data, config);

            // 2. 随机天赋
            data.Talents = RollTalents();
            
            // 3 随机装备
            data.Equipment=this.GetSystem<IEquipmentFactorySystem>().CreateRandom(data.Equipment);
            // 4.技能(普通攻击)
            data.mSkillIds.Add(1002);
            data.CurHp = data.GetFinalStat().MaxHp;
            return data;
        }

        // ==================== 基础属性 ====================

        private void RollBaseStats(RoleInfo data, RoleConfigData config)
        {
            data.stat.Add(new StatAffix(AffixStatType.MaxHp, Random.Range(config.HpMin, config.HpMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.Attack, Random.Range(config.AtkMin, config.AtkMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.Defense, Random.Range(config.StepMin, config.StepMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.Step, Random.Range(config.StepMin, config.StepMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.Crit, Random.Range(config.CritMin, config.CritMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.Avd, Random.Range(config.AvdMin, config.AvdMax + 1)));
            data.stat.Add(new StatAffix(AffixStatType.AtkRange, 2));
        }

        // ==================== 天赋 ====================

        private Dictionary<AffixStatType, int> RollTalents()
        {
            var talents = new Dictionary<AffixStatType, int>();
            var pool = new List<AffixStatType>((AffixStatType[])System.Enum.GetValues(typeof(AffixStatType)));

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
