using QFramework;
using UnityEngine;

namespace Game
{
    public interface IShopSystem : ISystem
    {

        void Sell(ItemInfo item);
        void Buy(ItemInfo item);
    }
    public class ShopSystem : AbstractSystem, IShopSystem
    {
        IShopModel mShopModel;
        ITimeModel mTimeModel;
        int id;
        protected override void OnInit()
        {
            
            mShopModel = this.GetModel<IShopModel>();
            mTimeModel = this.GetModel<ITimeModel>();
            this.RegisterEvent<TownEnterEvent>(e =>
            {
                id = e.BuildId;
                TryRefreshShop();
            });
        }
        void TryRefreshShop()
        {
            var LastRefreshTime = mShopModel.GetFreshTime(id);
            var currentTime = mTimeModel.TotalGameSeconds.Value;
            if (currentTime - LastRefreshTime >= 86400f|| LastRefreshTime == 0f)
            {

                mShopModel.Clear(id);
                var items = this.GetSystem<IEquipmentFactorySystem>()
                    .CreateRandomBatch(ItemType.Weapon, 5);
                foreach (var item in items)
                {
                    mShopModel.AddItem(id, item);
                }
                float daysPassed = Mathf.Floor((currentTime - LastRefreshTime) / 86400f);
                LastRefreshTime += daysPassed * 86400f;
                mShopModel.SetFreshTime(id, LastRefreshTime);
            } 
        }
        public void Buy(ItemInfo item)
        {
            mShopModel.RemoveItem(id, item);
        }
        public void Sell(ItemInfo item)
        {
            mShopModel.AddItem(id, item);
        }
    }
}
