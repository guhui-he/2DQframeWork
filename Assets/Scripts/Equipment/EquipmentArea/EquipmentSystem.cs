using QFramework;

namespace Game
{
    public interface IEquipmentSystem : ISystem
    {
        bool Equip(ItemInfo item);      // 从背包装备
        bool Unequip(ItemType slot);    // 卸下到背包
    }

    public class EquipmentSystem : AbstractSystem, IEquipmentSystem
    {
        private IBagModel mBag;
        private IHeroModel mHero;

        protected override void OnInit()
        {
            mBag = this.GetModel<IBagModel>();
            mHero = this.GetModel<IHeroModel>();
        }

        public bool Equip(ItemInfo item)
        {
            if (item == null) return false;          
            var slot = item.itemData.Type;
            var previous = mHero.GetHero().Equipment.GetEquipped(slot);
            mBag.RemoveItem(item);
            if (previous != null)
            {
                Unequip(slot);
            }
            item.area = AreaType.Equipment;
            mHero.GetHero().Equipment.SetEquipped(slot, item);
            return true;
        }

        public bool Unequip(ItemType slot)
        {
            var item = mHero.GetHero().Equipment.GetEquipped(slot);
            if (item == null) return false;

            mHero.GetHero().Equipment.RemoveEquipped(slot);
            mBag.AddItem(item);

            return true;
        }
    }
}
