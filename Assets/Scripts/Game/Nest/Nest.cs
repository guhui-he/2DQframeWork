using QFramework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public partial class Nest : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        [SerializeField]
        private int nestId;

        public void Init(SettlementInfo data)
        {            
           nestId=data.BuildId;
        }
        void Start()
        {

        }
    }
}
