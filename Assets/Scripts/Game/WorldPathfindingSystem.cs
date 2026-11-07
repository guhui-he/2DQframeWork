using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IWorldPathfindingSystem : ISystem
    {
        List<Vector2Int> FindPath(Vector2Int start, Vector2Int end);
    }

    public class WorldPathfindingSystem : AbstractSystem, IWorldPathfindingSystem
    {
        private IWorldMapModel mModel;

        protected override void OnInit()
        {
            mModel = this.GetModel<IWorldMapModel>();
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
        {
            return AStarGridTool.FindPath(start, end, pos => mModel.WalkableCells.Contains(pos));
        }
    }
}
