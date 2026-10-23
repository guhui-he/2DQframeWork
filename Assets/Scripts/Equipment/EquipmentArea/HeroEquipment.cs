using System.Collections.Generic;
using QFramework;

namespace Game
{
    public class HeroEquipment
    {
        private readonly Dictionary<ItemType, BindableProperty<ItemInfo>> mSlots = new();

        public IReadOnlyDictionary<ItemType, BindableProperty<ItemInfo>> Slots => mSlots;

        public HeroEquipment()
        {
            foreach (ItemType slot in System.Enum.GetValues(typeof(ItemType)))
                mSlots[slot] = new BindableProperty<ItemInfo>(null);
        }

        public ItemInfo GetEquipped(ItemType slot)
            => mSlots.TryGetValue(slot, out var p) ? p.Value : null;

        public void SetEquipped(ItemType slot, ItemInfo item)
        {           
            if (mSlots.TryGetValue(slot, out var p)) p.Value = item;
        }

        public void RemoveEquipped(ItemType slot)
        {
            if (mSlots.TryGetValue(slot, out var p)) p.Value = null;
        }
    }
}
