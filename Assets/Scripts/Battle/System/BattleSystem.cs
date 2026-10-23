
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBattleSystem : ISystem
    {
        void SelectUnit(BattleUnitData unit);
        void EnterAttackState();
        void Deselect();
        void TryMove(GridPos target);
        void ExecuteAttack(BattleUnitData target);
        void FinishAction();
        void CancelAction();

        // 供 Action 内部调用的纯逻辑方法
        bool CalculateAndApplyMove(BattleUnitData unit, GridPos target, out bool needWaitAnimation);
    }

    public class BattleSystem : AbstractSystem, IBattleSystem
    {
        private IBattleModel mModel;
        private IMapModel mMapModel;
        private IPathfindingSystem mPath;
        private IPathPreviewSystem mPreview;
        private ICombatSystem mCombat;

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mMapModel = this.GetModel<IMapModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
            mPreview = this.GetSystem<IPathPreviewSystem>();
            mCombat = this.GetSystem<ICombatSystem>();

            // 移动完成事件，仅用于玩家移动后弹菜单
            //this.RegisterEvent<UnitMoveCompletedEvent>(e =>
            //{
            //    var selected = mModel.SelectedUnit.Value;
            //    if (selected != null && selected.UnitId == e.UnitId && selected.IsPlayer)
            //    {
            //        mModel.Action.Value = ActionState.ActionMenu;
            //        this.SendEvent(new ShowActionMenuEvent { Unit = selected });
            //    }
            //});
        }

        public void SelectUnit(BattleUnitData unit)
        {
            if (unit == null || unit.HasActed || mModel.Phase.Value != BattlePhase.PlayerTurn) return;

            mModel.SelectedUnit.Value = unit;
            if (!unit.HasMoved)
            {
                mModel.Action.Value = ActionState.Moving;
                mPreview.ShowReachableGrids(unit);
            }
            else
            {
                mModel.Action.Value = ActionState.ActionMenu;
                this.SendEvent(new ShowActionMenuEvent { Unit = unit });
            }
        }

        public void TryMove(GridPos target)
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;

            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();

            var queue = this.GetSystem<IActionQueueSystem>();

            // 1. 入队移动动作（如果原地不动，MoveAction 会瞬间完成）
            queue.Enqueue(new MoveAction(unit, target));


            if (unit.IsPlayer)
            {
                queue.Enqueue(new ActionWrapper(() => {
                    if (unit.IsAlive && unit.HasMoved && !unit.HasActed)
                    {
                        mModel.Action.Value = ActionState.ActionMenu;
                        this.SendEvent(new ShowActionMenuEvent { Unit = unit });
                    }
                }));
            }
        }

        public void EnterAttackState()
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;
            mModel.Action.Value = ActionState.Attacking;
            this.SendEvent(new HideActionMenuEvent());
            mPreview.ShowAttackGrids(unit);
        }

        public void ExecuteAttack(BattleUnitData target)
        {
            var attacker = mModel.SelectedUnit.Value;
            if (attacker == null) return;

            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();

            var queue = this.GetSystem<IActionQueueSystem>();
            queue.Enqueue(new AttackAction(attacker, target));
            queue.Enqueue(new ActionWrapper(() => {
                Deselect();
                this.GetSystem<IBattleTurnSystem>().CheckTurnEnd();
            }));
        }

        public void FinishAction()
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;

            unit.HasActed = true;
            unit.HasMoved = true;
            Deselect();
            this.GetSystem<IBattleTurnSystem>().CheckTurnEnd();
        }

        public void CancelAction()
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;

            if (mModel.Action.Value == ActionState.Attacking)
            {
                mModel.Action.Value = ActionState.ActionMenu;
                mPreview.ClearAllGrids();
                this.SendEvent(new ShowActionMenuEvent { Unit = unit });
            }
            else if (mModel.Action.Value == ActionState.ActionMenu && unit.HasMoved)
            {
                unit.Pos = unit.StartPos;
                unit.CurrentStep = unit.SourceInfo.Step;
                unit.HasMoved = false;
                this.SendEvent(new ForceTeleportEvent { Unit = unit });
                SelectUnit(unit);
            }
            else
            {
                Deselect();
            }
        }

        public void Deselect()
        {
            mModel.SelectedUnit.Value = null;
            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();
            this.SendEvent(new HideActionMenuEvent());
        }

        public bool CalculateAndApplyMove(BattleUnitData unit, GridPos target, out bool needWaitAnimation)
        {
            needWaitAnimation = false;
            if (unit == null || unit.HasMoved || !unit.IsAlive) return false;

            if (target.Equals(unit.Pos))
            {
                unit.StartPos = unit.Pos;
                unit.HasMoved = true;
                return true;
            }

            if (mModel.GetUnitAt(target) != null) return false;

            var path = mPath.FindPath(
                mMapModel.GetBlock(unit.Pos.Row, unit.Pos.Col),
                mMapModel.GetBlock(target.Row, target.Col));

            if (path == null || path.Count <= 1) return false;

            int cost = 0;
            for (int i = 1; i < path.Count; i++) cost += path[i].Cost;
            if (cost > unit.CurrentStep) return false;

            unit.StartPos = unit.Pos;
            unit.CurrentStep -= cost;
            unit.HasMoved = true;
            unit.Pos = target;

            var gridPath = path.ConvertAll(b => new GridPos(b.Row, b.Col));
            mModel.MovePath.Value = new UnitMoveStartEvent { UnitId = unit.UnitId, Path = gridPath };
            needWaitAnimation = true;
            return true;
        }
    }
}
