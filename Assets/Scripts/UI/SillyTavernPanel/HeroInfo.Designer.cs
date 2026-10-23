/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class HeroInfo
	{
		[SerializeField] public UnityEngine.UI.Text nameTxt;
		[SerializeField] public UnityEngine.UI.Text levelTxt;
		[SerializeField] public UnityEngine.UI.Slider Hp;
		[SerializeField] public UnityEngine.UI.Text curHp;
		[SerializeField] public UnityEngine.UI.Slider Exp;
		[SerializeField] public UnityEngine.UI.Text curExp;
		[SerializeField] public UnityEngine.UI.Text hp;
		[SerializeField] public UnityEngine.UI.Text atk;
		[SerializeField] public UnityEngine.UI.Text def;
		[SerializeField] public UnityEngine.UI.Text step;
		[SerializeField] public UnityEngine.UI.Text crit;
		[SerializeField] public UnityEngine.UI.Text avd;
		[SerializeField] public UnityEngine.UI.Text txt;

		public void Clear()
		{
			nameTxt = null;
			levelTxt = null;
			Hp = null;
			curHp = null;
			Exp = null;
			curExp = null;
			hp = null;
			atk = null;
			def = null;
			step = null;
			crit = null;
			avd = null;
			txt = null;
		}

		public override string ComponentName
		{
			get { return "HeroInfo";}
		}
	}
}
