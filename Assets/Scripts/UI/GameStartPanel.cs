using QFramework;

namespace Game
{
	public class GameStartPanelData : UIPanelData
	{
	}
	public partial class GameStartPanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GameStartPanelData ?? new GameStartPanelData();
			startBtn.onClick.AddListener(() =>
			{
				GameEnter(false);

            });
			contiuneBtn.onClick.AddListener(() =>
			{
				GameEnter(true);
            });
		}
		void GameEnter(bool load)
		{
            this.SendCommand(new GameStartCommand("Game",load));
            CloseSelf();
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

        public IArchitecture GetArchitecture()
        {
			return GameApp.Interface;
        }
    }
}
