
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ISillyTavenSystem : ISystem
    {
        void BuyHero();
    }
    public class SillyTavernSystem : AbstractSystem, ISillyTavenSystem
    {
        private ISillyTavernModel sillyTavernModel;
        private IHeroModel heroModel;
        private int townId;
        protected override void OnInit()
        {
            sillyTavernModel=this.GetModel<ISillyTavernModel>();
            heroModel =this.GetModel<IHeroModel>();
            this.RegisterEvent<TownEnterEvent>(e =>   
            {
                townId=e.BuildId;
                RefreshHero(e.BuildId);
                sillyTavernModel.SetCurHeros(townId);
            });
        }
        void RefreshHero(int id)
        {
            var currentTime = this.GetModel<ITimeModel>().TotalGameSeconds.Value;
            var LastRefreshTime = sillyTavernModel.GetFreshTime(id);
            if (currentTime - LastRefreshTime >= 86400f|| LastRefreshTime == 0f)
            {
                sillyTavernModel.Refresh(id,
                    this.GetSystem<IRoleFactorySystem>().CreateRoles(RoleType.Hero, 8));

                float daysPassed = Mathf.Floor((currentTime - LastRefreshTime) / 86400f);
                LastRefreshTime += daysPassed * 86400f;

                sillyTavernModel.SetFreshTime(id, LastRefreshTime);
            }
        }
        public void BuyHero()
        {
            var id = sillyTavernModel.curHeroid.Value;
            var hero=sillyTavernModel.Remove(townId,id);
            if(sillyTavernModel.CurHeros.Count>0)
            sillyTavernModel.curHeroid.Value = sillyTavernModel.CurHeros[0].Id;
            hero.isBuy = true;
            heroModel.AddHero(hero);
        }
    }
}
