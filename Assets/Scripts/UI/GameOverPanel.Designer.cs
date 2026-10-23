using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:2b4c2fb9-2a9d-4ca2-9ddc-8ae605b15376
	public partial class GameOverPanel
	{
		public const string Name = "GameOverPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text txt;
		[SerializeField]
		public UnityEngine.UI.Button okBtn;
		[SerializeField]
		public Game.SlotListView BgContent;
		[SerializeField]
		public UnityEngine.UI.Image Image;
		[SerializeField]
		public Game.SlotListView LootContent;
		
		private GameOverPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			txt = null;
			okBtn = null;
			BgContent = null;
			Image = null;
			LootContent = null;
			
			mData = null;
		}
		
		public GameOverPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		GameOverPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new GameOverPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
