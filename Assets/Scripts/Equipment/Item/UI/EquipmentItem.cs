using UnityEngine;
using QFramework;

namespace Game
{
    public partial class EquipmentItem : SlotView, IController
    {
        [Header("槽位配置")]
        [SerializeField] private ItemType slotType;

        private IHeroModel mHeroModel;
        private IUnRegister mSlotUnregister;   // ← 手动管理槽位订阅

        private void Start()
        {
            mHeroModel = this.GetModel<IHeroModel>();

            // 只订阅"当前英雄变化"
            mHeroModel.curHeroId
                .RegisterWithInitValue(OnHeroChanged)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnHeroChanged(int heroId)
        {
            mSlotUnregister?.UnRegister();
            mSlotUnregister = null;

            var hero = mHeroModel.GetHero(heroId);
            if (hero == null)
            {
                SetData(null);
                return;
            }

            mSlotUnregister = hero.Equipment.Slots[slotType]
                .RegisterWithInitValue(item => SetData(item));
        }

        protected  void OnDestroy()
        {
            mSlotUnregister?.UnRegister();
            mSlotUnregister = null;
        }

        protected override void RefreshVisual()
        {
            if (mData == null)
            {
                icon.SetAlpha(0f);
                icon.raycastTarget = false;
            }
            else
            {
                Tools.SetSprite(icon, mData.itemData.Type, mData.itemData.Icon);
                icon.SetAlpha(1f);
                icon.raycastTarget = true;
            }
        }
    }
}

