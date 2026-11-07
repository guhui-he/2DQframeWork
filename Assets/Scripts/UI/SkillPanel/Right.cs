/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public partial class Right : UIElement
	{
        private List<SkillPrefab> mAllSkills = new List<SkillPrefab>();

        private void Awake()
        {
            // 在 Awake 时把所有技能收集起来
            mAllSkills.Add(Atk);
            mAllSkills.Add(FireballSpell);
            mAllSkills.Add(TreatmentTechnique);
            mAllSkills.Add(FrostNova);
            mAllSkills.Add(Charge);
            mAllSkills.Add(SummonSkeleton);
            mAllSkills.Add(Frenzied);
            //mAllSkills.Add(Killerblow);
            //mAllSkills.Add(Firewall);
        }
        public void RefreshAll(RoleInfo role)
        {
            foreach (var skill in mAllSkills)
            {
                skill.Refresh(role);
            }
        }
        protected override void OnBeforeDestroy()
		{
		}
	}
}