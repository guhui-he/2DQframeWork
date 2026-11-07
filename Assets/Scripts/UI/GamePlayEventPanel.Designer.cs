using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:18e31791-1a4b-406e-bd26-48941bf6cfd0
	public partial class GamePlayEventPanel
	{
		public const string Name = "GamePlayEventPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image NpcIcon;
		[SerializeField]
		public UnityEngine.UI.Text DesTxt;
		[SerializeField]
		public RectTransform Root;
		[SerializeField]
		public UnityEngine.GameObject icon;
		[SerializeField]
		public UnityEngine.UI.Button Btn;
		
		private GamePlayEventPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NpcIcon = null;
			DesTxt = null;
			Root = null;
			icon = null;
			Btn = null;
			
			mData = null;
		}
		
		public GamePlayEventPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GamePlayEventPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GamePlayEventPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
