/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class Content
	{
		[SerializeField] public UnityEngine.GameObject HeroIcon;

		public void Clear()
		{
			HeroIcon = null;
		}

		public override string ComponentName
		{
			get { return "Content";}
		}
	}
}
