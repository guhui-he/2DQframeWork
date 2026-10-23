using UnityEngine;

namespace Game
{

    public struct PlayerPositionChangedEvent
    {
        public Vector2 Position;
        public PlayerPositionChangedEvent(Vector2 pos) => Position = pos;
    }
}
