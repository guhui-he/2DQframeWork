

namespace Game
{
    public class BuffTickAction : IBattleAction
    {
        private readonly BattleUnitData mTarget;
        private readonly BuffTickTiming mTiming;
        private float mWaitTimer;
        private bool mIsLogicExecuted = false;

        public BuffTickAction(BattleUnitData mTarget, BuffTickTiming mTiming)
        {
            this.mTarget = mTarget;
            this.mTiming = mTiming;
        }
        public void OnStart()
        {
            mIsLogicExecuted=false;
        }
        public bool OnUpdate(float dt)
        {
            if(!mIsLogicExecuted)
            {
                mIsLogicExecuted = true;
                var buffSystem = GameApp.Interface.GetSystem<IBuffSystem>();
                buffSystem.TriggerTick(mTarget, mTiming);

                mWaitTimer = 0.5f; // 飘字和受击表现停留时间
            }
            mWaitTimer -= dt;
            if (mWaitTimer <= 0)
            {
                return true; 
            }
            return false;
        }
        public void OnCancel() { }
    }
}
