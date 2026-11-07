using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public interface IWorldInitSystem : ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groundMap">基地格子</param>
        /// <param name="wallMap">不可行走区域</param>
        /// <param name="roadMap">道路区域</param>
        /// <param name="regionMap">划分整个map区域</param>
        void InitMapData(Tilemap groundMap, Tilemap wallMap, Tilemap roadMap, Tilemap regionMap );
    }

    public class WorldInitSystem : AbstractSystem, IWorldInitSystem
    {
        protected override void OnInit() { }

        public void InitMapData(Tilemap ground, Tilemap wall, Tilemap road, Tilemap region)
        {
            var model = this.GetModel<IWorldMapModel>();
            TilemapDataExtractor.Extract(ground, wall, (logicPos, cellPos, isWalkable) =>
            {
                // 1. 提取可行走
                if (isWalkable)
                {
                    model.AddWalkableCell(logicPos);
                }

                // 2. 提取道路
                if (road != null && road.HasTile(cellPos))
                {
                    model.AddRoadCell(logicPos);
                }

                // 3. 提取区域
                if (region != null && region.GetTile(cellPos) is RegionTile regionTile)
                {
                    model.AddRegionMap(logicPos, regionTile.RegionId);
                    if (isWalkable)
                    {
                        model.AddSpawnableCell(regionTile.RegionId, logicPos);
                    }
                }
            });

            Debug.Log($"[WorldInit] 可走点:{model.WalkableCells.Count}, 道路:{model.RoadCells.Count}");
        }
    }
}
