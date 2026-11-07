using QFramework;
using UnityEngine;

namespace Game
{
    public class UnitTurnEndAction : IBattleAction
    {
        private readonly BattleUnitData mUnit;
        private float mWaitTimer;
        private bool mIsLogicExecuted = false;

        public UnitTurnEndAction(BattleUnitData unit)
        {
            mUnit = unit;
        }

        public void OnStart()
        {
            mIsLogicExecuted = false;
        }

        public bool OnUpdate(float dt)
        {
            // 1. 瞬间执行逻辑
            if (!mIsLogicExecuted)
            {
                mIsLogicExecuted = true;

                if (mUnit != null && mUnit.IsAlive)
                {
                    var buffSystem = GameApp.Interface.GetSystem<IBuffSystem>();

                    // ★ 1. 触发回合结束 Buff（比如中毒掉血、流血）
                    buffSystem.TriggerTick(mUnit, BuffTickTiming.TurnEnd);

                    // ★ 2. 扣减所有 Buff 的持续时间
                    buffSystem.DecreaseTurn(mUnit);
                }

                // 表现等待
                mWaitTimer = 0.5f;
            }

            // 2. 表现等待
            mWaitTimer -= dt;
            return mWaitTimer <= 0;
        }

        public void OnCancel() { }
    }
}
