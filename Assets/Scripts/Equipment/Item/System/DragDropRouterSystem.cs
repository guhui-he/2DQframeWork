using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IDragDropRouterSystem : ISystem
    {
        void Route(ItemInfo item, AreaType sourceArea, DropTargetInfo target);
    }

    public class DragDropRouterSystem : AbstractSystem, IDragDropRouterSystem
    {
        // key = (源区域, 目标区域)，value = 处理委托
        private readonly Dictionary<
            (AreaType, AreaType),
            System.Action<ItemInfo,DropTargetInfo>
        > mRoutes = new();

        protected override void OnInit()
        {
            // ========== 一次性把所有分发规则注册在这 ==========
            Register(AreaType.Bag, AreaType.Equipment, OnBagToEquipment);
            Register(AreaType.Bag, AreaType.Shop, OnBagToShop);
            Register(AreaType.Bag, AreaType.Loot, OnBagToLoot);
            Register(AreaType.Equipment, AreaType.Bag, OnEquipmentToBag);
            Register(AreaType.Shop, AreaType.Bag, OnShopToBag);
            Register(AreaType.Loot, AreaType.Bag, OnLootToBag);
        }

        // ==================== 分发 ====================

        public void Route(ItemInfo item, AreaType sourceArea,  DropTargetInfo target)
        {
           
            if (mRoutes.TryGetValue((sourceArea, target.TargetArea), out var handler))
            {
                handler(item, target);
            }
        }

        private void Register(AreaType source, AreaType target,
                              System.Action<ItemInfo, DropTargetInfo> handler)
        {
            mRoutes[(source, target)] = handler;
        }

        // ==================== 具体规则 ====================

        // ---- Bag → Equipment：装备 ----
        private void OnBagToEquipment(ItemInfo item,  DropTargetInfo target)
        {
            if (item.itemData.Type != target.AcceptType) return;
            this.GetSystem<IEquipmentSystem>().Equip(item);
        }

        // ---- Bag → Shop：出售 ----
        private void OnBagToShop(ItemInfo item, DropTargetInfo target)
        {
            this.GetSystem<IShopSystem>().Sell( item);
            this.GetSystem<IBagSystem>().RemoveItem(item);
        }

        // ---- Bag → Loot：丢到战利品 ----
        private void OnBagToLoot(ItemInfo item, DropTargetInfo target)
        {
            this.GetSystem<ILootSystem>().BagToLoot(item);
            this.GetSystem<IBagSystem>().RemoveItem(item);
        }

        // ---- Equipment → Bag：卸下 ----
        private void OnEquipmentToBag(ItemInfo item, DropTargetInfo target)
        {
            this.GetSystem<IEquipmentSystem>().Unequip(item.itemData.Type);
        }

        // ---- Shop → Bag：购买 ----
        private void OnShopToBag(ItemInfo item, DropTargetInfo target)
        {

            this.GetSystem<IShopSystem>().Buy(item);
            this.GetSystem<IBagSystem>().AddItem(item);
        }

        // ---- Loot → Bag：拾取 ----
        private void OnLootToBag(ItemInfo item, DropTargetInfo target)
        {
            this.GetSystem<ILootSystem>().LootToBag(item);
            this.GetSystem<IBagSystem>().AddItem(item);
        }
    }
}
