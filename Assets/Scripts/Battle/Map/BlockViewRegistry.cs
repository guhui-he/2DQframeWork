using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>View 层：GridPos -> Transform 的查询表</summary>
    public static class BlockViewRegistry
    {
        private static readonly Dictionary<GridPos, Transform> sMap = new();

        public static void Register(int row, int col, Transform t)
            => sMap[new GridPos(row, col)] = t;

        public static bool TryGetWorldPosition(GridPos pos, out Vector3 worldPos)
        {
            if (sMap.TryGetValue(pos, out var t))
            {
                worldPos = t.position;
                return true;
            }
            worldPos = default;
            return false;
        }

        public static void Clear() => sMap.Clear();
    }
}
