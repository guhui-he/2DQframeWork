using QFramework;
using UnityEngine;

namespace Game
{
    public enum PerceptionType
    {
        None,       //没有对象
        Detected,   //有对象
    }
    public interface IEnemyPerceptionSystem : ISystem { }

    public class EnemyPerceptionSystem : AbstractSystem, IEnemyPerceptionSystem
    {
        private IEnemyIconModel mEnemyModel;

        private const float DetectRangeSqr = 5f*5f;
        private const float LoseRangeSqr = 6f*6f;

        protected override void OnInit()
        {
            mEnemyModel = this.GetModel<IEnemyIconModel>();

            this.RegisterEvent<PlayerPositionChangedEvent>(e =>
            {
                var enemies = mEnemyModel.Enemies;
                for (int i = 0; i < enemies.Count; i++)
                {
                    var enemy = enemies[i];
                   enemy.LastKnownPlayerPos.Value = e.Position;
                }
            });

            ActionKit.Repeat().Callback(Tick).StartGlobal();
        }

        private void Tick()
        {
            var enemies = mEnemyModel.Enemies;
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                //距离判断参数
                var enemyPos = enemy.CurrentPosition.Value;
                var playerPos = enemy.LastKnownPlayerPos.Value;
                float sqrDist = (enemyPos - playerPos).sqrMagnitude;
                //追逐类型判断
                var currentPerception = enemy.PerceptionResult.Value;
                bool isTracking = currentPerception == PerceptionType.Detected;
                if (!isTracking && sqrDist < DetectRangeSqr)
                    isTracking = true;
                else if (isTracking && sqrDist > LoseRangeSqr)
                    isTracking = false;
                //追逐类型赋值
                var newPerception = isTracking ? PerceptionType.Detected : PerceptionType.None;
                if (newPerception != currentPerception)
                    enemy.PerceptionResult.Value = newPerception;
            }
        }
    }
}
