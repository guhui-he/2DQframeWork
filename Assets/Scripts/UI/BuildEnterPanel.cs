
using QFramework;

namespace Game
{
	public class BuildEnterPanelData : UIPanelData
	{
		public int id;
	}
	public partial class BuildEnterPanel : UIPanel,IController,ICanSendEvent
	{
		private IBuildModel mBuildModel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BuildEnterPanelData ?? new BuildEnterPanelData();
			mBuildModel=this.GetModel<IBuildModel>();
			var buildModel = mBuildModel.GetBuildData(mData.id);
			nameTxt.text = buildModel.Name;
			desTxt.text = buildModel.Des;
			enterBtn.onClick.AddListener(() =>
			{
                this.SendCommand(new EnterTownCommand(mData.id));
                //CloseSelf();
            });
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public IArchitecture GetArchitecture() => GameApp.Interface;

    }
}
