/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/

using QFramework;

namespace Game
{
	public partial class LeftInfo : UIElement
	{
		private void Awake()
		{

		}
		public void RefreshInfo(RoleInfo role)
		{
			nameTxt.text = role.Name;
			level.text=role.Level.ToString();
			Hp.value=role.CurHp/(float)role.MaxHp;
			curHp.text=role.CurHp+"/"+role.MaxHp;
			Exp.value = role.Curexp / (float)role.Level * 100;
			curExp.text=role.Curexp + "/"+ role.Level * 100;
		}
		protected override void OnBeforeDestroy()
		{
		}
	}
}