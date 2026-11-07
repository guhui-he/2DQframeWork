using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class GamePlayEventPanelData : UIPanelData
	{
		public EventDisPlayData data;
    }
	public partial class GamePlayEventPanel : BaseUIPanel,ICanSendEvent
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GamePlayEventPanelData ?? new GamePlayEventPanelData();
            // please add init code here
            Btn.onClick.AddListener(() => this.SendEvent<HideEventPanelEvent>());
        }
        protected override void OnOpenWithParams(UIParams param)
        {
            EventDisPlayData shopId = param.ExtraData as EventDisPlayData;

			Debug.Log($"【商店面板】收到命令打开商店，ID: {shopId.Description}");
            // 接下来加载商品列表...
        }		
		protected override void OnClose()
		{
		}
    }
}
