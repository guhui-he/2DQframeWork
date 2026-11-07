using System;
using QFramework;

namespace Game
{

    public struct OpenPanelEvent
    {
        public UIPanelId PanelId;      // 对应配置表
        public UIParams Params;  // 快递盒
        public Action OnClosed;  // 关闭回调
    }

    public struct ClosePanelEvent
    {
        public UIPanelId PanelId;
    }
}
