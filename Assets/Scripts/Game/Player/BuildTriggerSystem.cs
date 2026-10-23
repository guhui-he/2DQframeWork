
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
        IBuildModel mBuildModel;
        protected override void OnInit()
        {
            mBuildModel = this.GetModel<IBuildModel>();
        }

        public void PlayerEnterBuild(int buildId)
        {
            mBuildModel.CurBuildId = buildId;
            this.SendEvent(new BuildTriggerEnteredEvent(buildId));
        }

        public void PlayerExitBuild(int buildId)
        {
            this.SendEvent(new BuildTriggerExitedEvent(buildId));
        }
    }
}
