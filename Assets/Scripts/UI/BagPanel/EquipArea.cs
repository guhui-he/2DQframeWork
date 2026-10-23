/****************************************************************************
 * 2026.10 DESKTOP-PD9N0BT
 ****************************************************************************/


using QFramework;

namespace Game
{
	public partial class EquipArea : UIElement
	{
		private void Awake()
		{
		}
		public void Refresh(RoleInfo info)
		{
			hero2.SetActive(false);
			pig_M.SetActive(false);
			emofashi_M.SetActive(false);
			switch (info.Model)
			{
				case "emofashi_M":
					emofashi_M.SetActive(true);
					break;
				case "qinagbin_M":
					hero2.SetActive(true);
					break ;
				case "pig_M":
					pig_M.SetActive(true) ;
					break ;

            }
		}
		protected override void OnBeforeDestroy()
		{
		}
	}
}