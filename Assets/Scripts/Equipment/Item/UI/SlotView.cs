// SlotView.cs
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// 槽位基类：统一处理拖拽、悬停、数据绑定。
    /// </summary>
    public abstract class SlotView : ViewController,
        IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler,
        IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        //[SerializeField] protected int sourceId;

        [Header("拖拽权限")]
        [SerializeField] protected bool canDrag = true;

        protected ItemInfo mData;
        public ItemInfo GetItemInfo() => mData;

        public virtual void SetData(ItemInfo data)
        {
            mData = data;
            RefreshVisual();
        }

        public void SetCanDrag(bool value) => canDrag = value;

        /// <summary>子类实现：根据 mData 显示/清空图标</summary>
        protected abstract void RefreshVisual();

        // ==================== 拖拽 ====================

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (mData == null || !canDrag) return;

            this.SendCommand(new BeginDragItemCommand(mData, mData.area));
            OnDragStartVisual();
        }

        public virtual void OnDrag(PointerEventData eventData) { }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            OnDragEndVisual();

            var target = DetectTarget(eventData);
            this.SendCommand(new EndDragItemCommand(target));
        }

        /// <summary>拖拽开始时的视觉反馈</summary>
        protected virtual void OnDragStartVisual() { }
        protected virtual void OnDragEndVisual() { }

        /// <summary>把 Unity 的射线结果转成抽象 DropTargetInfo，子类可重写以支持 QuestSlot 等</summary>
        protected virtual DropTargetInfo DetectTarget(PointerEventData eventData)
        {
            var hit = eventData.pointerCurrentRaycast.gameObject;
            if (hit == null) return default;

            var itemArea = hit.GetComponentInParent<ItemArea>();
            if (itemArea != null)
            {          
                return new DropTargetInfo
                {
                    IsValid = true,
                    TargetArea = itemArea.areaType,
                    AcceptType = itemArea.acceptType,
                };
            }

            return default;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (mData == null) return;
            this.SendEvent(new ShowItemInfoEvent { item = mData });
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this.SendEvent(new HideItemInfoEvent());
        }
    }
}
