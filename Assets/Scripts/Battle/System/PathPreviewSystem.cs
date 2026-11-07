using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IPathPreviewSystem : ISystem
    {
        void ShowReachableGrids(BattleUnitData unit);
        void ShowAttackGrids(BattleUnitData unit);
        void ShowAoePreviewGrids(GridPos centerPos, int aoeRange);
        void ShowPathPreview(GridPos target);
        void ClearPathPreview();
        void ClearAllGrids();
        bool IsInAttackRange(GridPos pos);
    }

    public class PathPreviewSystem : AbstractSystem, IPathPreviewSystem, ICanSendEvent
    {
        private IBattleModel mModel;
        private IMapModel mMapModel;
        private ISkillModel mSkillModel;
        private IPathfindingSystem mPath;
        private IBattleRangeSystem mBattleRange; // ★ 注入范围系统

        private readonly List<GridPos> mCurrentBfsRange = new List<GridPos>();
        private readonly List<GridPos> mCurrentPath = new List<GridPos>();

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mMapModel = this.GetModel<IMapModel>();
            mSkillModel = this.GetModel<ISkillModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
            mBattleRange = this.GetSystem<IBattleRangeSystem>();
        }

        public void ShowReachableGrids(BattleUnitData unit)
        {
            ClearAllGrids();
            var startBlock = mMapModel.GetBlock(unit.Pos.Row, unit.Pos.Col);
            var blocks = mPath.GetReachableBlocks(startBlock, unit.CurrentStep);

            foreach (var b in blocks)
            {
                var pos = new GridPos(b.Row, b.Col);
                if (mModel.GetUnitAt(pos) != null) continue;
                HighlightGrid(pos, ShowPathType._BFS);
            }
        }

        public void ShowAttackGrids(BattleUnitData unit)
        {
            ClearAllGrids();
            var skill = mSkillModel.GetSkill(mModel.SelectedSkillId.Value);
            if (skill == null) return;

            int maxRange = skill.Config.atkRange + unit.SourceInfo.GetFinalStat().AtkRange;
            var skillRange = mBattleRange.GetSkillRange(unit, skill, maxRange);
            foreach (var pos in skillRange)
            {
                HighlightGrid(pos, ShowPathType.Attack);
            }
        }

        public void ShowAoePreviewGrids(GridPos centerPos, int aoeRange)
        {
            ClearAllGrids();
            var centerBlock = mMapModel.GetBlock(centerPos.Row, centerPos.Col);
            if (centerBlock == null) return;

            var aoeBlocks = mPath.GetAttackRangeBlocks(centerBlock, aoeRange);
            foreach (var b in aoeBlocks)
            {
                HighlightGrid(new GridPos(b.Row, b.Col), ShowPathType.Attack);
            }
        }

        public void ShowPathPreview(GridPos target)
        {
            ClearPathPreview();
            var unit = mModel.SelectedUnit.Value;
            if (unit == null || mModel.Action.Value != ActionState.Moving) return;
            if (!mCurrentBfsRange.Contains(target)) return;

            var startBlock = mMapModel.GetBlock(unit.Pos.Row, unit.Pos.Col);
            var endBlock = mMapModel.GetBlock(target.Row, target.Col);
            var path = mPath.FindPath(startBlock, endBlock);
            if (path == null || path.Count == 0) return;

            foreach (var b in path)
            {
                var pos = new GridPos(b.Row, b.Col);
                mCurrentPath.Add(pos);
                this.SendEvent(new BlockUIStateChangedEvent(b.Row, b.Col, ShowPathType._A));
            }
        }

        public void ClearPathPreview()
        {
            if (mCurrentPath.Count == 0) return;
            foreach (var pos in mCurrentPath)
            {
                var type = mCurrentBfsRange.Contains(pos) ? ShowPathType._BFS : ShowPathType.None;
                this.SendEvent(new BlockUIStateChangedEvent(pos.Row, pos.Col, type));
            }
            mCurrentPath.Clear();
        }

        public void ClearAllGrids()
        {
            ClearPathPreview();
            foreach (var pos in mCurrentBfsRange)
            {
                this.SendEvent(new BlockUIStateChangedEvent(pos.Row, pos.Col, ShowPathType.None));
            }
            mCurrentBfsRange.Clear();
        }

        public bool IsInAttackRange(GridPos pos)
        {
            return mCurrentBfsRange.Contains(pos);
        }

        private void HighlightGrid(GridPos pos, ShowPathType type)
        {
            if (!mCurrentBfsRange.Contains(pos))
                mCurrentBfsRange.Add(pos);
            this.SendEvent(new BlockUIStateChangedEvent(pos.Row, pos.Col, type));
        }
    }
}
