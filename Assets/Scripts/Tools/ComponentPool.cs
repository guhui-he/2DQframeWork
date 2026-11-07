using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace Game
{
    /// <summary>
    /// 通用 Component 对象池。
    /// 基于 QFramework.SimpleObjectPool 封装，支持：
    ///   · 组件缓存（避免重复 GetComponent）
    ///   · 分配 / 回收回调
    ///   · GameObject 与 Component 两种 Recycle 入口
    /// </summary>
    public class ComponentPool<T> where T : Component
    {
        private readonly SimpleObjectPool<GameObject> _goPool;
        private readonly Dictionary<GameObject, T> _componentCache = new Dictionary<GameObject, T>();
        private readonly Action<T> _onAllocate;
        private readonly Action<T> _onRecycle;

        /// <summary>已取出但未归还的对象数。</summary>
        public int ActiveCount { get; private set; }

        /// <summary>池内空闲对象数。</summary>
        public int CachedCount => _goPool.CurCount;

        public ComponentPool(
            GameObject prefab,
            Transform parent = null,
            int initCount = 0,
            Action<T> onAllocate = null,
            Action<T> onRecycle = null)
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));

            _onAllocate = onAllocate;
            _onRecycle = onRecycle;

            _goPool = new SimpleObjectPool<GameObject>(
                factoryMethod: () =>
                {
                    var go = UnityEngine.Object.Instantiate(prefab, parent);
                    go.SetActive(false);
                    return go;
                },
                resetMethod: go => go.SetActive(false),
                initCount: initCount);
        }

        public ComponentPool(
            Component prefab,
            Transform parent = null,
            int initCount = 0,
            Action<T> onAllocate = null,
            Action<T> onRecycle = null)
            : this(prefab ? prefab.gameObject : null, parent, initCount, onAllocate, onRecycle) { }

        /// <summary>从池中取出一个组件实例。</summary>
        public T Allocate()
        {
            var go = _goPool.Allocate();
            go.SetActive(true);

            if (!_componentCache.TryGetValue(go, out var comp) || comp == null)
            {
                comp = go.GetComponent<T>();
                if (comp == null) comp = go.AddComponent<T>();
                _componentCache[go] = comp;
            }

            ActiveCount++;
            _onAllocate?.Invoke(comp);
            return comp;
        }

        /// <summary>归还组件实例。</summary>
        public void Recycle(T comp)
        {
            if (comp == null) return;
            _onRecycle?.Invoke(comp);
            _goPool.Recycle(comp.gameObject);
            ActiveCount--;
        }

        /// <summary>归还 GameObject（便于从 List&lt;GameObject&gt; 的旧代码平滑迁移）。</summary>
        public void Recycle(GameObject go)
        {
            if (go == null) return;
            if (_componentCache.TryGetValue(go, out var comp) && comp != null)
                Recycle(comp);
            else
            {
                _goPool.Recycle(go);
                ActiveCount--;
            }
        }
    }
}
