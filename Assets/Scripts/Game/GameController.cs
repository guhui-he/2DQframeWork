using UnityEngine;
using QFramework;

namespace Game
{
    public partial class GameController : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        void Start()
        {
            UIKit.OpenPanel<GamePanel>();

            this.RegisterEvent<GameLoadedEvent>(e =>
            {
                var partyModel = this.GetModel<IPartyModel>();
                foreach (var party in partyModel.Parties)
                {
                    var loader = this.GetUtility<IPrefabLoaderUtility>();
                    var go = loader.Instantiate("PartyIcon",
                        pos: new Vector3(party.CurrentPosition.Value.x, party.CurrentPosition.Value.y, -1));
                    go.GetComponent<PartyIcon>().Init(party.PartyId);
                }

                var mapManager = FindObjectOfType<MapManager>();
                if (mapManager != null)
                {
                    mapManager.RefreshNestViews(this.GetModel<ISettlementModel>());    
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            var gameModel = this.GetModel<IGameStartModel>();
            if (gameModel.IsLoadGameMode.Value)
            {
                ActionKit.DelayFrame(1, () =>
                {
                    this.GetSystem<ISaveSystem>().LoadGame();
                }).StartGlobal();        
            }
        }
    }
}
