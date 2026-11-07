using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;

namespace Game
{
	public class BattlePanelData : UIPanelData
	{
	}
	public partial class BattlePanel : UIPanel,IController
	{
		IBattleModel model;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BattlePanelData ?? new BattlePanelData();
			model = this.GetModel<IBattleModel>();

			model.SelectedUnit.Register(unit =>
			{
				if (unit != null)
				{
					SKill.gameObject.SetActive(true);
					SKill.Refresh(unit);
				}
				else
				{
					SKill.gameObject.SetActive(false);
				}
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
            model.TurnCount.RegisterWithInitValue(e =>
            {
                TurnTxt.text = $"µÚ{e}»ØºÏ";
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitDamagedEvent>(e =>
            {
                var unit = model.AllUnits.FirstOrDefault(u => u.UnitId == e.TargetId);
                if (unit == null) return;
                if (!BlockViewRegistry.TryGetWorldPosition(unit.Pos, out var wp)) return;
                Vector3 baseScreenPos = Camera.main.WorldToScreenPoint(wp) + Vector3.up * 80;
                for (int i = 0; i < e.HitCount; i++)
                {
                    int hitIndex = i; 
                    int currentDamage = e.Damage; 
                    ActionKit.Delay(hitIndex * 0.15f, () =>
                    {
                        HitNum.text = currentDamage.ToString(); 
                        var obj = Instantiate(HitNum, this.transform);
                        obj.gameObject.SetActive(true);
                        float randomX = Random.Range(-0.4f, 0.4f);
                        obj.transform.position = baseScreenPos + new Vector3(randomX, 0, 0);

                    }).Start(this);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
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
