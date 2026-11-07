using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRoleUpgradSystem:ISystem
    {
        void Upgrade(int id);
        void UpgradeStat(int id, AffixStatType type);
        void AutoUpgradeStat(int enemyId, int id);
        void UpgradeSkill(int skillId);
    }
    public class RoleUpgradSystem : AbstractSystem, IRoleUpgradSystem
    {
        IHeroModel Hmodel;
        IEnemyModel Emodel;
        protected override void OnInit()
        {           
            Hmodel=this.GetModel<IHeroModel>();
            Emodel=this.GetModel<IEnemyModel>();
        }
        public void Upgrade(int id)
        {
            var hero=Hmodel.GetHero(id);
            hero.Curexp -= hero.Level * 100;
            hero.Level++;
            hero.PendingPoints.Value += 3;
            if (hero.Level % 5 == 0)
                hero.SkillPoints.Value += 1;
        }
        //点击升级
        public void UpgradeStat(int id,AffixStatType type)
        {
            var role = Hmodel.GetHero(id);
            var existing = role.stat.Find(s => s.Stat == type);
            existing.Value += role.Talents[type];
            role.PendingPoints.Value--;
        }
        //怪物自动升级
        public void AutoUpgradeStat(int enemyId, int id)
        {
            var role = Emodel.GetRoleInfo(enemyId, id);
            role.Curexp -= role.Level * 100;
            role.Level++;
            role.PendingPoints.Value += 3;
            if(role.Level % 5 == 0)
                role.SkillPoints.Value += 1;
            int pointCount = role.PendingPoints.Value;

            var candidates = new List<AffixStatType>();
            foreach (var kv in role.Talents)
                candidates.Add(kv.Key);
            candidates.Sort((a, b) => role.Talents[b].CompareTo(role.Talents[a]));
            var pool = candidates.GetRange(0, 3);
            for (int i = 0; i < pointCount; i++)
            {
                var pick = pool[Random.Range(0, pool.Count)];
                int add = role.Talents[pick];

                var existing = role.stat.Find(s => s.Stat == pick);
                    existing.Value += add;
                role.PendingPoints.Value--;
            }
        }

        public void UpgradeSkill(int skillId)
        {
            var hero = Hmodel.GetCurHero();
            hero.mSkillIds.Add(skillId);
            hero.SkillPoints.Value -= 1;
        }
    }
}
