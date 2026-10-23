using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IMapGenerateSystem : ISystem
    {
        void GenerateMap(int rowCount, int colCount, BlockRawData[] rawData);
    }

    public class MapGenerateSystem : AbstractSystem, IMapGenerateSystem
    {
        private IMapModel mMapModel;
        private IEnemyModel mEnemyModel;
        private IBattleModel mBattleModel;
        private int mPendingEnemyId = -1;
        protected override void OnInit()
        {
            mMapModel = this.GetModel<IMapModel>();
            mEnemyModel = this.GetModel<IEnemyModel>();
            mBattleModel = this.GetModel<IBattleModel>();

            this.RegisterEvent<SceneEnterEvent>(e =>
            {
                mPendingEnemyId = e.EnemyId;
            });
            this.RegisterEvent<MapEnterEvent>(e =>
            {
                GenerateMap(e.row, e.col, e.rawDatas);
                if (mPendingEnemyId != -1)
                {
                    GenerateEnemy(mPendingEnemyId);
                    mPendingEnemyId = -1; // 生成完清空标记
                }
            });
        }

        public void GenerateMap(int rowCount, int colCount, BlockRawData[] rawData)
        {
            mMapModel.InitMap(rowCount, colCount, rawData);
        }

        private void GenerateEnemy(int id)
        {
            var enemies = mEnemyModel.GetRoleInfos(id);
            if (enemies == null || enemies.Count == 0) return;
            var availablePositions = GetAvailablePositions();
            Shuffle(availablePositions);

            var loader = this.GetUtility<IPrefabLoaderUtility>();
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemyInfo = enemies[i];
                var pos = availablePositions[i];
                var data = new BattleUnitData(enemies[i].Id, enemyInfo, false, pos);
                mBattleModel.AddUnit(data);
                Debug.Log(data.UnitId);
                if (BlockViewRegistry.TryGetWorldPosition(pos, out var wp))
                {
                    var obj = loader.Instantiate(enemyInfo.Model, pos: new Vector3(wp.x, wp.y, -1));
                    if (obj != null)
                    {
                        var view = obj.GetComponent<BattleUnitView>();
                        if (view != null)
                        {
                            view.UnitId = data.UnitId;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取全图可通行且无单位的空位
        /// </summary>
        private List<GridPos> GetAvailablePositions()
        {
            var list = new List<GridPos>();
            for (int r = 0; r < mMapModel.RowCount; r++)
            {
                for (int c = 0; c < mMapModel.ColCount; c++)
                {
                    var block = mMapModel.GetBlock(r, c);
                    // 必须是普通地形（不是墙/障碍），且当前格子上没站人
                    if (block != null && block.Type != BlockType.Obstacle)
                    {
                        var pos = new GridPos(r, c);
                        if (mBattleModel.GetUnitAt(pos) == null)
                        {
                            list.Add(pos);
                        }
                    }
                }
            }
            return list;
        }
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
