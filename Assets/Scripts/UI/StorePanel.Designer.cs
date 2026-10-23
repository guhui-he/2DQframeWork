using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:06e35a0e-dcfb-4b75-83ac-470566500504
	public partial class StorePanel
	{
		public const string Name = "StorePanel";
		
		[SerializeField]
		public SlotListView BagContent;
		[SerializeField]
		public UnityEngine.UI.Image Image;
		[SerializeField]
		public SlotListView StoreContent;
		[SerializeField]
		public UnityEngine.UI.Button close;
		[SerializeField]
		public UnityEngine.UI.Text nameTxt;
		[SerializeField]
		public UnityEngine.UI.Text monetTxt;
		
		private StorePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BagContent = null;
			Image = null;
			StoreContent = null;
			close = null;
			nameTxt = null;
			monetTxt = null;
			
			mData = null;
		}
		
		public StorePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		StorePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new StorePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
