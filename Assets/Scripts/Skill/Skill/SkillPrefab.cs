using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace Game
{
	public partial class SkillPrefab : ViewController,IController
	{
		[SerializeField]
		private int skillId;
        private IUnRegister mSkillPointsUnregister;
        private RoleInfo mRole;
        void Start()
		{
            btn.onClick.AddListener(() =>
			{
                this.SendCommand(new UpgradeSkillCommand(skillId));
			});
		}
        public void Refresh(RoleInfo role)
        {
            if (mRole != role)
            {
                mSkillPointsUnregister?.UnRegister();
                mRole = role;
                mSkillPointsUnregister = mRole.SkillPoints.RegisterWithInitValue(_ => 
                UpdateUI());
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (mRole == null) return;
            bool isLearned = mRole.HasLearnedSkill(skillId);

            if (isLearned)
            {
                btn.gameObject.SetActive(false);
                mask.enabled=false;
            }
            else
            {
                bool hasPoints = mRole.CanLearnSkill(this.GetModel<ISkillModel>().GetSkill(skillId).Config);
                btn.gameObject.SetActive(hasPoints);
                mask.enabled=!hasPoints;
            }
        }
        private void OnDestroy()
        {
            mSkillPointsUnregister?.UnRegister();
            mSkillPointsUnregister = null;
        }
        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}
