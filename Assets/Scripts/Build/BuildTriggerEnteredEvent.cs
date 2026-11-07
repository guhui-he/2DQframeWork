

namespace Game
{ 
    public  struct BuildTriggerEnteredEvent
    {
        public int BuildId;
        public BuildTriggerEnteredEvent(int id) { this.BuildId = id; }
    }

    public struct BuildTriggerExitedEvent
    {
        public int BuildId;
        public BuildTriggerExitedEvent(int id) { this.BuildId = id; }
    }
    public struct StoreEnterEvent
    { 
        public int BuildId;
        public StoreEnterEvent(int id)=>BuildId = id;
    }

}
