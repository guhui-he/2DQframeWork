using UnityEngine;
using QFramework;

namespace Game
{
	public partial class SkillSlot : ViewController,IController
	{
		Animator ani;
		void Awake()
		{
			ani=btn.GetComponent<Animator>();
		}
		public void Init(SkillInfo info)
		{
            //Tools.SetSprite(icon,)
            info.RemainingCd.RegisterWithInitValue(cd =>
			{
                mask.fillAmount = (float)cd/info.Config.cooldown;
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (info.RemainingCd.Value > 0)
                {
                     ani.SetTrigger("Reject");
                    return;
                }
                ani.SetTrigger("Pressed");
                this.SendCommand(new EnterAttackCommand(info.Config.id));
            });
        }

        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}
