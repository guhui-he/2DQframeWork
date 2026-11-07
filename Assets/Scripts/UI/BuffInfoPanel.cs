using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class BuffInfoPanelData : UIPanelData
	{
		public BuffInfo info;
	}
	public partial class BuffInfoPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BuffInfoPanelData ?? new BuffInfoPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			NameTxt.text=mData.info.config.Name;
			DurationTxt.text=mData.info.RemainingTurns.ToString();
			InfoTxt.text=mData.info.config.Des;

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
	}
}
