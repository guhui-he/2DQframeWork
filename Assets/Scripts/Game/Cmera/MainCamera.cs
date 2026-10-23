using UnityEngine;
using QFramework;

namespace Game
{
    public partial class MainCamera : ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private ICameraModel mCameraModel;
        void Start()
        {
            mCameraModel = this.GetModel<ICameraModel>();
            mCameraModel.CurrentPosition.RegisterWithInitValue(curPos =>
            {
                this.transform.position = curPos;
            }).UnRegisterWhenGameObjectDestroyed(this);
        }
    }
}
