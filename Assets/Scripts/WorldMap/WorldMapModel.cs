using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IWorldMapModel : IModel
    {
        HashSet<Vector2Int> WalkableCells { get; }
        Dictionary<Vector2Int, int> RegionMap { get; }
        Dictionary<int, List<Vector2Int>> SpawnableCellsByRegion { get; }
        HashSet<Vector2Int> RoadCells { get; }

        // ★ 新增：记录格子的尺寸和原点偏移（纯数据，不依赖任何组件）
        Vector3 CellSize { get; set; }
        Vector3 CellAnchorOffset { get; set; }

        void AddRoadCell(Vector2Int pos);
        void AddWalkableCell(Vector2Int pos);
        void AddRegionMap(Vector2Int pos, int id);
        void AddSpawnableCell(int regionId, Vector2Int pos);
        List<Vector2Int> GetSpawnableCells(int regionId);
    }

    public class WorldMapModel : AbstractModel, IWorldMapModel
    {
        public HashSet<Vector2Int> WalkableCells { get; } = new();
        public Dictionary<Vector2Int, int> RegionMap { get; } = new();
        public Dictionary<int, List<Vector2Int>> SpawnableCellsByRegion { get; } = new();
        public HashSet<Vector2Int> RoadCells { get; } = new();

        // ★ 纯数据：格子大小与偏移
        public Vector3 CellSize { get; set; } = Vector3.one;
        public Vector3 CellAnchorOffset { get; set; } = new Vector3(0.5f, 0.5f, 0);

        protected override void OnInit() { }

        public void AddRoadCell(Vector2Int pos) => RoadCells.Add(pos);
        public void AddWalkableCell(Vector2Int pos) => WalkableCells.Add(pos);
        public void AddRegionMap(Vector2Int pos, int id) => RegionMap[pos] = id;

        public void AddSpawnableCell(int regionId, Vector2Int pos)
        {
            if (!SpawnableCellsByRegion.ContainsKey(regionId)) SpawnableCellsByRegion[regionId] = new List<Vector2Int>();
            SpawnableCellsByRegion[regionId].Add(pos);
        }

        public List<Vector2Int> GetSpawnableCells(int regionId) => SpawnableCellsByRegion.TryGetValue(regionId, out var list) ? list : null;
    }
}
