using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:d0fc2350-aff9-4bfd-b016-71bab4e1183d
	public partial class BagPanel
	{
		public const string Name = "BagPanel";
		
		[SerializeField]
		public EquipArea EquipArea;
		[SerializeField]
		public Game.SlotListView Content;
		[SerializeField]
		public UnityEngine.UI.Image Image;
		[SerializeField]
		public HeroConent HeroConent;
		[SerializeField]
		public LeftInfo LeftInfo;
		[SerializeField]
		public RightInfo RightInfo;
		[SerializeField]
		public UnityEngine.UI.Button close;
		
		private BagPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			EquipArea = null;
			Content = null;
			Image = null;
			HeroConent = null;
			LeftInfo = null;
			RightInfo = null;
			close = null;
			
			mData = null;
		}
		
		public BagPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BagPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BagPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
