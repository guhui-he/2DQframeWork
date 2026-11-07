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
	public partial class HeroDes : UIElement,IController
	{
		private void Awake()
		{
        }
        public  void RefreshUI(BattleUnitData data)
        {
            Tools.SetSprite(icon, AssetPathDef.AtlasRole, data.SourceInfo.Icon);
            var stat = data.SourceInfo.GetFinalStat();
            fill.fillAmount = (float)data.Hp / stat.MaxHp;
            curTxt.text = data.Hp + "/" + stat.MaxHp;
            atk.text = stat.Attack.ToString();
            def.text = stat.Defense.ToString();
            crit.text = stat.Crit.ToString();
            avd.text = stat.Avd.ToString();
        }
        protected override void OnBeforeDestroy()
		{
		}

        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}