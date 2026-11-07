using UnityEngine;

namespace Game
{
    public class UnitTurnStartAction : IBattleAction
    {
        private readonly BattleUnitData mUnit;
        private float mWaitTimer;
        private bool mIsLogicExecuted = false; // ★ 增加标志位

        public UnitTurnStartAction(BattleUnitData unit)
        {
            mUnit = unit;
        }

        public void OnStart()
        {
            mIsLogicExecuted = false;
            mWaitTimer = 0f;
        }

        public bool OnUpdate(float dt)
        {
            // 1. 逻辑只执行一次
            if (!mIsLogicExecuted)
            {
                mIsLogicExecuted = true;
                    if (mUnit != null && mUnit.IsAlive)
                    {
                        // 重置移动和行动状态
                        mUnit.HasActed = false;
                        mUnit.HasMoved = false;
                        mUnit.CurrentStep = mUnit.SourceInfo.GetFinalStat().Step;

                        // 扣减技能 CD
                        foreach (var skill in mUnit.skills)
                        {
                            if (skill.RemainingCd.Value > 0) skill.RemainingCd.Value--;
                        }

                        // ★ 触发回合开始 Buff（确保判空）
                        var buffSystem = GameApp.Interface.GetSystem<IBuffSystem>();
                        if (buffSystem != null)
                        {
                            buffSystem.TriggerTick(mUnit, BuffTickTiming.TurnStart);
                        }
                    }

                // ★ 核心修复：只在逻辑执行完时，设置等待时间
                mWaitTimer = 0.5f;
            }

            // 2. 计时器每帧递减
            mWaitTimer -= dt;

            return mWaitTimer <= 0;
        }

        public void OnCancel() { }
    }
}
