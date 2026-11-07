using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBattleSystem : ISystem
    {
        void SelectUnit(BattleUnitData unit);
        void EnterAttackState(int skillId);
        void Deselect();
        void TryMove(GridPos target);
        void ExecuteAttack(BattleUnitData primaryTarget, GridPos targetPos);
        void FinishAction();
        void CancelAction();
        bool CalculateAndApplyMove(BattleUnitData unit, GridPos target, MoveStyle style, bool isSkillMove, out bool needWaitAnimation);
    }

    public class BattleSystem : AbstractSystem, IBattleSystem
    {
        private IBattleModel mModel;
        private IMapModel mMapModel;
        private IPathfindingSystem mPath;
        private IPathPreviewSystem mPreview;

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mMapModel = this.GetModel<IMapModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
            mPreview = this.GetSystem<IPathPreviewSystem>();
        }

        public void SelectUnit(BattleUnitData unit)
        {
            if (unit == null || unit.HasActed || mModel.Phase.Value != BattlePhase.PlayerTurn) return;
            mModel.SelectedUnit.Value = unit;
            if (unit.IsPlayer)
            {
                if (!unit.HasMoved)
                {
                    mModel.Action.Value = ActionState.Moving;
                    mPreview.ShowReachableGrids(unit);
                }
                else
                {
                    EnterAttackState(unit.skills[0].Config.id);
                }
            }
            else
            {
                mModel.Action.Value = ActionState.Inspecting;
                mPreview.ShowReachableGrids(unit);
            }
        }

        public void TryMove(GridPos target)
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null || unit.HasMoved) return;

            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();

            this.GetSystem<IActionQueueSystem>().Enqueue(new MoveAction(unit, target));
        }

        public void EnterAttackState(int skillId)
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;
            mModel.SelectedSkillId.Value = skillId;
            mModel.Action.Value = ActionState.Attacking;
            mPreview.ShowAttackGrids(unit);
        }

        public void ExecuteAttack(BattleUnitData primaryTarget, GridPos targetPos)
        {
            var attacker = mModel.SelectedUnit.Value;
            if (attacker == null) return;

            var skill = attacker.skills.Find(s => s.Config.id == mModel.SelectedSkillId.Value);
            if (skill == null) return;

            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();  
            var targets = BattleTargetUtility.CollectTargets(attacker, skill, primaryTarget, targetPos, mModel, mMapModel, mPath);
            bool isTargetRequired = skill.Config.targetCategory == SkillTargetCategory.Enemy ||
                            skill.Config.targetCategory == SkillTargetCategory.Ally;

            if (targets.Count == 0 && isTargetRequired)
            {
                CancelAction();
                return;
            }
            var queue = this.GetSystem<IActionQueueSystem>();
            ISkillCastStrategy strategy = SkillCastStrategyFactory.GetStrategy(skill);
            strategy.Cast(attacker, skill, targets, targetPos, queue);

            // 2. ★ 核心：攻击动作播完后，该单位的回合就结束了。直接入队回合结束动作
            queue.Enqueue(new UnitTurnEndAction(attacker));

            // 3. 收尾：取消选中，检查是否全队行动完毕（是否切敌方回合）
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
            var queue = this.GetSystem<IActionQueueSystem>();
            queue.Enqueue(new UnitTurnEndAction(unit));
            queue.Enqueue(new ActionWrapper(() => this.GetSystem<IBattleTurnSystem>().CheckTurnEnd()));
        }

        public void CancelAction()
        {
            var unit = mModel.SelectedUnit.Value;
            if (unit == null) return;

            if (mModel.Action.Value == ActionState.Attacking || mModel.Action.Value == ActionState.TargetConfirm)
            {
                mModel.Action.Value = ActionState.WaitInput;
                mPreview.ClearAllGrids();
            }
            else if (mModel.Action.Value == ActionState.WaitInput && unit.HasMoved)
            {
                unit.Pos = unit.StartPos;
                unit.CurrentStep = unit.SourceInfo.GetFinalStat().Step;
                unit.HasMoved = false;
                this.SendEvent(new ForceTeleportEvent { Unit = unit });
                SelectUnit(unit);
            }
            else Deselect();
        }

        public void Deselect()
        {
            mModel.SelectedUnit.Value = null;
            mModel.Action.Value = ActionState.WaitInput;
            mPreview.ClearAllGrids();
        }

        public bool CalculateAndApplyMove(BattleUnitData unit, GridPos target, MoveStyle style, bool isSkillMove, out bool needWaitAnimation)
        {
            needWaitAnimation = false;
            if (unit == null || !unit.IsAlive) return false;

            // 如果是普通移动且已经动过了，拦截
            if (!isSkillMove && unit.HasMoved) return false;

            if (target.Equals(unit.Pos))
            {
                if (!isSkillMove) unit.HasMoved = true;
                return true;
            }

            if (mModel.GetUnitAt(target) != null) return false;

            var path = mPath.FindPath(mMapModel.GetBlock(unit.Pos.Row, unit.Pos.Col), mMapModel.GetBlock(target.Row, target.Col));
            if (path == null || path.Count <= 1) return false;

            GridPos startPos = unit.Pos;

            // ★ 核心：瞬间修改真实逻辑坐标！
            unit.StartPos = startPos;
            unit.Pos = target;
            if (!isSkillMove) unit.HasMoved = true;

            var gridPath = path.ConvertAll(b => new GridPos(b.Row, b.Col));

            // 通知 View 播放动画
            mModel.MovePath.Value = new UnitMoveStartEvent
            {
                UnitId = unit.UnitId,
                Path = gridPath,
                From = startPos,
                To = target,
                Style = style
            };

            needWaitAnimation = true;
            return true;
        }
    }
}

