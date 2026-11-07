using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:ec5856e5-9b5b-4e98-b085-ac81bd4253ed
	public partial class BattleStartPanel
	{
		public const string Name = "BattleStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image bottom;
		[SerializeField]
		public RectTransform grid;
		[SerializeField]
		public UnityEngine.GameObject HeroIcon;
		[SerializeField]
		public UnityEngine.UI.Button startBtn;
		[SerializeField]
		public DragHeroPanel DragHeroPanel;
		[SerializeField]
		public UnityEngine.UI.Button Setting;
		
		private BattleStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			bottom = null;
			grid = null;
			HeroIcon = null;
			startBtn = null;
			DragHeroPanel = null;
			Setting = null;
			
			mData = null;
		}
		
		public BattleStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BattleStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BattleStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
