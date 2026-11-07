using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class BagPanelData : UIPanelData
	{
	}
    public partial class BagPanel : TimePausingPanel
    {
        private IBagModel mBagModel;
        IHeroModel mHeroModel;
        private IUnRegister mHeroSwitchUnregister; // π‹¿Ì”¢–€«–ªªº‡Ã˝

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as BagPanelData ?? new BagPanelData();
            mBagModel = this.GetModel<IBagModel>();
            mHeroModel = this.GetModel<IHeroModel>();
            close.onClick.AddListener(() => CloseSelf());

            var mBindList = mBagModel.GetBindableList();
            mBindList.OnAdd.Register((index, item) => Content.Append(item)).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnRemove.Register((index, item) => Content.Remove(item)).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnClear.Register(() => Content.Clear()).UnRegisterWhenGameObjectDestroyed(gameObject);

            // º‡Ã˝µ±«∞”¢–€±‰ªØ
            mHeroSwitchUnregister = mHeroModel.curHeroId.RegisterWithInitValue(e =>
            {
                var hero = mHeroModel.GetHero(e);
                if (hero != null)
                {
                    EquipArea.Refresh(hero);
                    LeftInfo.SwitchHero(hero);   
                    RightInfo.SwitchHero(hero);  
                    RightInfo.RefreshTalent(hero);
                }
            });
            this.RegisterEvent<EquipmentChangedEvent>(e =>
            {
                var hero = mHeroModel.GetHero(mHeroModel.curHeroId.Value);
                if (hero != null)
                {
                    LeftInfo.RefreshInfo(hero);
                    RightInfo.RefreshInfo(hero);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
            RefreshAll();
        }

        private void RefreshAll()
        {
            Content.Clear();
            for (int i = 0; i < mBagModel.Items.Count; i++)
            {
                Content.Append(mBagModel.Items[i]);
            }
            HeroConent.RefreshHero();
        }

        protected override void OnBeforeDestroy()
        {
            mHeroSwitchUnregister?.UnRegister();
            mHeroSwitchUnregister = null;
            base.OnBeforeDestroy();
        }
    }
}
