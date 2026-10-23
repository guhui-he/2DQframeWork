using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Unity.VisualScripting;

namespace Game
{
	public class BagPanelData : UIPanelData
	{
	}
	public partial class BagPanel : TimePausingPanel
    {
		private IBagModel mBagModel;
        IHeroModel mHeroModel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BagPanelData ?? new BagPanelData();
			mBagModel = this.GetModel<IBagModel>();
            mHeroModel = this.GetModel<IHeroModel>();
            close.onClick.AddListener(() =>
            {
                CloseSelf();
            });
            
            var mBindList = mBagModel.GetBindableList();
            mBindList.OnAdd.Register((index, item) =>
            {
                Content.Append(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnRemove.Register((index, item) =>
            {
                Content.Remove(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnClear.Register(() =>
            {
                Content.Clear();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            mHeroModel.curHeroId.RegisterWithInitValue(e =>
            {
                var hero=mHeroModel.GetHero(e);
                EquipArea.Refresh(hero);
                LeftInfo.RefreshInfo(hero);
                RightInfo.RefreshInfo(hero);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            base .OnOpen(uiData);
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
        protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            base.OnClose();
		}
    }
}
