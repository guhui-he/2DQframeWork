using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:bf0fe624-2430-4fa6-9d6c-fd4c68170e53
	public partial class ActionMenuPanel
	{
		public const string Name = "ActionMenuPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button Attack;
		[SerializeField]
		public UnityEngine.UI.Button Idle;
		[SerializeField]
		public UnityEngine.UI.Button Reback;
		
		private ActionMenuPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Attack = null;
			Idle = null;
			Reback = null;
			
			mData = null;
		}
		
		public ActionMenuPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ActionMenuPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ActionMenuPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
