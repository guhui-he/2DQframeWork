using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class ComponentPoolUtil
    {
        /// <summary>
        /// 按 items 差量刷新一个 active 列表：
        ///   · 多余 → 回收
        ///   · 已存在 → 复用并重新 Init
        ///   · 不足 → 从池中申请
        /// </summary>
        public static void Refresh<TComponent, TData>(
            List<TComponent> active,
            ComponentPool<TComponent> pool,
            IReadOnlyList<TData> items,
            Action<TComponent, TData> onInit)
            where TComponent : Component
        {
            if (active == null) throw new ArgumentNullException(nameof(active));
            if (pool == null) throw new ArgumentNullException(nameof(pool));

            int targetCount = items?.Count ?? 0;

            for (int i = active.Count - 1; i >= targetCount; i--)
            {
                pool.Recycle(active[i]);
                active.RemoveAt(i);
            }

            for (int i = 0; i < targetCount; i++)
            {
                TComponent comp;
                if (i < active.Count)
                {
                    comp = active[i];
                }
                else
                {
                    comp = pool.Allocate();
                    active.Add(comp);
                }
                onInit?.Invoke(comp, items[i]);
            }
        }
    }
}
