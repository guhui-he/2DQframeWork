using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:a9a47c15-1d63-41e5-a2fc-7f9ba5b17ca0
	public partial class SillyTavernPanel
	{
		public const string Name = "SillyTavernPanel";
		
		[SerializeField]
		public Content Content;
		[SerializeField]
		public HeroInfo HeroInfo;
		[SerializeField]
		public UnityEngine.UI.Button Btnclose;
		[SerializeField]
		public UnityEngine.UI.Text moneyTxt;
		[SerializeField]
		public UnityEngine.UI.Button BtnBuyHero;
		[SerializeField]
		public UnityEngine.UI.Text needMoneyTxt;
		
		private SillyTavernPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Content = null;
			HeroInfo = null;
			Btnclose = null;
			moneyTxt = null;
			BtnBuyHero = null;
			needMoneyTxt = null;
			
			mData = null;
		}
		
		public SillyTavernPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		SillyTavernPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new SillyTavernPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
