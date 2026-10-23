namespace Game
{
    public enum BlockType
    {
        Normal,
        Obstacle,
    }

    /// <summary>从 Unity 层提取出来的格子数据summary>
    /// 方便传递数据,值类型零CG
    public struct BlockRawData
    {
        public int Row;
        public int Col;
        public int Cost;
    }

    /// <summary>存于 Model 中的纯逻辑格子数据</summary>
    /// 数据需要进行引用,方便进行数据更改
    public class BlockData
    {
        public int Row;
        public int Col;
        public BlockType Type;
        public int Cost;

        public BlockData(int row, int col, BlockType type, int cost)
        {
            Row = row;
            Col = col;
            Type = type;
            Cost = cost;
        }
    }
}
