using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRandomItemSystem : ISystem
    {
        ItemConfigData GetRandomByType(ItemType type);
        ItemConfigData GetRandomByType(ItemType type, int minLevel, int maxLevel);
        List<ItemConfigData> GetRandomByType(ItemType type, int count);
        ItemConfigData GetRandomAny();
        List<ItemConfigData> GetRandomAny(int count);
    }
    public class RandomItemSystem : AbstractSystem, IRandomItemSystem
    {
        private IItemConfigModel mItemModel;

        private System.Random mRandom = new System.Random();//种子随机

        protected override void OnInit()
        {
            mItemModel = this.GetModel<IItemConfigModel>();
        }

        // ==================== 单个 ====================

        public ItemConfigData GetRandomByType(ItemType type)
        {
            var list = mItemModel.GetItemsByType(type);
            if (list.Count == 0) return null;
            return list[mRandom.Next(0, list.Count)];
        }

        public ItemConfigData GetRandomByType(ItemType type, int minLevel, int maxLevel)
        {
            var list = mItemModel.GetItemsByType(type);
            var candidates = FilterByLevel(list, minLevel, maxLevel);
            if (candidates.Count == 0) return null;
            return candidates[mRandom.Next(0, candidates.Count)];
        }

        public ItemConfigData GetRandomAny()
        {
            var all = CollectAllItems();
            if (all.Count == 0) return null;
            return all[mRandom.Next(0, all.Count)];
        }

        // ==================== 批量 ====================

        public List<ItemConfigData> GetRandomByType(ItemType type, int count)
        {
            var list = mItemModel.GetItemsByType(type);
            return PickRandom(list, count);
        }

        public List<ItemConfigData> GetRandomAny(int count)
        {
            var all = CollectAllItems();
            return PickRandom(all, count);
        }

        // ==================== 私有工具 ====================

        private List<ItemConfigData> FilterByLevel(IReadOnlyList<ItemConfigData> source, int min, int max)
        {
            var result = new List<ItemConfigData>();
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].Level >= min && source[i].Level <= max)
                    result.Add(source[i]);
            }
            return result;
        }

        private List<ItemConfigData> CollectAllItems()
        {
            var all = new List<ItemConfigData>();
            foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
                all.AddRange(mItemModel.GetItemsByType(type));
            return all;
        }

        private List<ItemConfigData> PickRandom(IReadOnlyList<ItemConfigData> pool, int count)
        {
            var result = new List<ItemConfigData>();
            if (pool.Count == 0 || count <= 0) return result;
            for (int i = 0; i < count; i++)
                result.Add(pool[mRandom.Next(0, pool.Count)]);
 

            return result;
        }
    }
}
