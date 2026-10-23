
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IHeroModel : IModel
    {
        BindableProperty<int> curHeroId { get; }
        BindableDictionary<int, RoleInfo> mHero { get; }
        void AddHero(RoleInfo roleInfo);
        RoleInfo GetHero(int id);   
        
        RoleInfo GetHero();

        bool HaveHero(int id);
    }
    public class HeroModel : AbstractModel, IHeroModel
    {
        public BindableProperty<int> curHeroId { get; }=new BindableProperty<int>();
        public BindableDictionary<int,RoleInfo> mHero { get; } =new BindableDictionary<int,RoleInfo>();
        protected override void OnInit()
        {           
            
        }
        public bool HaveHero(int id)
        {
            if (mHero.TryGetValue(id, out RoleInfo roleInfo))
            {
                return true;
            }
            return false;
        }
        public void AddHero(RoleInfo roleInfo)
        {
            mHero[roleInfo.Id] = roleInfo;
            if (curHeroId.Value == 0)          // 还没有当前英雄
                curHeroId.Value = roleInfo.Id;
        }
        public RoleInfo GetHero(int id)
        {
            return mHero[id];
        }
        public RoleInfo GetHero()
        {
            return GetHero(curHeroId.Value);
        }
    }
}
