using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:d29da769-9106-49da-9fb0-3bba8e3ee7d2
	public partial class GamePanel
	{
		public const string Name = "GamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button close;
		[SerializeField]
		public UnityEngine.UI.Button save;
		[SerializeField]
		public UnityEngine.UI.Button hero;
		[SerializeField]
		public UnityEngine.UI.Button bag;
		[SerializeField]
		public UnityEngine.UI.Button quest;
		[SerializeField]
		public UnityEngine.UI.Text Money;
		
		private GamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			close = null;
			save = null;
			hero = null;
			bag = null;
			quest = null;
			Money = null;
			
			mData = null;
		}
		
		public GamePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GamePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GamePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
