using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{

    public interface IEnemyIconModel : IModel
    {
        IReadOnlyList<EnemyIconData> Enemies { get; }
        EnemyIconData GetEnemy(int id);
        EnemyIconData CreateEnemy(int enemyId, int spawnId, Vector3 bornPos, float speed);
        void RemoveEnemy(int id);
        void ClearAll();
        void AddEnemyData(EnemyIconData data);
        int NextId { get; set; }
    }

    public class EnemyIconModel : AbstractModel, IEnemyIconModel
    {
        // 位置
        private readonly List<EnemyIconData> mEnemies = new List<EnemyIconData>();

        public IReadOnlyList<EnemyIconData> Enemies => mEnemies;

        public  int NextId { get; set; } =1;
        public EnemyIconData GetEnemy(int id)
        {
            foreach (var e in mEnemies) if (e.Id == id) return e;
            return null;
        }

        public EnemyIconData CreateEnemy(int enemyId, int spawnId, Vector3 bornPos, float speed)
        {
            var data = new EnemyIconData(NextId++, enemyId, spawnId, bornPos, speed);
            mEnemies.Add(data);
            return data;
        }

        public void RemoveEnemy(int id)
        {
            for (int i = mEnemies.Count - 1; i >= 0; i--)
            {
                if (mEnemies[i].Id == id)
                {
                    mEnemies.RemoveAt(i);
                    return;
                }
            }
        }
        public void ClearAll()
        {
            mEnemies.Clear();
            NextId = 1;
        }
        public void AddEnemyData(EnemyIconData data)
        {
            if (data != null) mEnemies.Add(data);
        }
        protected override void OnInit() { }

    }
}
