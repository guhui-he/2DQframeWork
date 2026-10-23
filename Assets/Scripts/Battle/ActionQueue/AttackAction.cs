using QFramework;
using UnityEngine;

namespace Game
{
    public class AttackAction : IBattleAction
    {
        private enum Step { Start, WaitBeforeHit, WaitAfterHit, Done }

        private readonly BattleUnitData mAttacker;
        private readonly BattleUnitData mTarget;
        private Step mStep;
        private float mTimer;

        public AttackAction(BattleUnitData attacker, BattleUnitData target)
        {
            mAttacker = attacker;
            mTarget = target;
        }

        public void OnStart()
        {
            mStep = Step.Start;
        }

        public bool OnUpdate(float dt)
        {
            if (mStep == Step.Done) return true;

            var model = GameApp.Interface.GetModel<IBattleModel>();
            if (model.Phase.Value == BattlePhase.GameOver) { Done(); return true; }
            if (mAttacker == null || !mAttacker.IsAlive || mTarget == null || !mTarget.IsAlive)
            {
                Done(); return true;
            }

            switch (mStep)
            {
                case Step.Start:
                    // 1. 广播前摇（View 层播挥剑动画）
                    GameApp.Interface.SendEvent(new UnitAttackEvent { AtkId = mAttacker.UnitId });
                    mTimer = 1.3f; // 等挥剑动画播完
                    mStep = Step.WaitBeforeHit;
                    break;

                case Step.WaitBeforeHit:
                    mTimer -= dt;
                    if (mTimer <= 0)
                    {
                        // 2. 动画到位，瞬间结算伤害
                        GameApp.Interface.GetSystem<ICombatSystem>().ExecuteAttackDirect(mAttacker, mTarget);
                        mTimer = 0.3f; // 受击后摇/顿帧
                        mStep = Step.WaitAfterHit;
                    }
                    break;

                case Step.WaitAfterHit:
                    mTimer -= dt;
                    if (mTimer <= 0)
                    {
                        GameApp.Interface.SendEvent(new UnitDamagedCompletedEvent { TargetId  = mTarget.UnitId });
                        Done();
                    }
                    break;
            }

            return mStep == Step.Done;
        }

        public void OnCancel() => Done();

        private void Done()
        {
            // 攻击完毕，标记结束
            if (mAttacker != null && mAttacker.IsAlive)
            {
                mAttacker.HasActed = true;
                mAttacker.HasMoved = true;
            }
            mStep = Step.Done;
        }
    }
}
