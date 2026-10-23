using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:322ab569-05aa-43d4-8bbf-333b841d1f1f
	public partial class BattleStartPanel
	{
		public const string Name = "BattleStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text TurnTxt;
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
		public UnityEngine.UI.Text HitNum;
		[SerializeField]
		public UnityEngine.UI.Button Setting;
		
		private BattleStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TurnTxt = null;
			bottom = null;
			grid = null;
			HeroIcon = null;
			startBtn = null;
			DragHeroPanel = null;
			HitNum = null;
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
