/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
	public partial class SKill : UIElement,IController
	{
        private ComponentPool<SkillSlot> _skillSlotPool;
        private ComponentPool<BuffIcon> _buffIconPool;
        private readonly List<SkillSlot> _activeSkillSlots = new List<SkillSlot>();
        private readonly List<BuffIcon> _activeBuffIcons = new List<BuffIcon>();
        private void Awake()
        {
            _skillSlotPool = new ComponentPool<SkillSlot>(SkillSlot, Content);
            _buffIconPool = new ComponentPool<BuffIcon>(BuffIcon, Buff);
            Idle.onClick.AddListener(() => this.SendCommand(new StandbyCommand()));
            Reback.onClick.AddListener(() => this.SendCommand(new CancelActionCommand()));
        }
        public void Refresh(BattleUnitData unit)
        {
            HeroDes.RefreshUI(unit);
            SetBtn(unit.IsPlayer);
            ComponentPoolUtil.Refresh(_activeSkillSlots, _skillSlotPool, unit.skills,
                (slot, skill) => slot.Init(skill));

            ComponentPoolUtil.Refresh(_activeBuffIcons, _buffIconPool, unit.buffs,
                (icon, buff) => icon.Init(buff));
        }
      
        void SetBtn(bool isplay)
        {
            Idle.interactable = isplay;
            Reback.interactable = isplay;
        }
        protected override void OnBeforeDestroy()
        {
            _activeSkillSlots.Clear();
            _activeBuffIcons.Clear();
        }
        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}