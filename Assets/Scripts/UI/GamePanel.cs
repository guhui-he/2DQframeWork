using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class GamePanelData : UIPanelData
	{
	}
	public partial class GamePanel : UIPanel,IController,ICanSendEvent
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GamePanelData ?? new GamePanelData();
			hero.onClick.AddListener(() =>
			{
				this.SendEvent(new OpenPanelEvent { PanelId = UIPanelId.Skill });
			});
			bag.onClick.AddListener(() =>
			{
                this.SendEvent(new OpenPanelEvent
                {
                    PanelId =UIPanelId.Bag, 
                });
            });
			save.onClick.AddListener(() =>
			{
				this.GetSystem<ISaveSystem>().SaveGame();
			});
			close.onClick.AddListener(() =>
			{
				CloseSelf();
                this.GetSystem<ILoadingSystem>().LoadSceneAsync("GameStart");
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
