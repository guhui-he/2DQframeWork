/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class SKill
	{
		[SerializeField] public RectTransform Content;
		[SerializeField] public UnityEngine.GameObject SkillSlot;
		[SerializeField] public UnityEngine.UI.Button Idle;
		[SerializeField] public UnityEngine.UI.Button Reback;
		[SerializeField] public HeroDes HeroDes;
		[SerializeField] public RectTransform Buff;
		[SerializeField] public UnityEngine.GameObject BuffIcon;

		public void Clear()
		{
			Content = null;
			SkillSlot = null;
			Idle = null;
			Reback = null;
			HeroDes = null;
			Buff = null;
			BuffIcon = null;
		}

		public override string ComponentName
		{
			get { return "SKill";}
		}
	}
}
