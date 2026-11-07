using System.Collections.Generic;
using System.Data;
using QFramework;
using UnityEngine;

namespace Game
{
	public class SkillPanelData : UIPanelData
	{
	}
	public partial class SkillPanel : TimePausingPanel
	{
		IHeroModel model;
		private ComponentPool<HeroIcon> _heroInfoPool;
        private readonly List<HeroIcon> _activeHeroIcons=new List<HeroIcon>();
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as SkillPanelData ?? new SkillPanelData();
			// please add init code here
			model=this.GetModel<IHeroModel>();
			_heroInfoPool = new ComponentPool<HeroIcon>(HeroIcon,Content);
			model.curHeroId.RegisterWithInitValue(id =>
			{
				var hero = model.GetCurHero();
                Right.RefreshAll(hero);
				Nametxt.text=hero.Name;
				skillPointstxt.text=hero.SkillPoints.Value.ToString();
				Infotxt.text = hero.Info;
			}).UnRegisterWhenGameObjectDestroyed(gameObject);

			close.onClick.AddListener(CloseSelf);
		}
        protected override void OnOpen(IUIData uiData = null)
		{
			base.OnOpen(uiData);
            var heroes = new List<RoleInfo>(model.mHero.Count);
            foreach (var kv in model.mHero) heroes.Add(kv.Value);
            ComponentPoolUtil.Refresh(_activeHeroIcons, _heroInfoPool, heroes,
                (icon, hero) => icon.SetIcon(hero));
        }
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			base.OnClose();
        }
        protected override void OnBeforeDestroy()
        {
            // 池内对象是 Content 的子节点，会随面板销毁，这里清引用即可
            _activeHeroIcons.Clear();
            base.OnBeforeDestroy();
        }
    }
}
