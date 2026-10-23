/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class EquipArea
	{
		[SerializeField] public Game.EquipmentItem weapon;
		[SerializeField] public Game.EquipmentItem shield;
		[SerializeField] public Game.EquipmentItem boot;
		[SerializeField] public Game.EquipmentItem armor;
		[SerializeField] public Game.EquipmentItem helmet;
		[SerializeField] public UnityEngine.GameObject hero2;
		[SerializeField] public UnityEngine.GameObject pig_M;
		[SerializeField] public UnityEngine.SpriteRenderer body;
		[SerializeField] public UnityEngine.GameObject emofashi_M;

		public void Clear()
		{
			weapon = null;
			shield = null;
			boot = null;
			armor = null;
			helmet = null;
			hero2 = null;
			pig_M = null;
			body = null;
			emofashi_M = null;
		}

		public override string ComponentName
		{
			get { return "EquipArea";}
		}
	}
}
