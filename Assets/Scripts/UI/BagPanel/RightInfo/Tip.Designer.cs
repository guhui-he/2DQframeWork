/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class Tip
	{
		[SerializeField] public UnityEngine.UI.Button yes;
		[SerializeField] public UnityEngine.UI.Button no;
		[SerializeField] public UnityEngine.UI.Text txt;

		public void Clear()
		{
			yes = null;
			no = null;
			txt = null;
		}

		public override string ComponentName
		{
			get { return "Tip";}
		}
	}
}
