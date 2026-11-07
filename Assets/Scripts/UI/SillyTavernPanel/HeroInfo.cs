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
				var stat = hero.GetFinalStat();
				nameTxt.text=hero.Name;
				levelTxt.text=hero.Level.ToString();
				Hp.value=(float)hero.CurHp/stat.MaxHp;
				curHp.text = hero.CurHp + "/" + stat.MaxHp;
				Exp.value = (float)hero.Curexp / (hero.Level * 100);
				curExp.text =hero.Curexp+"/" +(hero.Level * 100);
				hp.text = stat.MaxHp.ToString();
				atk.text = stat.Attack.ToString();
				def.text = stat.Defense.ToString();
				crit.text = stat.Crit.ToString();
				avd.text = stat.Avd.ToString();
				step.text = stat.Step.ToString();
				txt.text=hero.Info.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
		}

		protected override void OnBeforeDestroy()
		{
		}

		public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}