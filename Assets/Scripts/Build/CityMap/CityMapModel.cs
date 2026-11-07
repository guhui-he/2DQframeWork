using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public interface ICityMapModel : IModel
    {
        HashSet<Vector2Int> WalkableCells { get; }
        BoundsInt MapBounds { get; }

        void InitMap(Tilemap ground, Tilemap wall);
    }

    public class CityMapModel : AbstractModel, ICityMapModel
    {
        public HashSet<Vector2Int> WalkableCells { get; } = new();
        public BoundsInt MapBounds { get; private set; }

        protected override void OnInit() { }

        public void InitMap(Tilemap ground, Tilemap wall)
        {
            MapBounds = ground.cellBounds;
            WalkableCells.Clear();
        }
    }
}
