using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:06381eb5-fd0d-4684-a0da-77990b31196c
	public partial class SkillPanel
	{
		public const string Name = "SkillPanel";
		
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.GameObject HeroIcon;
		[SerializeField]
		public Right Right;
		[SerializeField]
		public UnityEngine.UI.Text Nametxt;
		[SerializeField]
		public UnityEngine.UI.Text Infotxt;
		[SerializeField]
		public UnityEngine.UI.Text skillPointstxt;
		[SerializeField]
		public UnityEngine.UI.Button close;
		
		private SkillPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Content = null;
			HeroIcon = null;
			Right = null;
			Nametxt = null;
			Infotxt = null;
			skillPointstxt = null;
			close = null;
			
			mData = null;
		}
		
		public SkillPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		SkillPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new SkillPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
