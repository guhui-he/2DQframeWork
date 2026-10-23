
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICameraModel : IModel
    {
        BindableProperty<Vector3> TargetPosition { get; }
        BindableProperty<Vector3> CurrentPosition { get; }
        float SmoothSpeed { get; }
    }
    public class CameraModel : AbstractModel, ICameraModel
    {
        public BindableProperty<Vector3> TargetPosition { get; } = new BindableProperty<Vector3>(new Vector3(0, 0, -0.4f));

        public BindableProperty<Vector3> CurrentPosition { get; } = new BindableProperty<Vector3>(new Vector3(0,0,-0.4f));

        public float SmoothSpeed => 3;

        protected override void OnInit()
        {
            
        }
    }
}
