using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICombatSystem : ISystem
    {
        void CalculateAndApplyDamage(BattleUnitData attacker, BattleUnitData target, float ratio, int hitCount = 1);
        void ApplyFixedDamage(BattleUnitData target, int damage);
        void ApplyHeal(BattleUnitData attacker, BattleUnitData target, float ratio);
        void ApplyHeal(BattleUnitData target, int healAmount);
    }

    public class CombatSystem : AbstractSystem, ICombatSystem
    {
        private IBattleModel mModel;
        protected override void OnInit() => mModel = this.GetModel<IBattleModel>();
        public void CalculateAndApplyDamage(BattleUnitData attacker, BattleUnitData target, float ratio, int hitCount = 1)
        {
            if (attacker == null || target == null || !attacker.IsAlive || !target.IsAlive) return;
            int baseAtk = attacker.SourceInfo.GetFinalStat().Attack;
            int targetDef = target.SourceInfo.GetFinalStat().Defense;
            int singleDmg = Mathf.Max(1, (int)((baseAtk - targetDef) * ratio));
            target.Hp -= (singleDmg * hitCount);

            this.SendEvent(new UnitDamagedEvent { TargetId = target.UnitId, Damage = singleDmg, HitCount = hitCount });
            CheckDeath(target);
        }
        public void ApplyFixedDamage(BattleUnitData target, int damage)
        {
            if (target == null || !target.IsAlive) return;
            target.Hp -= damage;
            this.SendEvent(new UnitDamagedEvent { TargetId = target.UnitId, Damage = damage, HitCount = 1 });
            CheckDeath(target);
        }
        public void ApplyHeal(BattleUnitData attacker, BattleUnitData target,float ratio)
        {
            int baseAtk = attacker.SourceInfo.GetFinalStat().Attack;
            int targetMaxHp = target.SourceInfo.GetFinalStat().MaxHp;
            target.Hp = Mathf.Min(targetMaxHp, target.Hp + (int)(baseAtk * ratio));
            this.SendEvent(new UnitDamagedEvent { TargetId = target.UnitId, Damage = -(int)(baseAtk * ratio), HitCount = 1 });

        }
        public void ApplyHeal(BattleUnitData target, int healAmount)
        {
            if (target == null || !target.IsAlive) return;
            int maxHp = target.SourceInfo.GetFinalStat().MaxHp;
            // 防止溢出
            target.Hp = Mathf.Min(maxHp, target.Hp + healAmount);

            // 发飘字事件（用负数表示回血，或者自定义一个 UnitHealedEvent）
            this.SendEvent(new UnitDamagedEvent { TargetId = target.UnitId, Damage = -healAmount, HitCount = 1 });
        }
        private void CheckDeath(BattleUnitData target)
        {
            if (target.Hp <= 0)
            {
                target.Hp = 0;
                this.SendEvent(new UnitDieEvent { UnitId = target.UnitId });
                mModel.RemoveUnit(target);
                ActionKit.Delay(1.2f, () => {
                    if (mModel.Phase.Value != BattlePhase.GameOver)
                        this.GetSystem<IBattleTurnSystem>().CheckGameOver();
                }).StartGlobal();
            }
        }
    }
}
