using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IPathfindingSystem : ISystem
    {
        List<BlockData> GetReachableBlocks(BlockData start, int step);
        List<BlockData> FindPath(BlockData start, BlockData end);
        bool HasPath(BlockData start, BlockData end);
        int GetMoveCost(BlockData start, BlockData end);
    }

    public class PathfindingSystem : AbstractSystem, IPathfindingSystem
    {
        private IMapModel mMapModel;
        private IBattleModel mBattleModel; // ★ 新增：用于检查动态单位占位

        protected override void OnInit()
        {
            mMapModel = this.GetModel<IMapModel>();
            mBattleModel = this.GetModel<IBattleModel>();
        }

        // ================== 加权 BFS：可达范围 ==================
        public List<BlockData> GetReachableBlocks(BlockData start, int step)
        {
            var result = new List<BlockData>();
            if (start == null || step < 0) return result;

            var maxRemaining = new Dictionary<BlockData, int>();
            var queue = new Queue<(BlockData block, int remaining)>();
            maxRemaining[start] = step;
            queue.Enqueue((start, step));

            while (queue.Count > 0)
            {
                var (current, remaining) = queue.Dequeue();
                if (!result.Contains(current)) result.Add(current);

                foreach (var neighbor in GetNeighbors(current, start))
                {
                    if (neighbor == null || neighbor.Type == BlockType.Obstacle) continue;

                    int next = remaining - neighbor.Cost;
                    if (next < 0) continue;
                    if (maxRemaining.TryGetValue(neighbor, out var rec) && rec >= next) continue;

                    maxRemaining[neighbor] = next;
                    queue.Enqueue((neighbor, next));
                }
            }
            return result;
        }

        // ================== A*：具体路径 ==================
        public List<BlockData> FindPath(BlockData start, BlockData end)
        {
            if (start == null || end == null) return null;
            if (end.Type == BlockType.Obstacle) return null;
            if (start == end) return new List<BlockData> { start };

            var open = new MinHeap<PathNode>((a, b) => a.F.CompareTo(b.F));
            var gScore = new Dictionary<BlockData, int>();
            var cameFrom = new Dictionary<BlockData, BlockData>();
            var closed = new HashSet<BlockData>();

            gScore[start] = 0;
            open.Push(new PathNode(start, Heuristic(start, end)));

            while (open.Count > 0)
            {
                var current = open.Pop().Block;
                if (closed.Contains(current)) continue;
                if (current == end) return ReconstructPath(cameFrom, current);

                closed.Add(current);
                int curG = gScore[current];

                foreach (var neighbor in GetNeighbors(current, start))
                {
                    if (neighbor == null || neighbor.Type == BlockType.Obstacle) continue;
                    if (closed.Contains(neighbor)) continue;

                    // ★ A* 寻路时，如果邻居是终点（代表玩家），可以放行，让寻路寻到玩家面前；
                    // 如果邻居是途中的其他单位，则不通行。
                    if (neighbor != end && IsOccupied(neighbor)) continue;

                    int tentativeG = curG + neighbor.Cost;
                    if (gScore.TryGetValue(neighbor, out var oldG) && tentativeG >= oldG) continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    open.Push(new PathNode(neighbor, tentativeG + Heuristic(neighbor, end)));
                }
            }
            return null;
        }

        public bool HasPath(BlockData start, BlockData end) => FindPath(start, end) != null;

        public int GetMoveCost(BlockData start, BlockData end)
        {
            var path = FindPath(start, end);
            if (path == null) return -1;
            int total = 0;
            for (int i = 1; i < path.Count; i++) total += path[i].Cost;
            return total;
        }

        // ================== 内部工具 ==================
        private static int Heuristic(BlockData a, BlockData b)
            => System.Math.Abs(a.Row - b.Row) + System.Math.Abs(a.Col - b.Col);

        private static List<BlockData> ReconstructPath(Dictionary<BlockData, BlockData> cameFrom, BlockData current)
        {
            var path = new List<BlockData> { current };
            while (cameFrom.TryGetValue(current, out var prev))
            {
                path.Add(prev);
                current = prev;
            }
            path.Reverse();
            return path;
        }

        // ★ 新增：检查格子是否被其他单位占用
        private bool IsOccupied(BlockData block)
        {
            var pos = new GridPos(block.Row, block.Col);
            var unit = mBattleModel.GetUnitAt(pos);
            return unit != null && unit.Hp > 0;
        }

        // ★ 修改：传入起点，避免把起点自己当成障碍物
        private List<BlockData> GetNeighbors(BlockData c, BlockData start)
        {
            var list = new List<BlockData>(4);

            void TryAdd(int row, int col)
            {
                var b = mMapModel.GetBlock(row, col);
                if (b == null) return;

                // 如果是起点自己，直接加入
                if (b == start) { list.Add(b); return; }

                // 如果是被占用的格子，不加入邻居（寻路会绕开）
                if (IsOccupied(b)) return;

                list.Add(b);
            }

            TryAdd(c.Row - 1, c.Col);
            TryAdd(c.Row + 1, c.Col);
            TryAdd(c.Row, c.Col - 1);
            TryAdd(c.Row, c.Col + 1);

            return list;
        }
    }
}
