
using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IItemContainerModel : IModel
    {
        IReadOnlyList<ItemInfo> Items { get; }
        void AddItem(ItemInfo item);
        void RemoveItem(ItemInfo item);
        void Refresh(List<ItemInfo> newItems);
        BindableList<ItemInfo> GetBindableList();
    }
    public abstract class ItemContainerModel : AbstractModel
    {
        private readonly BindableList<ItemInfo> mItems = new BindableList<ItemInfo>();

        public IReadOnlyList<ItemInfo> Items => mItems;
        public BindableProperty<float> LastRefreshTime { get; } = new BindableProperty<float>(0);

        protected abstract AreaType ContainerArea { get; }

        public BindableList<ItemInfo> GetBindableList() => mItems;

        protected override void OnInit() { }

        public void AddItem(ItemInfo item)
        {
            if (item == null) return;
            item.area = ContainerArea;
            mItems.Add(item);
        }

        public void RemoveItem(ItemInfo item)
        {
            if (item == null) return;
            mItems.Remove(item);
        }

        public void Refresh(List<ItemInfo> newItems)
        {
            mItems.Clear();
            if (newItems == null) return;
            foreach (var i in newItems)
            {
                if (i == null) continue;
                i.area = ContainerArea;
                mItems.Add(i);
            }
        }
    }
}
