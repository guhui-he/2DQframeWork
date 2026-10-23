using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface ISpawnEnemySystem : ISystem { }

    public class SpawnEnemySystem : AbstractSystem, ISpawnEnemySystem
    {
        private ISpawnEnemyModel mModel;
        private ITimeSystem mTimeSystem;
        private ITimeModel mTimeModel;

        protected override void OnInit()
        {
            mModel = this.GetModel<ISpawnEnemyModel>();
            mTimeSystem = this.GetSystem<ITimeSystem>();
            mTimeModel = this.GetModel<ITimeModel>();

            mTimeSystem.RegisterTick(OnTick);
        }

        private void OnTick(float dt)
        {
            float gameDt = dt / mTimeModel.SecondsPerRealSecond;
            var readyToSpawn = new List<int>();

            var keys = new List<int>(mModel.SpawnTimers.Keys);
            foreach (var id in keys)
            {
                mModel.SpawnTimers[id] -= gameDt;
                if (mModel.SpawnTimers[id] <= 0f)
                {
                    readyToSpawn.Add(id);
                    mModel.SpawnTimers[id] = 5f; // 重置CD
                }
            }

            foreach (var id in readyToSpawn)
            {
                this.SendEvent(new SpawnEnemyTriggerEvent(id));
            }
        }
    }
}
