using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ISpawnPartySystem : ISystem { }

    public class SpawnPartySystem : AbstractSystem, ISpawnPartySystem
    {
        private ISettlementModel mSettlementModel;
        private IPartyModel mPartyModel;
        private IWorldMapModel mWorldMapModel;
        private ITimeSystem mTimeSystem;
        private IPartyConfigModel mPartyConfigModel;
        private IGlobalModel mGlobalModel;

        protected override void OnInit()
        {
            mSettlementModel = this.GetModel<ISettlementModel>();
            mPartyModel = this.GetModel<IPartyModel>();
            mWorldMapModel = this.GetModel<IWorldMapModel>();
            mTimeSystem = this.GetSystem<ITimeSystem>();
            mPartyConfigModel = this.GetModel<IPartyConfigModel>();
            mGlobalModel = this.GetModel<IGlobalModel>();
            mTimeSystem.RegisterRealTick(OnTick);
        }

        private void OnTick(float dt)
        {
            foreach (var building in mSettlementModel.Settlements.Values)
            {
                int count = mPartyModel.Parties.Count(p => p.OwnerId == building.Id);
                if (count >= building.config.PartyNum) continue;

                building.SpawnTimer -= dt;
                if (building.SpawnTimer <= 0)
                {
                    building.SpawnTimer = building.SpawnInterval;
                    if (building.config.PartyPool == null || building.config.PartyPool.Count == 0) continue;
                    int randomPartyConfigId = building.config.PartyPool[Random.Range(0, building.config.PartyPool.Count)];

                    var partyConfig = mPartyConfigModel.GetConfig(randomPartyConfigId);
                    if (partyConfig == null) continue;
                    Vector3 pos = new Vector3(building.Pos.x, building.Pos.y, 0);
                    if (building.Type == SettlementType.Wilderness)
                    {
                        var cells = mWorldMapModel.WalkableCells.ToList();
                        if (cells.Count > 0)
                        {
                            var p = cells[Random.Range(0, cells.Count)];
                            pos = new Vector3(p.x, p.y, 0);
                        }
                    }
                    var partyRolePool = new List<int>(building.config.RolePool);
                    mPartyModel.CreateParty(mGlobalModel.GetNextPartyId(),partyConfig,building.Id, building.config.Faction ,partyRolePool, pos);
                }
            }
        }
    }
}
