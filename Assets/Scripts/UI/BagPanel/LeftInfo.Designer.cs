/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class LeftInfo
	{
		[SerializeField] public UnityEngine.UI.Text nameTxt;
		[SerializeField] public UnityEngine.UI.Text level;
		[SerializeField] public UnityEngine.UI.Slider Hp;
		[SerializeField] public UnityEngine.UI.Text curHp;
		[SerializeField] public UnityEngine.UI.Slider Exp;
		[SerializeField] public UnityEngine.UI.Text curExp;

		public void Clear()
		{
			nameTxt = null;
			level = null;
			Hp = null;
			curHp = null;
			Exp = null;
			curExp = null;
		}

		public override string ComponentName
		{
			get { return "LeftInfo";}
		}
	}
}
