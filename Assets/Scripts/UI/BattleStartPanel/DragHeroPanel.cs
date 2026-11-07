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

        private void Awake()
        {

        }
        private void OnEnable()
        {

        }
        private void Update()
        {
            var panelRect = transform as RectTransform;
            var canvasRect = panelRect.parent as RectTransform;
            Vector2 localPos = canvasRect.GetLocalPosInRect();
            panelRect.anchoredPosition=localPos;
        }
        protected override void OnBeforeDestroy()
        {
        }
    }
}
