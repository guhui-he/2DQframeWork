using QFramework;

namespace Game
{
    // BattleCommands.cs
    public class StartGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            // 把指令交给 System 处理
            this.GetSystem<IBattleTurnSystem>().StartGame();
        }
    }
    // ==========================================
    // 1. 选中单位（玩家点击自己的英雄）
    // ==========================================
    public class SelectUnitCommand : AbstractCommand
    {
        private readonly BattleUnitData mUnit;
        public SelectUnitCommand(BattleUnitData unit) => mUnit = unit;

        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().SelectUnit(mUnit);
        }
    }

    // ==========================================
    // 2. 移动指令（玩家点击地面）
    // ==========================================
    public class MoveCommand : AbstractCommand
    {
        private readonly GridPos mTarget;
        public MoveCommand(GridPos target) => mTarget = target;

        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().TryMove(mTarget);
        }
    }

    // ==========================================
    // 3. 进入攻击模式（点击面板上的“攻击”按钮）
    // ==========================================
    public class EnterAttackCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().EnterAttackState();
        }
    }

    // ==========================================
    // 4. 执行攻击（玩家点击敌人）
    // ==========================================
    public class AttackCommand : AbstractCommand
    {
        private readonly BattleUnitData mTarget;
        public AttackCommand(BattleUnitData target) => mTarget = target;

        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().ExecuteAttack(mTarget);
        }
    }

    // ==========================================
    // 5. 待机（点击面板上的“待机”按钮）
    // ==========================================
    public class StandbyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().FinishAction();
        }
    }

    // ==========================================
    // 6. 取消当前操作（右键 / 点击空地 / 撤销移动）
    // ==========================================
    public class CancelActionCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IBattleSystem>().CancelAction();
        }
    }

    // ==========================================
    // 7. 生成战斗单位（拖拽入场时）
    // ==========================================
    public class SpawnUnitCommand : AbstractCommand
    {
        private readonly RoleInfo mInfo;
        private readonly GridPos mPos;
        private readonly bool mIsPlayer;

        public SpawnUnitCommand(RoleInfo info, GridPos pos, bool isPlayer)
        {
            mInfo = info;
            mPos = pos;
            mIsPlayer = isPlayer;
        }

        protected override void OnExecute()
        {
            var model = this.GetModel<IBattleModel>();
            var newUnit = new BattleUnitData(model.AllUnits.Count + 1, mInfo, mIsPlayer, mPos);
            model.AddUnit(newUnit);
        }
    }

    // ==========================================
    // 8. 切换回合阶段（供系统内部切换使用）
    // ==========================================
    public class SwitchPhaseCommand : AbstractCommand
    {
        private readonly BattlePhase mTargetPhase;
        public SwitchPhaseCommand(BattlePhase phase) => mTargetPhase = phase;

        protected override void OnExecute()
        {
            var turnSystem = this.GetSystem<IBattleTurnSystem>();
            if (mTargetPhase == BattlePhase.PlayerTurn)
                turnSystem.StartPlayerTurn();
            else if (mTargetPhase == BattlePhase.EnemyTurn)
                turnSystem.CheckTurnEnd(); // 强制触发敌方回合
        }
    }
}
