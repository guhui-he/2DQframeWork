
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICameraSystem : ISystem
    {

    }
    public class CameraSystem : AbstractSystem, ICameraSystem
    {
        private ICameraModel mCameraModel;
        protected override void OnInit()
        {
            mCameraModel = this.GetModel<ICameraModel>();
            this.RegisterEvent<PlayerPositionChangedEvent>(e =>
            {
                mCameraModel.TargetPosition.Value =
                    new Vector3(e.Position.x, e.Position.y, mCameraModel.TargetPosition.Value.z);
            });

            // 每帧平滑跟随
            ActionKit.Repeat()
                .Callback(Tick)
                .StartGlobal();
        }

        private void Tick()
        {
            var current = mCameraModel.CurrentPosition.Value;
            var target = mCameraModel.TargetPosition.Value;
            mCameraModel.CurrentPosition.Value = Vector3.Lerp(
                current, target, mCameraModel.SmoothSpeed * Time.deltaTime);
        }
    }
}
