using QFramework;
using UnityEngine;

namespace Game
{
    // 所有游戏面板的基类
    public abstract class BaseUIPanel : UIPanel, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
            if (uiData is GlobalUIData globalData)
            {
                OnOpenWithParams(globalData.Params);
            }
            else
            {
                OnOpenWithParams(default);
            }
        }
        protected abstract void OnOpenWithParams(UIParams param);
    }
}
