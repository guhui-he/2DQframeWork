using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class BaseBehavior : IPartyBehavior
    {
        private IPartyModel mPartyModel;
        protected IPartyModel PartyModel => mPartyModel ??= GameApp.Interface.GetModel<IPartyModel>();

        private IWorldMapModel mMapModel;
        protected IWorldMapModel MapModel => mMapModel ??= GameApp.Interface.GetModel<IWorldMapModel>();

        private ISettlementModel mSettlementModel;
        protected ISettlementModel SettlementModel => mSettlementModel ??= GameApp.Interface.GetModel<ISettlementModel>();

        private IRelationModel mRelationModel;
        protected IRelationModel RelationModel => mRelationModel ??= GameApp.Interface.GetModel<IRelationModel>();

        public abstract void Update(PartyInfo party, float dt);
    }
}
  