using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:46aed685-7920-4c7b-9040-8a17ad40a727
	public partial class BattlePanel
	{
		public const string Name = "BattlePanel";
		
		[SerializeField]
		public UnityEngine.UI.Text TurnTxt;
		[SerializeField]
		public UnityEngine.UI.Text HitNum;
		[SerializeField]
		public UnityEngine.UI.Button Setting;
		[SerializeField]
		public SKill SKill;
		
		private BattlePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TurnTxt = null;
			HitNum = null;
			Setting = null;
			SKill = null;
			
			mData = null;
		}
		
		public BattlePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BattlePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BattlePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
