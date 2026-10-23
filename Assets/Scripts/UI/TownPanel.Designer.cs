using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:3c846342-ba1b-4521-b30b-ce9ab58108f8
	public partial class TownPanel
	{
		public const string Name = "TownPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnStore;
		[SerializeField]
		public UnityEngine.UI.Button BtnSillyTavern;
		[SerializeField]
		public UnityEngine.UI.Button BtnSmithy;
		[SerializeField]
		public UnityEngine.UI.Text nameTxt;
		[SerializeField]
		public UnityEngine.UI.Button BtnBack;
		
		private TownPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnStore = null;
			BtnSillyTavern = null;
			BtnSmithy = null;
			nameTxt = null;
			BtnBack = null;
			
			mData = null;
		}
		
		public TownPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		TownPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new TownPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
