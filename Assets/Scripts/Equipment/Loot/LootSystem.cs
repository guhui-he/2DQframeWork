using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

namespace Game
{
    public interface ILootSystem : ISystem
    {
        void BagToLoot(ItemInfo item);
        void LootToBag(ItemInfo item);
    }

    public class LootSystem : AbstractSystem, ILootSystem
    {
        ILootModel lootModel;
        protected override void OnInit()
        {
            lootModel=this.GetModel<ILootModel>();
        }
        public void BagToLoot(ItemInfo item)
        {
           lootModel.AddItem(item);
        }

        public void LootToBag(ItemInfo item)
        {
            lootModel.RemoveItem(item);
        }
    }
}
