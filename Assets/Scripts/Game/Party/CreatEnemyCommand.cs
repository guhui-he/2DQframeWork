using QFramework;

namespace Game
{
    public class CreatEnemyCommand : AbstractCommand
    {
        private PartyInfo _data;
        public CreatEnemyCommand(PartyInfo data)
        {
            _data = data;
        }
        protected override void OnExecute()
        {
            this.GetSystem<IEnemyFactorySystem>().CreatEnemy(_data);
        }
    }
}
