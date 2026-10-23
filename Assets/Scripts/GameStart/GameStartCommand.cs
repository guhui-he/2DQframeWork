using QFramework;

namespace Game
{
    public struct ContinueGameEnterEvent
    { 
    }

    public class GameStartCommand : AbstractCommand
    {
        private readonly string mSceneName;
        bool load;
        public GameStartCommand(string sceneName,bool load=false)
        {
            mSceneName = sceneName;
            this.load = load;
        }
        protected override void OnExecute()
        {
            this.GetSystem<ILoadingSystem>().LoadSceneAsync(mSceneName);
            if (this.load)
            {
                this.GetModel<IGameStartModel>().IsLoadGameMode.Value = true;
            }
            else
            {
                this.GetModel<IGameStartModel>().IsLoadGameMode.Value = false;
            }
        }
    }
}
