using QFramework;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public interface IPartyAISystem : ISystem { }

    public class PartyAISystem : AbstractSystem, IPartyAISystem
    {
        // 用 PartyType 作为 Key 来分发行为逻辑
        private Dictionary<PartyType, IPartyBehavior> mBehaviors = new();

        protected override void OnInit()
        {
            mBehaviors[PartyType.Caravan] = new CaravanBehavior();
            mBehaviors[PartyType.Enemy] = new MonsterBehavior();
            mBehaviors[PartyType.Wanderer] = new MonsterBehavior();

            var timeSystem = this.GetSystem<ITimeSystem>();
            var partyModel = this.GetModel<IPartyModel>();

            timeSystem.RegisterTick(dt =>
            {
                var t = dt / 60;

                foreach (var party in partyModel.Parties)
                {
                    MoveAlongPath(party, t);
                    if (party.State == PartyState.Chase)
                    {
                        if (party.PathQueue.Count == 0)
                        {
                            party.State = PartyState.Idle;
                        }
                    }
                    if (mBehaviors.TryGetValue(party.Type, out var behavior))
                    {
                        behavior.Update(party, t);
                    }
                }
            });
        }

        private void MoveAlongPath(PartyInfo party, float t)
        {
            if (party.PathQueue.Count > 0)
            {
                Vector3 nextTarget = party.PathQueue.Peek();
                party.CurrentPosition.Value = Vector3.MoveTowards(party.CurrentPosition.Value, nextTarget, party.Config.MoveSpeed * t);
                if (Vector3.Distance(party.CurrentPosition.Value, nextTarget) < 0.1f)
                {
                    party.CurrentPosition.Value = nextTarget;
                    party.PathQueue.Dequeue();
                }
            }
        }
    }

    public struct PartyBattleEvent
    {
        public int AttackerId;
        public int DefenderId;
        public PartyBattleEvent(int a, int d) { AttackerId = a; DefenderId = d; }
    }
}
