/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public partial class HeroConent : UIElement,IController
	{
		private IHeroModel mHeroModel;
		private void Awake()
		{
			mHeroModel = this.GetModel<IHeroModel>();
		}
		public void RefreshHero()
		{
			foreach(var hero in mHeroModel.mHero)
			{
				var obj = Instantiate(HeroIcon, this.transform);
				obj.SetActive(true);
				obj.GetComponent<HeroIcon>().SetIcon(hero.Value);
				
			}
		}
		protected override void OnBeforeDestroy()
		{
		}

		public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}