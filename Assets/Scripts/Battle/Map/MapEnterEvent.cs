using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public struct SceneEnterEvent
    {
        public int EnemyId;
        public SceneEnterEvent(int id)
        { EnemyId = id; }
    }

    public struct MapEnterEvent
    {
        public int row;
        public int col;
        public BlockRawData[] rawDatas;

        public MapEnterEvent(int  row, int col, BlockRawData[] datas)
        {
            this.row = row;
            this.col = col;
            this.rawDatas = datas;
        }
    }
}
