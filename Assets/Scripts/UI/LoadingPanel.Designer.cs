using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:f50cd22b-d5a5-4056-8230-78f09e1a4744
	public partial class LoadingPanel
	{
		public const string Name = "LoadingPanel";
		
		
		private LoadingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public LoadingPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		LoadingPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new LoadingPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
