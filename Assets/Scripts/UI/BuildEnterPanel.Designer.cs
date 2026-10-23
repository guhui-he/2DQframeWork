using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:4e33b3e3-0af2-47ba-8cbc-5664d6c5107e
	public partial class BuildEnterPanel
	{
		public const string Name = "BuildEnterPanel";
		
		[SerializeField]
		public UnityEngine.UI.Text nameTxt;
		[SerializeField]
		public UnityEngine.UI.Text desTxt;
		[SerializeField]
		public UnityEngine.UI.Button enterBtn;
		
		private BuildEnterPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			nameTxt = null;
			desTxt = null;
			enterBtn = null;
			
			mData = null;
		}
		
		public BuildEnterPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BuildEnterPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BuildEnterPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
