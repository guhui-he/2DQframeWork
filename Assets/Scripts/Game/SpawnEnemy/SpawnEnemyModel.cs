using System;
using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface ISpawnEnemyModel : IModel
    {
        Dictionary<int,SpawnEnemyData> SpawnConfigs {  get; }
        Dictionary<int, float> SpawnTimers { get; }
        void AddSpawnEnemy(SpawnEnemyData data);
        SpawnEnemyData GetSpwanEnemyData(int id);
        void InitTimers(float defaultTime);
    }
    public class SpawnEnemyModel : AbstractModel, ISpawnEnemyModel
    {
        public  Dictionary<int,SpawnEnemyData> SpawnConfigs { get;}=new ();
        public Dictionary<int, float> SpawnTimers { get; } = new();
        protected override void OnInit()
        {
            
        }
        public void AddSpawnEnemy(SpawnEnemyData data)
        {
            SpawnConfigs[data.Id] = data;
            SpawnTimers[data.Id] = 5f;
        }
        public SpawnEnemyData GetSpwanEnemyData(int id) => 
            SpawnConfigs.TryGetValue(id, out var data) ? data : null;
        public void InitTimers(float defaultTime)
        {
            SpawnTimers.Clear();
            foreach (var id in SpawnConfigs.Keys)
                SpawnTimers[id] = defaultTime;
        }
    }
}
