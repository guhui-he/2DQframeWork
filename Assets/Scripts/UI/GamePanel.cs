using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class GamePanelData : UIPanelData
	{
	}
	public partial class GamePanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GamePanelData ?? new GamePanelData();

			bag.onClick.AddListener(() =>
			{
				UIKit.OpenPanel<BagPanel>();
			});
			save.onClick.AddListener(() =>
			{
				this.GetSystem<ISaveSystem>().SaveGame();
			});
			quest.onClick.AddListener(() =>
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
