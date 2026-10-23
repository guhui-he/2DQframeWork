
using QFramework;
namespace Game
{
    public interface IGameStartSystem : ISystem
    {

    }
    public class GameStartSystem : AbstractSystem, IGameStartSystem
    {
        protected override void OnInit()
        {
            
        }
        public void LoadSceneAsync(string sceneName)
        {
  
        }
    }
}
