using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEnemyFactorySystem : ISystem
    {
        void CreatEnemy(PartyInfo data);
    }

    public class EnemyFactorySystem : AbstractSystem, IEnemyFactorySystem
    {
        private IPartyModel mPartyModel;
        private ITimeSystem mTimeSystem;
        private ITimeModel mTimeModel;
        private IEnemyModel mEnemyModel;

        protected override void OnInit()
        {
            mPartyModel = this.GetModel<IPartyModel>();
            mTimeSystem = this.GetSystem<ITimeSystem>();
            mTimeModel = this.GetModel<ITimeModel>();
            mEnemyModel = this.GetModel<IEnemyModel>();

            mTimeSystem.RegisterRealTick(OnTick);
        }

        private void OnTick(float dt)
        {
            var parties = mPartyModel.Parties;

            for (int i = 0; i < parties.Count; i++)
            {
                var party = parties[i];

                var roles = mEnemyModel.GetRoleInfos(party.PartyId);
                if (roles != null && roles.Count >= party.Config.MaxMember) continue;

                party.CreatTimer -= dt;

                if (party.CreatTimer <= 0)
                {
                    CreatEnemy(party);
                    party.CreatTimer = 20f;
                }
            }
        }

        public void CreatEnemy(PartyInfo data)
        {
            
            int randomRoleId = data.RolePool[Random.Range(0, data.RolePool.Count)];

            float decay = 1.0f;
            if (data.Type == PartyType.Caravan) decay = 0.5f;
            else if (data.Type == PartyType.Enemy || data.Type == PartyType.Wanderer) decay = 1.5f;
            int randomLevel = Tools.Roll(data.Config.MinRarity, data.Config.MaxRarity, decay);
            Debug.Log($"队伍{data.Config.Name},类型{data.Config.Type.ToString()}创建角色:{randomRoleId}");
            //var enemy = this.GetSystem<IRoleFactorySystem>().CreateRole(randomRoleId, randomLevel);
            //mEnemyModel.AddEnemy(data.PartyId, enemy);
        }
    }
}
