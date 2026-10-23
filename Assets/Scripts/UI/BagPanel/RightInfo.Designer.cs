/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class RightInfo
	{
		[SerializeField] public UnityEngine.UI.Button hpUp;
		[SerializeField] public UnityEngine.UI.Button atkUp;
		[SerializeField] public UnityEngine.UI.Button defUp;
		[SerializeField] public UnityEngine.UI.Button stepUp;
		[SerializeField] public UnityEngine.UI.Button critUp;
		[SerializeField] public UnityEngine.UI.Button avdUp;
		[SerializeField] public UnityEngine.UI.Button Up;
		[SerializeField] public UnityEngine.UI.Text hp;
		[SerializeField] public UnityEngine.UI.Text atk;
		[SerializeField] public UnityEngine.UI.Text def;
		[SerializeField] public UnityEngine.UI.Text step;
		[SerializeField] public UnityEngine.UI.Text crit;
		[SerializeField] public UnityEngine.UI.Text avd;
		[SerializeField] public UnityEngine.UI.Text txt;
		[SerializeField] public Tip Tip;

		public void Clear()
		{
			hpUp = null;
			atkUp = null;
			defUp = null;
			stepUp = null;
			critUp = null;
			avdUp = null;
			Up = null;
			hp = null;
			atk = null;
			def = null;
			step = null;
			crit = null;
			avd = null;
			txt = null;
			Tip = null;
		}

		public override string ComponentName
		{
			get { return "RightInfo";}
		}
	}
}
