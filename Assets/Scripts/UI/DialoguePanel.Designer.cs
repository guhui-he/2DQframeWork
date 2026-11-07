using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:ed2b04e3-3bfd-490a-af7c-d6d4c17a7372
	public partial class DialoguePanel
	{
		public const string Name = "DialoguePanel";
		
		[SerializeField]
		public UnityEngine.UI.Text speakerTxt;
		[SerializeField]
		public UnityEngine.UI.Text contentTxt;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		[SerializeField]
		public RectTransform root;
		[SerializeField]
		public choicePrefab choicePrefab;
		
		private DialoguePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			speakerTxt = null;
			contentTxt = null;
			NextBtn = null;
			root = null;
			choicePrefab = null;
			
			mData = null;
		}
		
		public DialoguePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		DialoguePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new DialoguePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
