
using UnityEngine;

namespace Game
{
    public class SlotListView : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private int prewarmCount = 10;

        private SlotPool mPool;

        void Awake()
        {
            mPool = new SlotPool(slotPrefab, transform, prewarmCount);
        }

        public void Append(ItemInfo data) => mPool.Append(data);

        public void RemoveAt(int index) => mPool.RemoveAt(index);

        public void Remove(ItemInfo item) => mPool.RemoveByItem(item);

        public void Clear() => mPool.Clear();
    }
}
