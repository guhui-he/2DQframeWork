using UnityEngine;

namespace Game
{
    public enum TargetCategory { Party, Player, Settlement }

    public class PerceivedTarget
    {
        public int TargetId;
        public TargetCategory Category;
        public Faction Faction;
        public Vector3 Position;
        public float Distance;
    }
}
