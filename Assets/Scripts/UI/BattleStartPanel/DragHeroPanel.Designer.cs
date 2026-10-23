/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class DragHeroPanel
	{
		[SerializeField] public UnityEngine.UI.Image icon;

		public void Clear()
		{
			icon = null;
		}

		public override string ComponentName
		{
			get { return "DragHeroPanel";}
		}
	}
}
