using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:5bf53046-431b-433f-a484-db3a5aca1872
	public partial class HeroDesPanel
	{
		public const string Name = "HeroDesPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image icon;
		[SerializeField]
		public UnityEngine.UI.Image fill;
		[SerializeField]
		public UnityEngine.UI.Text curTxt;
		[SerializeField]
		public UnityEngine.UI.Text atk;
		[SerializeField]
		public UnityEngine.UI.Text def;
		[SerializeField]
		public UnityEngine.UI.Text crit;
		[SerializeField]
		public UnityEngine.UI.Text avd;
		
		private HeroDesPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			icon = null;
			fill = null;
			curTxt = null;
			atk = null;
			def = null;
			crit = null;
			avd = null;
			
			mData = null;
		}
		
		public HeroDesPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		HeroDesPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new HeroDesPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
