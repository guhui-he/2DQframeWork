using System.Data;
using QFramework;

namespace Game
{
    public partial class LeftInfo : UIElement, IController
    {
        private IUnRegister mPendingPointsUnregister;

        private void Awake()
        {
            Up.onClick.AddListener(() =>
            {
                this.SendCommand(new UpgradeCommand());
            });
        }

        public void SwitchHero(RoleInfo newRole)
        {
            if (newRole == null) return;

            mPendingPointsUnregister?.UnRegister();

            mPendingPointsUnregister = newRole.PendingPoints.Register(_ =>
            {
                RefreshInfo(newRole);
            });

            RefreshInfo(newRole);
        }

        public void RefreshInfo(RoleInfo role)
        {
            if (role == null) return;

            nameTxt.text = role.Name;
            level.text = role.Level.ToString();
            var finalStat = role.GetFinalStat();

            Hp.value = role.CurHp / (float)finalStat.MaxHp;
            curHp.text = role.CurHp + "/" + finalStat.MaxHp;
            Exp.value = role.Curexp / (float)role.Level * 100;
            curExp.text = role.Curexp + "/" + role.Level * 100;
        }

        private void OnDestroy()
        {
            mPendingPointsUnregister?.UnRegister();
            mPendingPointsUnregister = null;
        }

        protected override void OnBeforeDestroy() { }
        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}