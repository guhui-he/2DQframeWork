using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class MapManager : ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        private List<Nest> _activeNests = new List<Nest>();
        private ComponentPool<Nest> _nestPool;

        private List<PartyIcon> _activeParties = new List<PartyIcon>();
        private ComponentPool<PartyIcon> _partyPool;

        void Start()
        {
            _nestPool = new ComponentPool<Nest>(Nest, entityRoot);
            var loader = this.GetUtility<IPrefabLoaderUtility>();
            GameObject enemyIconPrefab = loader.LoadPrefab("PartyIcon");

            if (enemyIconPrefab != null)
            {
                _partyPool = new ComponentPool<PartyIcon>(enemyIconPrefab, entityRoot);
            }

            var settlementModel = this.GetModel<ISettlementModel>();
            var partyModel = this.GetModel<IPartyModel>();

            this.RegisterEvent<DayChangedEvent>(e =>
            {
                this.GetSystem<IWorldSpawnSystem>().OnNewDay(e.Day);
                RefreshNestViews(settlementModel);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<PartyCreatedEvent>(e => RefreshPartyViews(partyModel)).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.GetSystem<IWorldInitSystem>().InitMapData(background, Wall, road,Region);
            this.GetSystem<IWorldSpawnSystem>().GenerateInitialWorld();

            RefreshNestViews(settlementModel);
            RefreshPartyViews(partyModel);
        }

        private void Update()
        {
           
        }

        public void RefreshNestViews(ISettlementModel settlementModel)
        {
            var nests = settlementModel.Settlements.Values.Where(s => s.Type == SettlementType.Nest).ToList();

            ComponentPoolUtil.Refresh(_activeNests, _nestPool, nests,
               (nestView, data) =>
               {
                   nestView.transform.position = background.GetCellCenterWorld(new Vector3Int(data.Pos.x, data.Pos.y, 0));
                   nestView.Init(data);
               });
        }

        public void RefreshPartyViews(IPartyModel partyModel)
        {
            ComponentPoolUtil.Refresh(_activeParties, _partyPool, partyModel.Parties,
               (partyView, data) =>
               {
                   partyView.transform.position = data.CurrentPosition.Value;
                   partyView.Init(data.PartyId);
               });
        }
    }
}
