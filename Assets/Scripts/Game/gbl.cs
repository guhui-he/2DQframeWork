using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
	public partial class gbl : ViewController,IController,ICanSendEvent, IPointerClickHandler
    {
        public int id;
        public IArchitecture GetArchitecture() => GameApp.Interface;
        IPlayerModel playerModel;
        void Start()
		{
            playerModel=this.GetModel<IPlayerModel>();
        }
        private void Update()
        {
           if (Vector3.Distance(this.transform.position,playerModel.Position.Value)<2f)
            {
                tip.gameObject.SetActive(true);
                UIKit.OpenPanel<GamePanel>();
            }
            else
                tip.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (tip.gameObject.activeSelf)
            {
                this.SendCommand(new DialogueCommand(id));
            }
        }
    }
}
