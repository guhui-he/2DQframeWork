using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class SillyTavernPanelData : UIPanelData
	{
	}
	public partial class SillyTavernPanel : UIPanel,IController
	{
		ISillyTavernModel model;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as SillyTavernPanelData ?? new SillyTavernPanelData();
			model=this.GetModel<ISillyTavernModel>();
			Btnclose.onClick.AddListener(() =>
			{
				CloseSelf();
			});

			BtnBuyHero.onClick.AddListener(() =>
			{
				this.SendCommand(new BuyHeroCommand());
				Content.RefreshHero();
				if(model.CurHeros.Count==0)
				{
                    HeroInfo.gameObject.SetActive(false);
					BtnBuyHero.gameObject.SetActive(false);
                }
					
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			Content.RefreshHero();
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
