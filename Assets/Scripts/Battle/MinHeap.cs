using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>A* 开表节点：Block + f 值</summary>
    internal struct PathNode
    {
        public BlockData Block;
        public int F;

        public PathNode(BlockData block, int f) { Block = block; F = f; }
    }

    /// <summary>轻量二叉最小堆，避免 Unity 缺 PriorityQueue</summary>
    internal class MinHeap<T> where T : struct
    {
        private readonly List<T> mItems = new List<T>();
        private readonly Comparison<T> mCompare;

        public MinHeap(Comparison<T> compare) => mCompare = compare;

        public int Count => mItems.Count;

        public void Push(T item)
        {
            mItems.Add(item);
            SiftUp(mItems.Count - 1);
        }

        public T Pop()
        {
            var top = mItems[0];
            int last = mItems.Count - 1;
            mItems[0] = mItems[last];
            mItems.RemoveAt(last);
            if (mItems.Count > 0) SiftDown(0);
            return top;
        }

        private void SiftUp(int i)
        {
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (mCompare(mItems[i], mItems[p]) >= 0) break;
                (mItems[i], mItems[p]) = (mItems[p], mItems[i]);
                i = p;
            }
        }

        private void SiftDown(int i)
        {
            int n = mItems.Count;
            while (true)
            {
                int l = i * 2 + 1, r = l + 1, smallest = i;
                if (l < n && mCompare(mItems[l], mItems[smallest]) < 0) smallest = l;
                if (r < n && mCompare(mItems[r], mItems[smallest]) < 0) smallest = r;
                if (smallest == i) break;
                (mItems[i], mItems[smallest]) = (mItems[smallest], mItems[i]);
                i = smallest;
            }
        }
    }
}
