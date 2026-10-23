/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using QFramework;
using UnityEngine;

namespace Game
{
	public partial class Content : UIElement,IController
	{
        private ISillyTavernModel mHeroModel;
        private void Awake()
        {
            mHeroModel = this.GetModel<ISillyTavernModel>();
        }
        public void RefreshHero()
        {
            var all = GetComponentsInChildren<Transform>();
            foreach (var t in all)
            {
                if (t != transform)      // 必须跳过自己
                    Destroy(t.gameObject);
            }
            foreach (var hero in mHeroModel.CurHeros)
            {
                var obj = Instantiate(HeroIcon, this.transform);
                obj.SetActive(true);
                obj.GetComponent<HeroIcon>().SetIcon(hero);
            }
        }

        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}