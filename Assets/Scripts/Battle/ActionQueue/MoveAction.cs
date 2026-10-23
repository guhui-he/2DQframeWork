using QFramework;

namespace Game
{
    public class MoveAction : IBattleAction
    {
        private readonly BattleUnitData mUnit;
        private readonly GridPos mTarget;
        private bool mIsDone = false;
        private IUnRegister mUnregister;

        public MoveAction(BattleUnitData unit, GridPos target)
        {
            mUnit = unit;
            mTarget = target;
        }

        public void OnStart()
        {
            var battleSystem = GameApp.Interface.GetSystem<IBattleSystem>();

            // 1. 纯逻辑计算、校验、改数据
            if (!battleSystem.CalculateAndApplyMove(mUnit, mTarget, out bool needWaitAnimation))
            {
                mIsDone = true; // 校验失败（比如被挡了），直接结束
                return;
            }
            if (!needWaitAnimation)
            {
                mIsDone = true; // 原地不动，直接结束 Action
                return;
            }
            // 2. 订阅 View 动画播放完成事件
            mUnregister = GameApp.Interface.RegisterEvent<UnitMoveCompletedEvent>(e =>
            {
                if (e.UnitId == mUnit.UnitId) mIsDone = true;
            });
        }

        public bool OnUpdate(float dt)
        {
            if (mIsDone)
            {
                mUnregister?.UnRegister();
                return true;
            }
            return false;
        }

        public void OnCancel() => mUnregister?.UnRegister();
    }
}
