using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPerceptionSystem : ISystem
    {
        // 扫描周围实体（返回所有看到的目标，由行为逻辑自己判断敌友）
        List<PerceivedTarget> ScanNearbyTargets(PartyInfo self, float radius);
    }

    public class PerceptionSystem : AbstractSystem, IPerceptionSystem
    {
        IPartyModel partyModel;
        IPlayerModel playerModel;
        ISettlementModel settlementModel;
        protected override void OnInit()
        {
             partyModel = this.GetModel<IPartyModel>();
            playerModel = this.GetModel<IPlayerModel>();
            settlementModel = this.GetModel<ISettlementModel>();
        }
        public List<PerceivedTarget> ScanNearbyTargets(PartyInfo self, float radius)
        {

            var results = new List<PerceivedTarget>();
            float sqrRadius = radius * radius;
            Vector2 selfPos = self.CurrentPosition.Value;
            foreach (var other in partyModel.Parties)
            {
                if (other.PartyId == self.PartyId) continue;

                float sqrDist = (other.CurrentPosition.Value - selfPos).sqrMagnitude;
                if (sqrDist <= sqrRadius)
                {
                    results.Add(new PerceivedTarget
                    {
                        TargetId = other.PartyId,
                        Category = TargetCategory.Party,
                        Faction = other.Faction,
                        Position = other.CurrentPosition.Value,
                        Distance = Mathf.Sqrt(sqrDist)
                    });
                }
            }

            // 2. 扫描玩家
            float sqrDistToPlayer = (playerModel.Position.Value - selfPos).sqrMagnitude;
            if (sqrDistToPlayer <= sqrRadius)
            {
                results.Add(new PerceivedTarget
                {
                    TargetId = -1,
                    Category = TargetCategory.Player,
                    Faction = playerModel.Faction,
                    Position = playerModel.Position.Value,
                    Distance = Mathf.Sqrt(sqrDistToPlayer)
                });
            }

            // 3. 扫描城市/巢穴（建筑）
            foreach (var settlement in settlementModel.Settlements.Values)
            {
                Vector2 sPos = new Vector3(settlement.Pos.x, settlement.Pos.y);
                float sqrDist = (sPos - selfPos).sqrMagnitude;
                if (sqrDist <= sqrRadius)
                {
                    results.Add(new PerceivedTarget
                    {
                        TargetId = settlement.Id,
                        Category = TargetCategory.Settlement,
                        // 建筑也带势力（比如城市是City势力，巢穴是Monster势力）
                        Faction = settlement.config.Faction,
                        Position = sPos,
                        Distance = Mathf.Sqrt(sqrDist)
                    });
                }
            }

            return results;
        }

       
    }
}
