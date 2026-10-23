using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:367c53ef-11c1-4dfd-bdcf-3652df781175
	public partial class ItemInfoPanel
	{
		public const string Name = "ItemInfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text NameTxt;
		[SerializeField]
		public UnityEngine.UI.Text PriceTxt;
		[SerializeField]
		public UnityEngine.UI.Text InfoTxt;
		
		private ItemInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NameTxt = null;
			PriceTxt = null;
			InfoTxt = null;
			
			mData = null;
		}
		
		public ItemInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ItemInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ItemInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
