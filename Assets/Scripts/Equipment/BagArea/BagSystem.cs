
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBagSystem : ISystem
    {
        void RemoveItem(ItemInfo item);

        void AddItem(ItemInfo item);
    }
    public class BagSystem : AbstractSystem, IBagSystem
    {
        private IBagModel   mBagModel;
        protected override void OnInit()
        {
            mBagModel=this.GetModel<IBagModel>();

            Test();
        }
        void Test()
        {
            var items = this.GetSystem<IEquipmentFactorySystem>()
                .CreateRandomBatch(ItemType.Weapon, 5);

            foreach (var item in items)
            { 
                mBagModel.AddItem(item);
            }
            var role = this.GetSystem<IRoleFactorySystem>();
            var model = this.GetModel<IHeroModel>();
            model.AddHero(role.CreateRole(RoleType.Hero,true));
            model.AddHero(role.CreateRole(RoleType.Hero,true));
        }

        public void AddItem(ItemInfo item)
        {
            mBagModel.AddItem(item);
        }
        public void RemoveItem(ItemInfo item)
        {
            mBagModel.RemoveItem(item);
        }

    }
}
