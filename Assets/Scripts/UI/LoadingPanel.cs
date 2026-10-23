using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class LoadingPanelData : UIPanelData
	{
	}
	public partial class LoadingPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as LoadingPanelData ?? new LoadingPanelData();
			// please add init code here
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
	}
}
