using QFramework;
using UnityEngine;

namespace Game
{
    public interface IMapModel : IModel
    {
        int RowCount { get; }
        int ColCount { get; }
        
        BlockData GetBlock(int row, int col);
        BlockType GetBlockType(int row, int col);
        void ChangeBlockType(int row, int col, BlockType type);

        void InitMap(int rowCount, int colCount, BlockRawData[] rawData);
    }

    public class MapModel : AbstractModel, IMapModel
    {
        private BlockData[,] mMapArr;
        
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
       
       
        protected override void OnInit() { }

        public void InitMap(int rowCount, int colCount, BlockRawData[] rawData)
        {
            RowCount = rowCount;
            ColCount = colCount;
            mMapArr = new BlockData[rowCount, colCount];

            // 默认填满障碍，防止越界
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    mMapArr[r, c] = new BlockData(r, c, BlockType.Obstacle, 999);
                }
            }

            // 填充实际数据
            for (int i = 0; i < rawData.Length; i++)
            {
                var raw = rawData[i];
                if (raw.Row >= 0 && raw.Row < rowCount && raw.Col >= 0 && raw.Col < colCount)
                {
                    mMapArr[raw.Row, raw.Col].Type = BlockType.Normal;
                    mMapArr[raw.Row, raw.Col].Cost = raw.Cost;
                }
            }
        }

        public BlockData GetBlock(int row, int col)
        {
            if (mMapArr == null || row < 0 || row >= RowCount || col < 0 || col >= ColCount)
                return null;
            return mMapArr[row, col];
        }

        public BlockType GetBlockType(int row, int col)
        {
            return GetBlock(row, col)?.Type ?? BlockType.Obstacle;
        }

        public void ChangeBlockType(int row, int col, BlockType type)
        {
            var block = GetBlock(row, col);
            if (block != null)
            {
                block.Type = type;
            }
        }
    }
}
