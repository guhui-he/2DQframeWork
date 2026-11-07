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
		[SerializeField] public RectTransform Hp;
		[SerializeField] public UnityEngine.UI.Text hp;
		[SerializeField] public RectTransform Atk;
		[SerializeField] public UnityEngine.UI.Text atk;
		[SerializeField] public RectTransform Def;
		[SerializeField] public UnityEngine.UI.Text def;
		[SerializeField] public RectTransform Step;
		[SerializeField] public UnityEngine.UI.Text step;
		[SerializeField] public RectTransform Crit;
		[SerializeField] public UnityEngine.UI.Text crit;
		[SerializeField] public RectTransform Avd;
		[SerializeField] public UnityEngine.UI.Text avd;
		[SerializeField] public UnityEngine.UI.Text txt;
		[SerializeField] public Tip Tip;
		[SerializeField] public UnityEngine.GameObject star;

		public void Clear()
		{
			hpUp = null;
			atkUp = null;
			defUp = null;
			stepUp = null;
			critUp = null;
			avdUp = null;
			Up = null;
			Hp = null;
			hp = null;
			Atk = null;
			atk = null;
			Def = null;
			def = null;
			Step = null;
			step = null;
			Crit = null;
			crit = null;
			Avd = null;
			avd = null;
			txt = null;
			Tip = null;
			star = null;
		}

		public override string ComponentName
		{
			get { return "RightInfo";}
		}
	}
}
