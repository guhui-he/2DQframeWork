using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>专门管理 SlotListView 槽位的池
    public class SlotPool
    {
        private readonly GameObject prefab;
        private readonly Transform parent;

        private readonly List<Item> active = new List<Item>();  
        private readonly Stack<Item> cache = new Stack<Item>(); 

        public int ActiveCount => active.Count;

        public SlotPool(GameObject prefab, Transform parent, int prewarm = 10)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < prewarm; i++)
                cache.Push(Create());
        }

        /// <summary>追加一个槽到末尾</summary>
        public Item Append(ItemInfo data)
        {
            var slot = cache.Count > 0 ? cache.Pop() : Create();
            slot.transform.SetParent(parent, false);
            slot.gameObject.SetActive(true);
            slot.SetData(data);
            active.Add(slot);
            slot.transform.SetSiblingIndex(active.Count - 1);
            return slot;
        }

        /// <summary>移除指定索引的槽,后面的槽自动前移</summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= active.Count) return;

            var slot = active[index];
            active.RemoveAt(index);
            Recycle(slot);
            for (int i = index; i < active.Count; i++)
                active[i].transform.SetSiblingIndex(i);
        }

        /// <summary>清空所有活跃槽</summary>
        public void Clear()
        {
            for (int i = 0; i < active.Count; i++)
                Recycle(active[i]);
            active.Clear();
        }

        /// <summary>刷新指定索引的数据</summary>
        public void RemoveByItem(ItemInfo item)
        {
            for (int i = 0; i < active.Count; i++)
            {
                if (active[i].GetItemInfo() == item)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        private void Recycle(Item slot)
        {
            slot.SetData(null);
            slot.gameObject.SetActive(false);
            slot.transform.SetParent(parent, false);
            cache.Push(slot);
        }

        private Item Create()
        {
            var go = Object.Instantiate(prefab, parent);
            go.SetActive(false);
            return go.GetComponent<Item>();
        }
    }
}
