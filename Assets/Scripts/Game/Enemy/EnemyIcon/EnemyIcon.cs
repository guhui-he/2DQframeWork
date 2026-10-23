using UnityEngine;
using QFramework;

namespace Game
{
	public partial class EnemyIcon : ViewController,IController
	{
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private IEnemyIconModel mEnemyModel;
        public int id;

        private float timer = 0;
        public void Init(int enemyId)
        {
            mEnemyModel = this.GetModel<IEnemyIconModel>();
            id=enemyId;
            var data = mEnemyModel.GetEnemy(id);
            data.CurrentPosition.RegisterWithInitValue(curPos =>
            {
                transform.position = curPos;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            if (!data.HasSpawnedInitialMembers)
            {
                data.HasSpawnedInitialMembers = true;
                this.SendCommand(new CreatEnemyCommand(data));
            }
        }
    }
}
