
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
        IEnemyIconModel model;
        protected override void OnInit()
        {
            model =this.GetModel<IEnemyIconModel>();
        }

        public  void PlayerEnterEnemy(Collider2D col)
        {
           var Id=model.GetEnemy(col.GetComponent<EnemyIcon>().id).Id;
            this.SendEvent(new SceneEnterEvent(Id));
            this.GetSystem<ILoadingSystem>().LoadSceneAsync("level_1");
        }

        public void PlayerExitEnemy(Collider2D col)
        {
            //col.GetComponent<EnemyIcon>().
        }
    }
}
