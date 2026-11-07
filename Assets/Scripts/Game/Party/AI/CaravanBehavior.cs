using System.Linq;
using UnityEngine;

namespace Game
{
    public class CaravanBehavior : BaseBehavior
    {
        private const float DetectRadius = 8f;
        private const float ArriveDistance = 0.5f;
        private const float RestDuration = 0f;

        public override void Update(PartyInfo party, float dt)
        {
            bool hasThreat = false;

            foreach (var other in PartyModel.Parties) 
            {
                if (other.PartyId == party.PartyId) continue;

                if (RelationModel.IsHostile(party.Faction, other.Faction) &&
                    Vector3.Distance(party.CurrentPosition.Value, other.CurrentPosition.Value) < DetectRadius)
                {
                    hasThreat = true;
                    break;
                }
            }

            if (hasThreat)
            {
                var closestCity = GetClosestCity(party.CurrentPosition.Value);
                if (closestCity != null)
                {
                    Vector3 cityPos = GridMathTool.GetCellCenterWorld(MapModel, closestCity.Pos);

                    if (Vector3.Distance(party.TargetPosition.Value, cityPos) > ArriveDistance)
                    {
                        party.TargetPosition.Value = cityPos;
                        party.PathQueue.Clear();
                        party.State = PartyState.Moving;
                        party.StateTimer = 0f;
                    }
                }
            }

            // ==========================================
            // 2. 状态机逻辑
            // ==========================================
            if (party.State == PartyState.Idle)
            {
                party.StateTimer += dt;
                if (party.StateTimer >= RestDuration)
                {
                    var ownerCity = SettlementModel.GetSettlement(party.OwnerId); 
                    if (ownerCity == null) return;

                    Vector3 ownerPos = new Vector3(ownerCity.Pos.x, ownerCity.Pos.y, 0);
                    bool isAtHome = Vector3.Distance(party.CurrentPosition.Value, ownerPos) < ArriveDistance;

                    if (isAtHome)
                    {
                        var cities = SettlementModel.Settlements.Values
                            .Where(s => s.Type == SettlementType.City && s.Id != party.OwnerId)
                            .ToList();

                        if (cities.Count > 0)
                        {
                            var target = cities[Random.Range(0, cities.Count)];
                            party.TargetPosition.Value = new Vector3(target.Pos.x, target.Pos.y, 0);
                        }
                        else
                        {
                            party.TargetPosition.Value = party.BornPosition;
                        }
                    }
                    else
                    {
                        party.TargetPosition.Value = ownerPos;
                    }

                    party.State = PartyState.Moving;
                    party.PathQueue.Clear();
                }
                return;
            }

            // ==========================================
            // 3. 移动逻辑
            // ==========================================
            if (party.State == PartyState.Moving)
            {
                if (Vector3.Distance(party.CurrentPosition.Value, party.TargetPosition.Value) < ArriveDistance)
                {
                    party.State = PartyState.Idle;
                    party.StateTimer = 0f;
                    party.PathQueue.Clear();
                    return;
                }

                if (party.PathQueue.Count == 0)
                {
                    Vector2Int start = new Vector2Int((int)(party.CurrentPosition.Value.x), (int)(party.CurrentPosition.Value.y));
                    Vector2Int end = new Vector2Int((int)(party.TargetPosition.Value.x), (int)(party.TargetPosition.Value.y));

                    var path = AStarGridTool.FindPath(start, end, pos => MapModel.RoadCells.Contains(pos));
                    if (path != null && path.Count > 0)
                    {
                        foreach (var cell in path)
                        {
                            Vector3 center = GridMathTool.GetCellCenterWorld(MapModel, cell);
                            party.PathQueue.Enqueue(center);
                        }
                        if (Vector3.Distance(party.PathQueue.LastOrDefault(), party.TargetPosition.Value) > 0.1f)
                        {
                            party.PathQueue.Enqueue(party.TargetPosition.Value);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Id为{party.PartyId}的{party.Config.Name}寻路失败！起点 {start} 或 终点 {end} 可能不在道路上，或者路没连通。");
                        party.State = PartyState.Idle;
                        party.StateTimer = 0f;
                    }
                }
            }
        }

        private SettlementInfo GetClosestCity(Vector3 pos)
        {
            return SettlementModel.Settlements.Values
                .Where(s => s.Type == SettlementType.City)
                .OrderBy(s => Vector3.Distance(pos, new Vector3(s.Pos.x, s.Pos.y, 0)))
                .FirstOrDefault();
        }
    }
}
