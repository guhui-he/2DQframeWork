
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEnemyTriggerSystem : ISystem
    {
        void PlayerEnterEnemy(Collider2D col);
        void PlayerExitEnemy(Collider2D col);
    }
    public class EnemyTriggerSystem : AbstractSystem, IEnemyTriggerSystem
    {
        IPartyModel model;
        protected override void OnInit()
        {
            model =this.GetModel<IPartyModel>();
        }

        public  void PlayerEnterEnemy(Collider2D col)
        {
            var Id=model.GetParty(col.GetComponent<PartyIcon>().id).PartyId;  
            this.SendEvent(new SceneEnterEvent(Id));
            this.GetSystem<ILoadingSystem>().LoadSceneAsync("level_1");
        }

        public void PlayerExitEnemy(Collider2D col)
        {
            //col.GetComponent<EnemyIcon>().
        }
    }
}
