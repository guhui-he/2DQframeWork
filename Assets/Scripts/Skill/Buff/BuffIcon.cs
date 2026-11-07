using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
	public partial class BuffIcon : ViewController, IController,IPointerEnterHandler, IPointerExitHandler,ICanSendEvent

    {
		public IArchitecture GetArchitecture() => GameApp.Interface;
        private BuffInfo BuffInfo;

        public void Init(BuffInfo info)
        {
            //Tools.SetSprite(icon,info.config.Icon)
            BuffInfo = info;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            this.SendEvent<OpenBuffInfoPanelEvent>(new OpenBuffInfoPanelEvent(BuffInfo));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.SendEvent<HideBuffInfoPanelEvent>();
        }
	}
}
