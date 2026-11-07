using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public abstract class BaseCombatBehavior : BaseBehavior
    {
        protected PerceivedTarget FindClosestEnemy(PartyInfo self, List<PerceivedTarget> perceivedTargets)
        {
           PerceivedTarget closest = null;
        float minDist = float.MaxValue;

            foreach (var target in perceivedTargets)
            {
                if (RelationModel.IsHostile(self.Faction, target.Faction))
                {
                    if (target.Distance<minDist)
                    {
                        minDist = target.Distance;
                        closest = target;
                    }
}
            }
            return closest;
        }

        protected void ChaseTarget(PartyInfo self, PerceivedTarget target, float dt)
        {
            if (target.Distance < 1f)
            {
                //if (target.Category == TargetCategory.Player)
                //    //GameApp.Interface.SendEvent(new PlayerEnterBattleEvent(self.PartyId));
                //else if (target.Category == TargetCategory.Party)
                //    GameApp.Interface.SendEvent(new PartyBattleEvent(self.PartyId, target.TargetId));
                //else if (target.Category == TargetCategory.Settlement)
                //    //GameApp.Interface.SendEvent(new AttackSettlementEvent(target.TargetId)); // 攻城事件

                self.State = PartyState.Idle;
                self.PathQueue.Clear();
                return;
            }

            if (self.PathQueue.Count == 0 || Random.value < 0.1f)
            {
                CalculatePath(self, target.Position, MapModel);
            }
        }

        protected void WanderAround(PartyInfo self, float dt, bool isPatrol)
        {
            if (self.State == PartyState.Idle)
            {
                self.StateTimer += dt;
                if (self.StateTimer > 3f)
                {
                    self.State = isPatrol ? PartyState.Patrol : PartyState.Idle;
                    self.StateTimer = 0f;
                    Vector3 target = self.BornPosition + (Vector3)(Random.insideUnitCircle * Random.Range(3, 8));
                    CalculatePath(self, target, MapModel);
                    self.State = PartyState.Patrol;
                }
            }
            else if (self.State == PartyState.Patrol)
            {
                if (self.PathQueue.Count == 0)
                {
                    self.State = PartyState.Idle;
                    self.StateTimer = 0f;
                }
            }
        }

        protected void CalculatePath(PartyInfo party, Vector3 targetPos, IWorldMapModel mapModel, bool useRoadOnly = false)
        {
            Vector2Int start = new Vector2Int(Mathf.RoundToInt(party.CurrentPosition.Value.x), Mathf.RoundToInt(party.CurrentPosition.Value.y));
            Vector2Int end = new Vector2Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y));

            System.Func<Vector2Int, bool> isWalkable = useRoadOnly
                ? (System.Func<Vector2Int, bool>)(pos => mapModel.RoadCells.Contains(pos))
                : (pos => mapModel.WalkableCells.Contains(pos));

            var path = AStarGridTool.FindPath(start, end, isWalkable);

            if (path != null && path.Count > 0)
            {
                party.PathQueue.Clear();
                foreach (var cell in path) party.PathQueue.Enqueue(new Vector3(cell.x, cell.y, 0));
                party.TargetPosition.Value = new Vector3(end.x, end.y, 0);
            }
        }
    }
}
