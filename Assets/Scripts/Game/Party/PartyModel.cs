using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPartyModel : IModel
    {
        IReadOnlyList<PartyInfo> Parties { get; }
        PartyInfo GetParty(int id);
        PartyInfo CreateParty(int partyId,PartyConfig config, int ownerId,Faction faction, List<int> rolePool, Vector3 bornPos); 
        void RemoveParty(int id);
        void AddParty(PartyInfo partyInfo);
        void ClearAll();
    }

    public class PartyModel : AbstractModel, IPartyModel
    {
        private readonly List<PartyInfo> mParties = new List<PartyInfo>();
        public IReadOnlyList<PartyInfo> Parties => mParties;
        protected override void OnInit() { }

        public PartyInfo GetParty(int id) => mParties.Find(p => p.PartyId == id);
        public void AddParty(PartyInfo partyInfo)=>mParties.Add(partyInfo);

        public PartyInfo CreateParty(int partyId,PartyConfig config, int ownerId,Faction faction, List<int> rolePool, Vector3 bornPos)
        {
            var data = new PartyInfo(partyId, config, ownerId,faction, rolePool, bornPos);
            mParties.Add(data);
            this.SendEvent(new PartyCreatedEvent(data.PartyId));
            return data;
        }

        public void RemoveParty(int id)
        {
            var p = GetParty(id);
            if (p != null)
            {
                mParties.Remove(p);
                this.SendEvent(new PartyRemovedEvent(id, p.OwnerId));
            }
        }

        public void ClearAll()
        {
            mParties.Clear();
        }
    }

    public struct PartyCreatedEvent { public int PartyId; public PartyCreatedEvent(int id) => PartyId = id; }
    public struct PartyRemovedEvent { public int PartyId; public int OwnerId; public PartyRemovedEvent(int id, int ownerId) { PartyId = id; OwnerId = ownerId; } }
}
