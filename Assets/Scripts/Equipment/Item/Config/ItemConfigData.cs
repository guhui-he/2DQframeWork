

namespace Game
{
    public enum ItemType
    {
        Weapon,     
        Shield,     
        Helmet,     
        Armor,      
        Boot,      
        Supplie,    
    }
    public enum EquipSlot
    {
        None,
        Weapon,
        Shield,
        Helmet,
        Armor,
        Boot,
    }
    public  class ItemConfigData
    {
        public int Id;
        public string Name;
        public ItemType Type;
        public string Icon;
        public int Price;
        public string Slot;
        public int Level;
    }
}
