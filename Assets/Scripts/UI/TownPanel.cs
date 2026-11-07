using UnityEngine;
using QFramework;

namespace Game
{
	public class TownPanelData : UIPanelData
	{

	}
	public partial class TownPanel : TimePausingPanel, ICanSendEvent
	{
		private ICityConfigModel mBuildModel;
		IPlayerModel playerModel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as TownPanelData ?? new TownPanelData();
			mBuildModel=this.GetModel<ICityConfigModel>();
			playerModel = this.GetModel<IPlayerModel>();

			var buildModel = mBuildModel.GetCityConfig(playerModel.CurrentCityId.Value);

			nameTxt.text=buildModel.Name;
			BtnBack.onClick.AddListener(() =>
			{
                CloseSelf();
			});
			BtnStore.onClick.AddListener(() =>
			{
				this.SendEvent(new StoreEnterEvent());
			});
			BtnSmithy.onClick.AddListener(() => { });
			BtnSillyTavern.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<SillyTavernPanel>();
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			base.OnOpen(uiData);
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
