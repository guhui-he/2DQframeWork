using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public static class TilemapDataExtractor
    {
        /// <summary>
        /// 通用遍历提取器
        /// </summary>
        public static void Extract(
            Tilemap groundMap,
            Tilemap wallMap,
            Action<Vector2Int, Vector3Int, bool> onCellProcessed)
        {
            if (groundMap == null) return;

            BoundsInt bounds = groundMap.cellBounds;
            foreach (var cellPos in bounds.allPositionsWithin)
            {
                if (!groundMap.HasTile(cellPos)) continue;
                Vector2Int logicPos = new Vector2Int(cellPos.x, cellPos.y);
                bool isWalkable = (wallMap == null || !wallMap.HasTile(cellPos));

                onCellProcessed?.Invoke(logicPos, cellPos, isWalkable);
            }
        }
    }
}
