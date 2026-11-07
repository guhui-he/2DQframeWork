using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ISkillEffect
    {
        void Execute(BattleUnitData caster, List<BattleUnitData> targets, GridPos targetPos);
    }
    public class DamageEffect : ISkillEffect
    {
        public float Ratio;
        public void Execute(BattleUnitData caster, List<BattleUnitData> targets,GridPos targetPos)
        {
            var combat = GameApp.Interface.GetSystem<ICombatSystem>();
            foreach (var target in targets)
            {
                combat.CalculateAndApplyDamage(caster, target, Ratio);
            }
        }
    }
    public class MultiHitDamageEffect : ISkillEffect
    {
        public float Ratio;
        public int HitCount = 1;

        public void Execute(BattleUnitData caster, List<BattleUnitData> targets, GridPos targetPos)
        {
            var combat = GameApp.Interface.GetSystem<ICombatSystem>();
            foreach (var target in targets)
            {
              combat.CalculateAndApplyDamage(caster, target, Ratio,HitCount);

            }
        }
    }
    public class HealEffect : ISkillEffect
    {
        public float Ratio;
        public void Execute(BattleUnitData caster, List<BattleUnitData> targets,GridPos targetPos)
        {
            var combatSystem = GameApp.Interface.GetSystem<ICombatSystem>();
            foreach (var target in targets)
            {
                combatSystem.ApplyHeal(caster, target,Ratio);
            }
        }
    }
    public class AddBuffEffect : ISkillEffect
    {
        public int BuffId;
        public int Duration;        
        public void Execute(BattleUnitData caster, List<BattleUnitData> targets, GridPos targetPos)
        {
            var system= GameApp.Interface.GetSystem<IBuffSystem>();
            foreach (var target in targets)
            {
               system.AddBuff(target, BuffId);
                Debug.Log($"给 {target.SourceInfo.Name} 添加了 Buff:{BuffId}，持续 {Duration} 回合");
            }
        }
    }
    public class SummonEffect : ISkillEffect
    {
        public int SummonUnitId;

        public void Execute(BattleUnitData caster, List<BattleUnitData> targets, GridPos targetPos)
        {

            var RoleInfo = GameApp.Interface.GetSystem<IRoleFactorySystem>().CreateRole(SummonUnitId);
            Debug.Log($"在 {targetPos.Row},{targetPos.Col} 召唤了单位 {SummonUnitId}");
            GameApp.Interface.SendCommand(new SpawnUnitCommand(RoleInfo, targetPos, caster.IsPlayer));
        }
    }
    
}
