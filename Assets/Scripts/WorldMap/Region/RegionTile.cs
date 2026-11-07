using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    [CreateAssetMenu(fileName = "NewRegionTile", menuName = "Tilemap/Region Tile")]
    public class RegionTile : Tile
    {
        public int RegionId; 
    }
}
