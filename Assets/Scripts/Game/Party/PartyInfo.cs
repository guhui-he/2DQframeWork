using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public enum PartyType { Enemy, Caravan, Wanderer,Player }
    public enum PartyState { Idle, Patrol, Chase ,Moving}
    public enum Faction
    {
        Player,    
        Nest,
        Wanderer,
        City,   
    }
    public class PartyInfo
    {
        public int PartyId;
        public PartyConfig Config;
        public PartyType Type => Config.Type;
        public int OwnerId;
        public Faction Faction;
        public List<int> RolePool; 
        public float CreatTimer = 20f;
        public bool HasSpawnedInitialMembers = false;
        public BindableProperty<Vector2> CurrentPosition { get; } = new(Vector2.zero);
        public BindableProperty<Vector2> TargetPosition { get; } = new(Vector2.zero);
        public Queue<Vector3> PathQueue { get; set; } = new();
        public Vector3 BornPosition;
        public PartyState State = PartyState.Idle;
        public float StateTimer = 0f;
        public PartyInfo(int partyId, PartyConfig config, int ownerId,Faction faction, List<int> rolePool, Vector3 bornPos)
        {
            PartyId = partyId;
            Config = config;
            OwnerId = ownerId;
            RolePool = rolePool;
            BornPosition = bornPos;
            CurrentPosition.Value = bornPos;
            TargetPosition.Value = bornPos;
            Faction=faction;
        }
    }
}
