using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 管理 SlotListView 的有序活跃列表 + 池。
    /// 对象生命周期交给 ComponentPool&lt;Item&gt;，这里只负责顺序与数据映射。
    /// </summary>
    public class SlotPool
    {
        private readonly List<Item> _active = new List<Item>();
        private readonly ComponentPool<Item> _pool;
        private readonly Transform _parent;

        public int ActiveCount => _active.Count;
        public int CachedCount => _pool.CachedCount;

        public SlotPool(GameObject prefab, Transform parent, int prewarm = 10)
        {
            _parent = parent;
            _pool = new ComponentPool<Item>(
                prefab,
                parent,
                initCount: prewarm,
                onAllocate: slot =>
                {
                    // 保证从池里拿出来的槽挂回正确的父节点（防御性，正常情况已在此）
                    if (slot.transform.parent != _parent)
                        slot.transform.SetParent(_parent, false);
                },
                onRecycle: slot =>
                {
                    // 回收时清数据，避免图标/文字残留在池里
                    slot.SetData(null);
                });
        }

        /// <summary>追加一个槽到末尾。</summary>
        public Item Append(ItemInfo data)
        {
            var slot = _pool.Allocate();
            slot.SetData(data);
            _active.Add(slot);
            slot.transform.SetSiblingIndex(_active.Count - 1);
            return slot;
        }

        /// <summary>移除指定索引的槽，后面的槽自动前移。</summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _active.Count) return;

            var slot = _active[index];
            _active.RemoveAt(index);
            _pool.Recycle(slot);

            for (int i = index; i < _active.Count; i++)
                _active[i].transform.SetSiblingIndex(i);
        }

        /// <summary>按数据对象移除（首个匹配）。</summary>
        public void Remove(ItemInfo item)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].GetItemInfo() == item)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>清空所有活跃槽（数据保留在池中等待复用）。</summary>
        public void Clear()
        {
            for (int i = 0; i < _active.Count; i++)
                _pool.Recycle(_active[i]);
            _active.Clear();
        }
    }
}
