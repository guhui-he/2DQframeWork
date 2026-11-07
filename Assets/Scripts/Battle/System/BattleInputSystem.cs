using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBattleInputSystem : ISystem
    {
        void HandleClick(GridPos pos, int targetUnitId);
    }

    public class BattleInputSystem : AbstractSystem, IBattleInputSystem
    {
        private IBattleModel mModel;
        private ISkillModel mSkillModel;
        private IPathPreviewSystem mPreview;

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mSkillModel = this.GetModel<ISkillModel>();
            mPreview = this.GetSystem<IPathPreviewSystem>();
        }

        public void HandleClick(GridPos pos, int targetUnitId)
        {
            var skill = mSkillModel.GetSkill(mModel.SelectedSkillId.Value);

            switch (mModel.Action.Value)
            {
                case ActionState.WaitInput:
                case ActionState.Inspecting:
                    if (targetUnitId != -1)
                    {
                        var unit = mModel.AllUnits.FirstOrDefault(u => u.UnitId == targetUnitId);
                        if (unit != null) this.GetSystem<IBattleSystem>().SelectUnit(unit);
                    }
                    else if (mModel.Action.Value == ActionState.Inspecting)
                    {
                        this.GetSystem<IBattleSystem>().Deselect();
                    }
                    break;

                case ActionState.Moving:
                    var selected = mModel.SelectedUnit.Value;
                    if (selected == null || !selected.IsPlayer) return;
                    if (targetUnitId == selected.UnitId) this.GetSystem<IBattleSystem>().TryMove(selected.Pos);
                    else this.GetSystem<IBattleSystem>().TryMove(pos);
                    break;

                case ActionState.Attacking:
                    if (skill == null || !mPreview.IsInAttackRange(pos)) return;
                    ProcessAttackClick(skill, pos, targetUnitId);
                    break;

                case ActionState.TargetConfirm:
                    if (skill == null) return;
                    if (pos.Equals(mModel.PendingTargetPos.Value))
                    {
                        var target = targetUnitId != -1 ? mModel.AllUnits.FirstOrDefault(u => u.UnitId == targetUnitId) : null;
                        this.GetSystem<IBattleSystem>().ExecuteAttack(target, pos);
                    }
                    else
                    {
                        mModel.Action.Value = ActionState.Attacking;
                        mPreview.ShowAttackGrids(mModel.SelectedUnit.Value);
                    }
                    break;
            }
        }

        private void ProcessAttackClick(SkillInfo skill, GridPos pos, int targetUnitId)
        {
            var selected = mModel.SelectedUnit.Value;
            // ★ 传入 skill，由工厂根据 CastMode 分发
            var strategy = SkillCastStrategyFactory.GetStrategy(skill);

            if (!strategy.CanCast(selected, skill, selected.Pos, pos)) return;

            if (skill.Config.targetType == SkillTargetType.Area)
            {
                mModel.PendingTargetPos.Value = pos;
                mModel.Action.Value = ActionState.TargetConfirm;
                mPreview.ShowAoePreviewGrids(pos, skill.Config.atkRange);
                return;
            }

            var targetUnit = targetUnitId != -1 ? mModel.AllUnits.FirstOrDefault(u => u.UnitId == targetUnitId) : null;

            // ★ 纯数据驱动判断：如果是冲锋，且目标位置没人，直接拦截
            if (skill.Config.CastMode == SkillCastMode.Charge && targetUnit == null)
            {
                this.GetSystem<IBattleSystem>().CancelAction();
                return;
            }

            if (targetUnit == null && skill.Config.targetCategory != SkillTargetCategory.Ground && skill.Config.targetCategory != SkillTargetCategory.All)
            {
                this.GetSystem<IBattleSystem>().CancelAction(); // 敌军/友军单体不能空放
                return;
            }

            this.GetSystem<IBattleSystem>().ExecuteAttack(targetUnit, pos);
        }
    }
}
