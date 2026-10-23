
using UnityEngine;

namespace Game
{
    public partial class Item : SlotView
    {
        protected override void RefreshVisual()
        {
            if (icon == null) return;

            if (mData == null)
            {
                BgImage.color = Color.white;
                icon.SetAlpha(0f);
                icon.raycastTarget = false;
            }
            else
            {
                BgImage.color = Tools.GetLevelColor(mData.itemData.Level);
                Tools.SetSprite(icon,mData.itemData.Type, mData.itemData.Icon);
                icon.SetAlpha(1f);
                icon.raycastTarget = true;
            }
        }

        protected override void OnDragStartVisual()
        {
            if (icon != null) icon.SetAlpha(0.3f);
        }

        protected override void OnDragEndVisual()
        {
            if (icon != null) icon.SetAlpha(mData == null ? 0f : 1f);
        }
    }
}
