using QFramework;

namespace Game
{
    public interface IGameStartModel : IModel
    {
        BindableProperty<bool> IsLoadGameMode { get; }
    }
    public class GameStartModel : AbstractModel, IGameStartModel
    {
        public BindableProperty<bool> IsLoadGameMode { get; } = new BindableProperty<bool>(false);
        protected override void OnInit()
        {
            
        }
    }
}
