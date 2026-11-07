/****************************************************************************
 * 2026.11 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class choicePrefab
	{
		[SerializeField] public UnityEngine.UI.Text txt;
		[SerializeField] public UnityEngine.UI.Button btn;

		public void Clear()
		{
			txt = null;
			btn = null;
		}

		public override string ComponentName
		{
			get { return "choicePrefab";}
		}
	}
}
