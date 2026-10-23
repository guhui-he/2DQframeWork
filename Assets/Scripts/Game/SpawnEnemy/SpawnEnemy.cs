using UnityEngine;
using QFramework;

namespace Game
{
    public partial class SpawnEnemy : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        [SerializeField] private int id; // SpawnId
        private IEnemyIconModel mEnemyModel;
        private ISpawnEnemyModel mSpawnEnemyModel;

        void Start()
        {
            mEnemyModel = this.GetModel<IEnemyIconModel>();
            mSpawnEnemyModel = this.GetModel<ISpawnEnemyModel>();

            // 监听全局刷怪系统发来的事件
            this.RegisterEvent<SpawnEnemyTriggerEvent>(e =>
            {
                if (e.SpawnId == this.id)
                {
                    CreatEnemyIcon(2f);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        public void CreatEnemyIcon(float speed)
        {
            var spawnData = mSpawnEnemyModel.GetSpwanEnemyData(id);
            if (spawnData == null) return;

            var data = mEnemyModel.CreateEnemy(spawnData.EnemyId, id, this.Position(), speed);

            var loader = this.GetUtility<IPrefabLoaderUtility>();
            var go = loader.Instantiate("EnemyIcon", pos: new Vector3(0, 0, -1));
            var view = go.GetComponent<EnemyIcon>();
            view.Init(data.Id);

            this.SendEvent(new EnemyCreatedEvent(data.Id));
        }
    }
}
