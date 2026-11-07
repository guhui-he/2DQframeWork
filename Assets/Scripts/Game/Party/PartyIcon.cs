using UnityEngine;
using QFramework;

namespace Game
{
	public partial class PartyIcon : ViewController,IController
	{
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private IPartyModel mPartyModel;
        public int id;
        public void Init(int enemyId)
        {
            mPartyModel = this.GetModel<IPartyModel>();
            id=enemyId;
            var data = mPartyModel.GetParty(enemyId);
            if (icon.AnimationState != null)
            {
               icon.AnimationState.ClearTracks();
            }
            icon.skeletonDataAsset = this.GetSystem<IResSystem>().GetSkeleton(data.Config.IconPath);
            icon.Initialize(true);
            icon.AnimationState.SetAnimation(0, "Idle", true);
            data.CurrentPosition.RegisterWithInitValue(curPos =>
            {
                transform.position = curPos;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            if (!data.HasSpawnedInitialMembers)
            {
                data.HasSpawnedInitialMembers = true;
                this.SendCommand(new CreatEnemyCommand(data));
            }
        }
    }
}
