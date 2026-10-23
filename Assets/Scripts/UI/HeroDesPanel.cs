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

			// 3. 监听自动刷新（掉血、死亡等）
			this.RegisterEvent<UnitDamagedEvent>(e =>
			{
				if (e.TargetId == mData.data.UnitId)
					RefreshUI(); // 如果是自己掉血，刷新
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitDieEvent>(e =>
            {
                if (e.UnitId == mData.data.UnitId)
                    CloseSelf(); // 如果自己死了，关掉面板
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        private void RefreshUI()
        {
            Tools.SetSprite(icon, AssetPathDef.AtlasRole, mData.data.SourceInfo.Icon);
			fill.fillAmount=(float)mData.data.Hp/mData.data.SourceInfo.MaxHp;
			curTxt.text = (float)mData.data.Hp + "/" + mData.data.SourceInfo.MaxHp;
			atk.text=mData.data.SourceInfo.Atk.ToString();
			def.text= mData.data.SourceInfo.Def.ToString();
            crit.text = mData.data.SourceInfo.Crit.ToString();
            avd.text = mData.data.SourceInfo.Avd.ToString();

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
