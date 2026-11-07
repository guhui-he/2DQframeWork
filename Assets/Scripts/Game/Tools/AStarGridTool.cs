using System;
using System.Collections.Generic;
using UnityEngine; 

namespace Game
{
    public static class AStarGridTool
    {
        // 四方向移动常量
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        /// <summary>
        /// A* 寻路
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="isWalkable">判断该坐标是否可走的委托</param>
        /// <returns>路径列表（包含终点，不含起点），无路可走返回 null</returns>
        public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Func<Vector2Int, bool> isWalkable)
        {
            if (start == end) return new List<Vector2Int>();
            if (!isWalkable(end)) return null;

            var openSet = new List<Vector2Int> { start };
            var closedSet = new HashSet<Vector2Int>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

            var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
            var fScore = new Dictionary<Vector2Int, int> { [start] = Heuristic(start, end) };

            while (openSet.Count > 0)
            {
                // 找 F 值最小的节点
                Vector2Int current = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (fScore.TryGetValue(openSet[i], out int f) && fScore.TryGetValue(current, out int currF) && f < currF)
                        current = openSet[i];
                }

                if (current == end)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var dir in Directions)
                {
                    Vector2Int neighbor = current + dir;

                    if (!isWalkable(neighbor) || closedSet.Contains(neighbor))
                        continue;

                    int tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, int.MaxValue))
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, end);
                }
            }
            return null; // 没有可行路径
        }

        private static int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // 曼哈顿距离
        }

        private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
            return path;
        }
    }
}
