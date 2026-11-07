
using QFramework;
using UnityEngine;

namespace Game
{
	public class ItemInfoPanelData : UIPanelData
	{
		public ItemInfo item;
	}
	public partial class ItemInfoPanel : UIPanel,IController
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ItemInfoPanelData ?? new ItemInfoPanelData();
			
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			var data=mData.item;
			NameTxt.text = data.itemData.Name;
			PriceTxt.text=data.itemData.Price.ToString();
			InfoTxt.text=data.GetAffixDescription();

			var panelRect = transform as RectTransform;
            var canvasRect = panelRect.parent as RectTransform;
            Vector2 localPos = canvasRect.GetLocalPosInRect();
            panelRect.anchoredPosition = localPos;
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
