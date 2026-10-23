using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IPathPreviewSystem : ISystem
    {
        void ShowReachableGrids(BattleUnitData unit);
        void ShowAttackGrids(BattleUnitData unit);
        void ShowPathPreview(GridPos target);
        void ClearPathPreview();
        void ClearAllGrids();
    }

    public class PathPreviewSystem : AbstractSystem, IPathPreviewSystem, ICanSendEvent
    {
        private IBattleModel mModel;
        private IMapModel mMapModel;
        private IPathfindingSystem mPath;

        private readonly List<GridPos> mCurrentBfsRange = new List<GridPos>();
        private readonly List<BlockData> mCurrentPath = new List<BlockData>();

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mMapModel = this.GetModel<IMapModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
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
                mCurrentBfsRange.Add(pos);
                this.SendEvent(new BlockUIStateChangedEvent(b.Row, b.Col, ShowPathType._BFS));
            }
        }

        public void ShowAttackGrids(BattleUnitData unit)
        {
            ClearAllGrids();
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if (UnityEngine.Mathf.Abs(r) + UnityEngine.Mathf.Abs(c) == 1)
                    {
                        int row = unit.Pos.Row + r, col = unit.Pos.Col + c;
                        if (mMapModel.GetBlock(row, col) != null)
                        {
                            this.SendEvent(new BlockUIStateChangedEvent(row, col, ShowPathType.Attack));
                            mCurrentBfsRange.Add(new GridPos(row, col));
                        }
                    }
                }
            }
        }

        public void ShowPathPreview(GridPos target)
        {
            ClearPathPreview();
            var unit = mModel.SelectedUnit.Value;
            if (unit == null || mModel.Action.Value != ActionState.Moving) return;
            if (!mCurrentBfsRange.Exists(p => p.Row == target.Row && p.Col == target.Col)) return;

            var startBlock = mMapModel.GetBlock(unit.Pos.Row, unit.Pos.Col);
            var endBlock = mMapModel.GetBlock(target.Row, target.Col);
            var path = mPath.FindPath(startBlock, endBlock);
            if (path == null || path.Count == 0) return;

            mCurrentPath.AddRange(path);
            foreach (var b in path)
                this.SendEvent(new BlockUIStateChangedEvent(b.Row, b.Col, ShowPathType._A));
        }

        public void ClearPathPreview()
        {
            if (mCurrentPath.Count == 0) return;
            foreach (var b in mCurrentPath)
            {
                var type = mCurrentBfsRange.Exists(p => p.Row == b.Row && p.Col == b.Col)
                    ? ShowPathType._BFS : ShowPathType.None;
                this.SendEvent(new BlockUIStateChangedEvent(b.Row, b.Col, type));
            }
            mCurrentPath.Clear();
        }

        public void ClearAllGrids()
        {
            ClearPathPreview();
            foreach (var pos in mCurrentBfsRange)
                this.SendEvent(new BlockUIStateChangedEvent(pos.Row, pos.Col, ShowPathType.None));
            mCurrentBfsRange.Clear();
        }
    }
}
