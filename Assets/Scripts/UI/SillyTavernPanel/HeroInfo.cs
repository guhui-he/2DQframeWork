/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using QFramework;

namespace Game
{
	public partial class HeroInfo : UIElement,IController
	{
		ISillyTavernModel model;
		private void Awake()
		{
			model=this.GetModel<ISillyTavernModel>();
			model.curHeroid.RegisterWithInitValue(e =>
			{
				var hero=model.GetHero(e);
				nameTxt.text=hero.Name;
				levelTxt.text=hero.Level.ToString();
				Hp.value=(float)hero.CurHp/hero.MaxHp;
				curHp.text = hero.CurHp + "/" + hero.MaxHp;
				Exp.value = (float)hero.Curexp / (hero.Level * 100);
				curExp.text =hero.Curexp+"/" +(hero.Level * 100);
				hp.text=hero.MaxHp.ToString();
				atk.text=hero.Atk.ToString();
				def.text=hero.Def.ToString();
				crit.text=hero.Crit.ToString();
				avd.text=hero.Avd.ToString();
				step.text=hero.Step.ToString();
				txt.text=hero.Info.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
		}

		protected override void OnBeforeDestroy()
		{
		}

		public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}