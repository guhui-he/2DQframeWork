using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace Game
{
	public partial class GameController : ViewController,IController,ICanSendEvent
	{
        public IArchitecture GetArchitecture() => GameApp.Interface;

        void Start()
		{
			UIKit.OpenPanel<GamePanel>();
          
            this.RegisterEvent<GameLoadedEvent>(e =>
            {
                var enemies = this.GetModel<IEnemyIconModel>().Enemies;
                foreach (var enemy in enemies)
                {
                    var loader = this.GetUtility<IPrefabLoaderUtility>();
                    var go = loader.Instantiate("EnemyIcon", 
                        pos: new Vector3(enemy.CurrentPosition.Value.x, enemy.CurrentPosition.Value.y, -1));
                    go.GetComponent<EnemyIcon>().Init(enemy.Id);
                    Debug.Log(enemy.Id);
                    // ★ 重新激活 AI 状态机
                    this.SendEvent(new EnemyCreatedEvent(enemy.Id));
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            var gameModel = this.GetModel<IGameStartModel>();
            if(gameModel.IsLoadGameMode.Value)
            this.GetSystem<ISaveSystem>().LoadGame();

        }
	}
}
