/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class Right
	{
		[SerializeField] public Game.SkillPrefab Atk;
		[SerializeField] public Game.SkillPrefab FireballSpell;
		[SerializeField] public Game.SkillPrefab TreatmentTechnique;
		[SerializeField] public Game.SkillPrefab FrostNova;
		[SerializeField] public Game.SkillPrefab Charge;
		[SerializeField] public Game.SkillPrefab SummonSkeleton;
		[SerializeField] public Game.SkillPrefab Frenzied;
		[SerializeField] public Game.SkillPrefab Killerblow;
		[SerializeField] public Game.SkillPrefab Firewall;

		public void Clear()
		{
			Atk = null;
			FireballSpell = null;
			TreatmentTechnique = null;
			FrostNova = null;
			Charge = null;
			SummonSkeleton = null;
			Frenzied = null;
			Killerblow = null;
			Firewall = null;
		}

		public override string ComponentName
		{
			get { return "Right";}
		}
	}
}
