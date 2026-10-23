using QFramework;
using UnityEngine;

namespace Game
{
 
    public interface IEnemyMoveSystem : ISystem { }
    public class EnemyMoveSystem : AbstractSystem, IEnemyMoveSystem
    {
        private IEnemyIconModel mEnemyModel;

        protected override void OnInit()
        {
            mEnemyModel = this.GetModel<IEnemyIconModel>();
            var timeModel=this.GetModel<ITimeModel>();
            var timeSystem = this.GetSystem<ITimeSystem>();
            timeSystem.RegisterTick(dt =>
            {
                float deltaGameTime = dt /timeModel.SecondsPerRealSecond; // 转换成游戏时间步长

                foreach (var enemy in mEnemyModel.Enemies)
                {
                    var cur = enemy.CurrentPosition.Value;
                    var target = enemy.TargetPosition.Value;

                    if (Vector3.Distance(cur, target) < 0.001f) continue;
                    enemy.CurrentPosition.Value = Vector3.MoveTowards(
                        cur, target, enemy.MoveSpeed * deltaGameTime);
                }
            });
        }
    }
}
