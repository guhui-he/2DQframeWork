
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBuildTriggerSystem : ISystem
    {
         void PlayerEnterBuild(int buildId);
        void PlayerExitBuild(int buildId);
    }

    public class BuildTriggerSystem : AbstractSystem, IBuildTriggerSystem
    {
        ICityConfigModel mBuildModel;
        protected override void OnInit()
        {
            mBuildModel = this.GetModel<ICityConfigModel>();
        }

        public void PlayerEnterBuild(int buildId)
        {
            mBuildModel.CurCityId = buildId;
            this.SendEvent(new BuildTriggerEnteredEvent(buildId));
        }

        public void PlayerExitBuild(int buildId)
        {
            this.SendEvent(new BuildTriggerExitedEvent(buildId));
        }
    }
}
