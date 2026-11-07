using QFramework;


namespace Game
{
    public enum MoveStyle { GridPath, Dash, Jump }
    public class MoveAction : IBattleAction
    {
        private readonly BattleUnitData mUnit;
        private readonly GridPos mTarget;
        private readonly MoveStyle mStyle;
        private readonly bool mIsSkillMove;
        private bool mIsDone = false;
        private IUnRegister mUnregister;

        public MoveAction(BattleUnitData unit, GridPos target, MoveStyle style = MoveStyle.GridPath, bool isSkillMove = false)
        {
            mUnit = unit; mTarget = target; mStyle = style; mIsSkillMove = isSkillMove;
        }

        public void OnStart()
        {
            var battleSystem = GameApp.Interface.GetSystem<IBattleSystem>();

            // 瞬间修改坐标并发事件
            if (!battleSystem.CalculateAndApplyMove(mUnit, mTarget, mStyle, mIsSkillMove, out bool needWaitAnimation))
            {
                mIsDone = true; return;
            }
            if (!needWaitAnimation) { mIsDone = true; return; }

            mUnregister = GameApp.Interface.RegisterEvent<UnitMoveCompletedEvent>(e =>
            {
                if (e.UnitId == mUnit.UnitId) mIsDone = true;
            });
        }

        public bool OnUpdate(float dt) { if (mIsDone) { mUnregister?.UnRegister(); return true; } return false; }
        public void OnCancel() => mUnregister?.UnRegister();
    }
}
