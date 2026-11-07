// BagModel.cs
using QFramework;

namespace Game
{
    public interface IBagModel : IItemContainerModel
    {
        bool IsEquipped(int itemId);
    }

    public class BagModel : ItemContainerModel, IBagModel
    {
        protected override AreaType ContainerArea => AreaType.Bag;

        public bool IsEquipped(int  itemId)
        {
            foreach(var item in Items) 
                if(item.itemData.Id == itemId) 
                    return true;
            return false;
        }
    }
}
