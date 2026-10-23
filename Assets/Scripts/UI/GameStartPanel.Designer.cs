using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:611c4db8-0a51-4db6-87ac-7acf31771f37
	public partial class GameStartPanel
	{
		public const string Name = "GameStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button startBtn;
		[SerializeField]
		public UnityEngine.UI.Button contiuneBtn;
		[SerializeField]
		public UnityEngine.UI.Button setBtn;
		[SerializeField]
		public UnityEngine.UI.Button quitBtn;
		
		private GameStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			startBtn = null;
			contiuneBtn = null;
			setBtn = null;
			quitBtn = null;
			
			mData = null;
		}
		
		public GameStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GameStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GameStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
