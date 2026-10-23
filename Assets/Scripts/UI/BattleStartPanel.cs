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
        private IBattleModel model;
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as BattleStartPanelData ?? new BattleStartPanelData();
            mHeroModel = this.GetModel<IHeroModel>();
            model = this.GetModel<IBattleModel>();
            startBtn.onClick.AddListener(() =>
            {
                this.SendCommand(new StartGameCommand());
                bottom.gameObject.SetActive(false);
            });
            Setting.onClick.AddListener(() => { });
            model.TurnCount.RegisterWithInitValue(e =>
            {
                TurnTxt.text = $"µÚ{e}»ØºÏ";
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<BattleBeginDragEvent>(e =>
            {
                DragHeroPanel.gameObject.SetActive(true);
                DragHeroPanel.curHeroid = e.id;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<BattleEndDragEvent>(e =>
            {
                DragHeroPanel.gameObject.SetActive(false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<UnitDamagedEvent>(e =>
            {
                var unit = model.AllUnits.FirstOrDefault(u => u.UnitId == e.TargetId);
                if (unit == null) return;
                if (BlockViewRegistry.TryGetWorldPosition(unit.Pos, out var wp))
                {
                    HitNum.text = e.Damage.ToString();
                    var obj = Instantiate(HitNum, this.transform);
                    obj.gameObject.SetActive(true);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(wp) + Vector3.up * 80;
                    obj.transform.position = screenPos;
                }
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
