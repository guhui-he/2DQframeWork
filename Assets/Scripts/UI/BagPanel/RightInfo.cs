/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/
using QFramework;

namespace Game
{
	public partial class RightInfo : UIElement
	{
		private void Awake()
		{
		}
		public void RefreshInfo(RoleInfo info)
		{
			hp.text = info.MaxHp.ToString();
			atk.text=info.Atk.ToString();
			def.text=info.Def.ToString();
			avd.text=info.Avd.ToString();
			crit.text=info.Crit.ToString();
			step.text=info.Step.ToString();
			txt.text=info.PendingPoints.ToString();

        }
		protected override void OnBeforeDestroy()
		{
		}
	}
}