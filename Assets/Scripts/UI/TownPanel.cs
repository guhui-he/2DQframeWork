using UnityEngine;
using QFramework;

namespace Game
{
	public class TownPanelData : UIPanelData
	{
		public int id;
	}
	public partial class TownPanel : TimePausingPanel, ICanSendEvent
	{
		private IBuildModel mBuildModel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as TownPanelData ?? new TownPanelData();
			mBuildModel=this.GetModel<IBuildModel>();
			var buildModel = mBuildModel.GetBuildData(mData.id);

			nameTxt.text=buildModel.Name;
			BtnBack.onClick.AddListener(() =>
			{
                CloseSelf();
			});
			BtnStore.onClick.AddListener(() =>
			{
				this.SendEvent(new StoreEnterEvent(mData.id));
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
