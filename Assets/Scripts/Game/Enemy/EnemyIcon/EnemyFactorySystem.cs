
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEnemyFactorySystem : ISystem
    {
        void CreatEnemy(EnemyIconData data);
    }
    /// <summary>
    /// 为每个队伍配置队伍人数
    /// </summary>
    public class EnemyFactorySystem : AbstractSystem, IEnemyFactorySystem
    {
        private IEnemyIconModel mEnemyIconModel;
        private ITimeSystem mTimeSystem;
        private ITimeModel mTimeModel;
        private IEnemyModel mEnemyModel;
        protected override void OnInit()
        {
            mEnemyIconModel = this.GetModel<IEnemyIconModel>();
            mTimeSystem = this.GetSystem<ITimeSystem>();
            mTimeModel = this.GetModel<ITimeModel>();
            mEnemyModel = this.GetModel<IEnemyModel>();

            mTimeSystem.RegisterTick(OnTick);
        }

        private void OnTick(float dt)
        {
            float gameDt = dt / mTimeModel.SecondsPerRealSecond;
            var enemies = mEnemyIconModel.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                
                // 倒计时递减
                enemy.CreatTimer -= gameDt;

                if (enemy.CreatTimer <= 0)
                {
                    // 触发下一次召唤
                   CreatEnemy(enemy);                   
                    // 重置倒计时
                    enemy.CreatTimer = 20f; 
                }
            }
        }
        public void CreatEnemy(EnemyIconData data)
        {
          var enemy=this.GetSystem<IRoleFactorySystem>().CreateRole(RoleType.Enemy, data.EnemyId);
            mEnemyModel.AddEnemy(data.Id, enemy);
        }
    }
}
