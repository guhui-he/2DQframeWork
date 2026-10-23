/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class DragHeroPanel : UIElement
	{
        public int curHeroid;
        public RectTransform parentRect;

        private RectTransform mRectTransform;
        private void Awake()
        {
            mRectTransform = GetComponent<RectTransform>();
            if (parentRect == null)
            {
                // 如果没有手动指定，默认使用父级的RectTransform
                parentRect = mRectTransform.parent as RectTransform;
            }
        }
        private void OnEnable()
        {

        }
        private void Update()
        {
            Vector2 screenPoint = Input.mousePosition;

            // 2. 将屏幕坐标转换为父RectTransform的本地坐标
            //    对于Overlay模式的Canvas，摄像机参数传null即可
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPoint,
                null, // 如果Canvas的Render Mode是Screen Space - Camera或World Space，这里需要传入对应的摄像机
                out localPoint))
            {
                // 3. 赋值给Panel的anchoredPosition
                mRectTransform.anchoredPosition = localPoint;
            }
        }
        protected override void OnBeforeDestroy()
        {
        }
    }
}
