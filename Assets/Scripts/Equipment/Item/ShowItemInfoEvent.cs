namespace Game
{
    public struct ShowItemInfoEvent
    {
        public ItemInfo item;

        public ShowItemInfoEvent(ItemInfo item)
            { this.item = item; }
    }

    public struct HideItemInfoEvent { }

    public struct DragStartedEvent
    {
        public ItemInfo Item;
        public DragStartedEvent(ItemInfo item) => Item = item;
    }

    public struct DragEndedEvent { }
}
