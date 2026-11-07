using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Game
{
	public class HeroDesPanelData : UIPanelData
	{
		public BattleUnitData data;
	}
	public partial class HeroDesPanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as HeroDesPanelData ?? new HeroDesPanelData();
			// please add init code here
			
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			
            RefreshUI();
			this.RegisterEvent<UnitDamagedEvent>(e =>
			{
				if (e.TargetId == mData.data.UnitId)
					RefreshUI(); 
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        private void RefreshUI()
        {
			var role = mData.data.SourceInfo;
            Tools.SetSprite(icon, AssetPathDef.AtlasRole,role.Icon);
			fill.fillAmount = (float)mData.data.Hp / role.GetFinalStat().MaxHp;
			curTxt.text = mData.data.Hp + "/" + role.GetFinalStat().MaxHp;
			atk.text = role.GetFinalStat().Attack.ToString();
			def.text=role.GetFinalStat().Defense.ToString();
			crit.text=role.GetFinalStat().Crit.ToString();
			avd.text=role.GetFinalStat().Avd.ToString();
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
