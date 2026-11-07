using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IPathfindingSystem : ISystem
    {
        List<BlockData> GetAttackRangeBlocks(BlockData start, int range);
        List<BlockData> GetReachableBlocks(BlockData start, int step);
        List<BlockData> FindPath(BlockData start, BlockData end);
        List<BlockData> GetLinePath(BlockData start, int dr, int dc, int maxRange, bool stopAtUnit = true);
        bool HasPath(BlockData start, BlockData end);
        int GetMoveCost(BlockData start, BlockData end);
    }

    public class PathfindingSystem : AbstractSystem, IPathfindingSystem
    {
        private IMapModel mMapModel;
        private IBattleModel mBattleModel;

        // 静态方向常量，避免每次循环 new 数组
        private static readonly int[] Dr = { 0, 1, 0, -1 };
        private static readonly int[] Dc = { 1, 0, -1, 0 };

        protected override void OnInit()
        {
            mMapModel = this.GetModel<IMapModel>();
            mBattleModel = this.GetModel<IBattleModel>();
        }
        public List<BlockData> GetLinePath(BlockData start, int dr, int dc, int maxRange, bool stopAtUnit = true)
        {
            var path = new List<BlockData>() { start };
            if(start==null||maxRange<=0) return path;
            dr = dr > 0 ? 1 : (dr < 0 ? -1 : 0);
            dc = dc > 0 ? 1:(dc < 0 ? -1 : 0);
            BlockData current=start;
            for (int i = 0; i < maxRange; i++)
            {
                var next = mMapModel.GetBlock(current.Row + dr, current.Col + dc);
                if (next == null || next.Type == BlockType.Obstacle) break; // 撞墙截断

                path.Add(next);
                if (stopAtUnit && IsOccupied(next)) break; // 遇人截断

                current = next;
            }
            return path;
        }

        // ================== 攻击范围（无视单位阻挡，只看墙） ==================
        public List<BlockData> GetAttackRangeBlocks(BlockData start, int range)
        {
            var result = new List<BlockData>();
            if (start == null || range < 0) return result;

            var visited = new HashSet<BlockData>();
            var queue = new Queue<(BlockData block, int remaining)>();

            visited.Add(start);
            queue.Enqueue((start, range));
            result.Add(start);

            while (queue.Count > 0)
            {
                var (current, remaining) = queue.Dequeue();
                if (remaining == 0) continue;

                for (int i = 0; i < 4; i++)
                {
                    var neighbor = mMapModel.GetBlock(current.Row + Dr[i], current.Col + Dc[i]);
                    if (neighbor == null || neighbor.Type == BlockType.Obstacle) continue;

                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        result.Add(neighbor);
                        queue.Enqueue((neighbor, remaining - 1));
                    }
                }
            }
            return result;
        }

        // ================== 直线冲锋计算 ==================
        public List<GridPos> FindLinePath(BattleUnitData caster, GridPos end, int maxRange, out BattleUnitData hitTarget)
        {
            hitTarget = null;
            var path = new List<GridPos> { caster.Pos };

            int dr = end.Row - caster.Pos.Row;
            int dc = end.Col - caster.Pos.Col;
            dr = dr > 0 ? 1 : (dr < 0 ? -1 : 0);
            dc = dc > 0 ? 1 : (dc < 0 ? -1 : 0);

            GridPos current = caster.Pos;
            for (int i = 0; i < maxRange; i++)
            {
                GridPos next = new GridPos(current.Row + dr, current.Col + dc);
                var block = mMapModel.GetBlock(next.Row, next.Col);

                if (block == null || block.Type == BlockType.Obstacle) break;

                path.Add(next);
                var unit = mBattleModel.GetUnitAt(next);

                if (unit != null)
                {
                    if (unit.UnitId != caster.UnitId)
                    {
                        hitTarget = unit;
                    }
                    break;
                }
                current = next;
                if (current.Equals(end)) break;
            }
            return path;
        }

        // ================== 加权 BFS：移动可达范围（避开单位） ==================
        public List<BlockData> GetReachableBlocks(BlockData start, int step)
        {
            var result = new List<BlockData>();
            var maxRemaining = new Dictionary<BlockData, int>();
            var queue = new Queue<(BlockData block, int remaining)>();

            maxRemaining[start] = step;
            queue.Enqueue((start, step));

            while (queue.Count > 0)
            {
                var (current, remaining) = queue.Dequeue();
                result.Add(current);

                foreach (var neighbor in GetNeighbors(current, start))
                {
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
                    if (closed.Contains(neighbor)) continue;
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

        private bool IsOccupied(BlockData block)
        {
            var unit = mBattleModel.GetUnitAt(new GridPos(block.Row, block.Col));
            return unit != null && unit.Hp > 0;
        }
        private IEnumerable<BlockData> GetNeighbors(BlockData c, BlockData start)
        {
            for (int i = 0; i < 4; i++)
            {
                var b = mMapModel.GetBlock(c.Row + Dr[i], c.Col + Dc[i]);
                if (b == null) continue;
                if (b == start || !IsOccupied(b))
                {
                    yield return b;
                }
            }
        }
    }
}
