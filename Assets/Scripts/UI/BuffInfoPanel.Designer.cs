using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:ff8c11b7-7a30-444a-bd02-56f5b7b220be
	public partial class BuffInfoPanel
	{
		public const string Name = "BuffInfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text NameTxt;
		[SerializeField]
		public UnityEngine.UI.Text DurationTxt;
		[SerializeField]
		public UnityEngine.UI.Text InfoTxt;
		
		private BuffInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NameTxt = null;
			DurationTxt = null;
			InfoTxt = null;
			
			mData = null;
		}
		
		public BuffInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BuffInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BuffInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
