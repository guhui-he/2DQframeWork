using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;

namespace Game
{
	public class BattleStartPanelData : UIPanelData
	{
	}
	public partial class BattleStartPanel : UIPanel,IController
	{
        private IHeroModel mHeroModel;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as BattleStartPanelData ?? new BattleStartPanelData();
            mHeroModel = this.GetModel<IHeroModel>();
            startBtn.onClick.AddListener(() =>
            {
                this.SendCommand(new StartGameCommand()); 
                CloseSelf();
                UIKit.OpenPanel<BattlePanel>();
            });
            Setting.onClick.AddListener(() => { });
           
            this.RegisterEvent<BattleBeginDragEvent>(e =>
            {
                DragHeroPanel.gameObject.SetActive(true);
                DragHeroPanel.curHeroid = e.id;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<BattleEndDragEvent>(e =>
            {
                DragHeroPanel.gameObject.SetActive(false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            foreach (var hero in mHeroModel.mHero)
            {
                var obj = Instantiate(HeroIcon, grid);
                obj.SetActive(true);
                obj.GetComponent<HeroIcon>().SetIcon(hero.Value);
            }
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
