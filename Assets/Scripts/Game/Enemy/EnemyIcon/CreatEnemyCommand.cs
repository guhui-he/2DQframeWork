using QFramework;

namespace Game
{
    public class CreatEnemyCommand : AbstractCommand
    {
        private EnemyIconData _data;
        public CreatEnemyCommand(EnemyIconData data)
        {
            _data = data;
        }
        protected override void OnExecute()
        {
            this.GetSystem<IEnemyFactorySystem>().CreatEnemy(_data);
        }
    }
}
