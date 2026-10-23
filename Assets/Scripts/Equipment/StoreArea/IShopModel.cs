
using System.Collections.Generic;
using QFramework;
using static UnityEditor.Progress;

namespace Game
{
    public interface IShopModel : IModel
    {
        Dictionary<int, BindableList<ItemInfo>> Items { get; }
        void AddItem(int id,ItemInfo item);
        void RemoveItem(int id,ItemInfo item);
        void Clear(int id);
        Dictionary<int,float> LastRefreshTime { get; }
        float GetFreshTime(int id);
        void SetFreshTime(int id, float time);
    }

    public class ShopModel : AbstractModel, IShopModel
    {
        protected  AreaType ContainerArea => AreaType.Shop;

        public  Dictionary<int, BindableList<ItemInfo>> Items { get; } = new();
        public Dictionary<int, float> LastRefreshTime {  get; }=new();
        protected override void OnInit()
        {
            
        }

        public  void AddItem(int id, ItemInfo item)
        {
            item.area = ContainerArea;
            if (!Items.TryGetValue(id, out var list))
            {
                list = new BindableList<ItemInfo>();
                Items[id] = list;
            }
            list.Add(item);
        }

        public  void RemoveItem(int id, ItemInfo item)
        {
            if (Items.TryGetValue(id, out var list))
            {         
                Items[id].Remove(item);
            }
        }

        public void Clear(int id)
        {
            if (Items.TryGetValue(id, out var list))
            {
                Items[id].Clear();
            }
        }

        public float GetFreshTime(int id)
        {
            return LastRefreshTime.TryGetValue(id, out var time) ? time : 0f;
        }
        public void SetFreshTime(int id, float time)
        {
            LastRefreshTime[id] = time;
        }
    }
}
