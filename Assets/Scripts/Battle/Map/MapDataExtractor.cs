using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public static class MapDataExtractor
    {
        public static (int rowCount, int colCount, BlockRawData[] data) Extract(Tilemap ground, Tilemap cost)
        {
            var rawDataList = new List<BlockRawData>();

            if (ground == null)
            {
                Debug.LogError("[MapDataExtractor] ground Tilemap 为空");
                return (0, 0, rawDataList.ToArray());
            }

            int yMin = ground.cellBounds.yMin;
            int xMin = ground.cellBounds.xMin;
            int rowCount = ground.cellBounds.size.y;
            int colCount = ground.cellBounds.size.x;

            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (!ground.HasTile(pos)) continue;

                int row = pos.y - yMin;
                int col = pos.x - xMin;

                if (row < 0 || row >= rowCount || col < 0 || col >= colCount) continue;

                int costVal = 1;
                if (cost != null)
                {
                    Vector3 worldPos = ground.CellToWorld(pos);
                    Vector3Int costPos = cost.WorldToCell(worldPos);
                    if (cost.HasTile(costPos)) costVal = 2;
                }

                rawDataList.Add(new BlockRawData
                {
                    Row = row,
                    Col = col,
                    Cost = costVal
                });
            }

            return (rowCount, colCount, rawDataList.ToArray());
        }
    }
}
