
using QFramework;

namespace Game
{
    public class UpgradeCommand : AbstractCommand
    {
        IHeroModel heroModel;
        protected override void OnExecute()
        {
            heroModel=this.GetModel<IHeroModel>();
            var curhero = heroModel.GetCurHero();
            //if (curhero.Curexp >= curhero.Level * 100)
            this.GetSystem<IRoleUpgradSystem>().Upgrade(heroModel.curHeroId.Value);
        }
    }
    public class UpgradeStatCommand : AbstractCommand
    {
        IHeroModel heroModel;
        AffixStatType type;
        public UpgradeStatCommand(AffixStatType type)
        {             
            this.type = type;
        }
        protected override void OnExecute()
        {
            heroModel = this.GetModel<IHeroModel>();

            this.GetSystem<IRoleUpgradSystem>().UpgradeStat(heroModel.curHeroId.Value,type);
        }
    }
    public class UpgradeSkillCommand : AbstractCommand
    {
        private int skillId;
        public UpgradeSkillCommand(int skillId)
        {
            this.skillId = skillId;
        }
        protected override void OnExecute()
        {
            this.GetSystem<IRoleUpgradSystem>().UpgradeSkill(skillId);
        }
    }
}
