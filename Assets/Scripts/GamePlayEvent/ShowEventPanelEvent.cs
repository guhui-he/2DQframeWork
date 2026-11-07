
using UnityEngine;

namespace Game
{
    public class EventDisPlayData
    {
        public string Name;
        public string Description;
        public Sprite Icon;
    }
    public struct ShowEventPanelEvent
    {
        public EventDisPlayData Data;
    }
    public struct HideEventPanelEvent { }
}
