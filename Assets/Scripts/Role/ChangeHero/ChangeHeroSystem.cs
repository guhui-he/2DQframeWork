
using QFramework;

namespace Game
{
    public interface IChangeHeroSystem : ISystem
    {
        void ChangeHero(int id);
    }
    public class ChangeHeroSystem : AbstractSystem, IChangeHeroSystem
    {
        private IHeroModel mHeroModel;
        private ISillyTavernModel sillyTavernModel;
        protected override void OnInit()
        {
            mHeroModel=this.GetModel<IHeroModel>();
            sillyTavernModel=this.GetModel<ISillyTavernModel>();
        }
        public void  ChangeHero(int id)
        {
            if(mHeroModel.HaveHero(id))
            mHeroModel.curHeroId.Value = id;
            else if(!sillyTavernModel.HaveHero(id))
                sillyTavernModel.curHeroid.Value = id;
        }
    }
}
