
using UnityEngine;

namespace Game
{
    public static class GridMathTool
    {
        public static Vector3 GetCellCenterWorld(IWorldMapModel model, Vector2Int cellPos)
        {
        
            return new Vector3(
                (cellPos.x * model.CellSize.x) + (model.CellSize.x * model.CellAnchorOffset.x),
                (cellPos.y * model.CellSize.y) + (model.CellSize.y * model.CellAnchorOffset.y),
                0
            );
        }
        public static Vector2Int WorldToCell(IWorldMapModel model, Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPos.x / model.CellSize.x),
                Mathf.FloorToInt(worldPos.y / model.CellSize.y)
            );
        }
    }
}
