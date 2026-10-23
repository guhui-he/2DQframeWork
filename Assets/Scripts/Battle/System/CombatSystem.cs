using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICombatSystem : ISystem
    {
        void ExecuteAttackDirect(BattleUnitData attacker, BattleUnitData target);
    }

    public class CombatSystem : AbstractSystem, ICombatSystem
    {
        private IBattleModel mModel;

        protected override void OnInit() => mModel = this.GetModel<IBattleModel>();

        public void ExecuteAttackDirect(BattleUnitData attacker, BattleUnitData target)
        {
            if (attacker == null || target == null || !attacker.IsAlive || !target.IsAlive) return;
            if (target.IsPlayer == attacker.IsPlayer || attacker.Pos.ManhattanTo(target.Pos) != 1) return;

            int dmg = Mathf.Max(1, attacker.SourceInfo.Atk - target.SourceInfo.Def);
            target.Hp -= dmg;
            this.SendEvent(new UnitDamagedEvent { TargetId = target.UnitId, Damage = dmg });

            if (target.Hp <= 0)
            {
                
                this.SendEvent(new UnitDieEvent { UnitId = target.UnitId });
                mModel.RemoveUnit(target);
                ActionKit.Delay(1.2f, () =>
                {
                    if (mModel.Phase.Value != BattlePhase.GameOver)
                    {
                        this.GetSystem<IBattleTurnSystem>().CheckGameOver();
                    }
                }).StartGlobal();
            }
        }
    }
}
