namespace Game
{
    public enum AreaType { Bag, Equipment, Shop, Loot, QuestSubmit }

    public struct DropTargetInfo
    {
        public bool IsValid;
        public AreaType TargetArea;
        public ItemType AcceptType;   // 装备槽的目标类型
        public int TargetId;          // 商店 CityId 等
    }
}
