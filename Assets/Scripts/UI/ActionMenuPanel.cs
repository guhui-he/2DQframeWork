using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class ActionMenuPanelData : UIPanelData
	{
	}
	public partial class ActionMenuPanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ActionMenuPanelData ?? new ActionMenuPanelData();
			// please add init code here
			Attack.onClick.AddListener(() =>
			{
                CloseSelf();
                this.SendCommand(new EnterAttackCommand());

			});
			Idle.onClick.AddListener(() =>
			{
                CloseSelf();
                this.SendCommand(new StandbyCommand());
			});
			Reback.onClick.AddListener(() =>
			{
                CloseSelf();
                this.SendCommand(new CancelActionCommand());
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
