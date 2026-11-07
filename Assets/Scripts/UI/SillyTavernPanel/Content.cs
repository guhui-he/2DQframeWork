/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using System.Collections.Generic;
using QFramework;

using UnityEngine;

namespace Game
{
	public partial class Content : UIElement,IController
	{
        private ComponentPool<HeroIcon> _heroIconPool;
        private readonly List<HeroIcon> _activeHeroIcon=new List<HeroIcon>();
        private ISillyTavernModel mHeroModel;
        private void Awake()
        {
            _heroIconPool =new ComponentPool<HeroIcon>(HeroIcon,this.transform);
            mHeroModel = this.GetModel<ISillyTavernModel>();
        }
        public void RefreshHero()
        {
            ComponentPoolUtil.Refresh(_activeHeroIcon, _heroIconPool, mHeroModel.CurHeros,
                (slot, hero) => slot.SetIcon(hero));

        }
        protected override void OnBeforeDestroy()
        {
            _activeHeroIcon.Clear();
        }
        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}