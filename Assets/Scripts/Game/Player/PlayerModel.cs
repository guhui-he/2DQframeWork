using QFramework;
using Unity.Properties;
using UnityEngine;
namespace Game
{
    public enum FacingDir { Left, Right }
    public interface IPlayerModel : IModel
    {
        BindableProperty<Vector2> Position { get; }
        BindableProperty<Vector2> Direction { get; }
        BindableProperty<bool> IsMoving { get; }
        BindableProperty<Vector2> Velocity { get; }
        BindableProperty<FacingDir> Facing { get; }
        float MoveSpeed { get; }
        bool HasLoadedPosition { get; set; }
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public BindableProperty<Vector2> Position { get; } = new BindableProperty<Vector2>(Vector2.zero);
        public BindableProperty<Vector2> Direction { get; } = new BindableProperty<Vector2>(Vector2.zero);

        public BindableProperty<Vector2> Velocity { get; } = new BindableProperty<Vector2>(Vector2.zero);
        public BindableProperty<bool> IsMoving { get; } = new BindableProperty<bool>(false);
        public BindableProperty<FacingDir> Facing { get; } = new BindableProperty<FacingDir>(FacingDir.Right);
        public float MoveSpeed { get; } = 3.5f;
        public bool HasLoadedPosition { get; set; } = false;
        protected override void OnInit() { }
    }
}
