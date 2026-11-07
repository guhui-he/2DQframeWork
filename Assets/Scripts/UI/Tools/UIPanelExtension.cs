using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public static class UIPanelExtension
    {
        /// <summary>
        /// 注册面板关闭回调（仅在面板关闭时触发一次）
        /// </summary>
        public static void OnPanelClosed(this UIPanel panel, Action onClosed)
        {
            if (panel == null) return;

            // 动态添加或获取监听组件
            var notifier = panel.gameObject.GetComponent<PanelCloseNotifier>();
            if (notifier == null)
            {
                notifier = panel.gameObject.AddComponent<PanelCloseNotifier>();
            }

            notifier.SetCallback(onClosed);
        }
    }

    /// <summary>
    /// 负责监听面板的 OnDisable 生命周期
    /// </summary>
    public class PanelCloseNotifier : MonoBehaviour
    {
        private Action mOnClosed;
        private bool mHasNotified = false;

        public void SetCallback(Action callback)
        {
            mOnClosed = callback;
            mHasNotified = false; // 重置状态，支持复用
        }

        private void OnDisable()
        {
            // 防止多次触发，确保只回调一次
            if (mHasNotified) return;
            mHasNotified = true;

            mOnClosed?.Invoke();
            mOnClosed = null; // 用完清空，防止下次打开误触发
        }
    }
}
