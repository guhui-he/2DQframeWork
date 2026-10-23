using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IItemConfigModel : IModel
    {
        IReadOnlyList<ItemConfigData> GetItemsByType(ItemType type);

        ItemConfigData GetItem(ItemType type, int id);
        ItemConfigData GetItem(int id);

        void AddItem(ItemType type, ItemConfigData item);
    }
    /// <summary>
    /// 未赋词条的装备配置表
    /// </summary>
    public class ItemConfigModel : AbstractModel, IItemConfigModel
    {
        private readonly Dictionary<ItemType, List<ItemConfigData>> mItemsByType
            = new Dictionary<ItemType, List<ItemConfigData>>();

        private readonly Dictionary<int, ItemConfigData> mItemById
            = new Dictionary<int, ItemConfigData>();//id查找具体装备(不同类型装备id不同)

        protected override void OnInit()
        {
            
            foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
            {
                mItemsByType[type] = new List<ItemConfigData>();
            }
        }

        // ==================== 查询 API ====================

        public IReadOnlyList<ItemConfigData> GetItemsByType(ItemType type)
        {
            if (mItemsByType.TryGetValue(type, out var list))
                return list;
            return System.Array.Empty<ItemConfigData>();
        }

        public ItemConfigData GetItem(ItemType type, int id)
        {
            var list = GetItemsByType(type);
            for (int i = 0; i < list.Count; i++)
                if (list[i].Id == id) return list[i];
            return null;
        }

        public ItemConfigData GetItem(int id)
        {
            return mItemById.TryGetValue(id, out var item) ? item :null; 
        }


        public void AddItem(ItemType type, ItemConfigData item)
        {
            if (!mItemsByType.TryGetValue(type, out var list))
            {
                list = new List<ItemConfigData>();
                mItemsByType[type] = list;
            }

            list.Add(item);
            mItemById[item.Id] = item;
        }
    }
}
