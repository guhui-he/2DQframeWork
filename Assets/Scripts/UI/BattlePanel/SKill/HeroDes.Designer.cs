/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class HeroDes
	{
		[SerializeField] public UnityEngine.UI.Image icon;
		[SerializeField] public UnityEngine.UI.Image fill;
		[SerializeField] public UnityEngine.UI.Text curTxt;
		[SerializeField] public UnityEngine.UI.Text atk;
		[SerializeField] public UnityEngine.UI.Text def;
		[SerializeField] public UnityEngine.UI.Text crit;
		[SerializeField] public UnityEngine.UI.Text avd;

		public void Clear()
		{
			icon = null;
			fill = null;
			curTxt = null;
			atk = null;
			def = null;
			crit = null;
			avd = null;
		}

		public override string ComponentName
		{
			get { return "HeroDes";}
		}
	}
}
