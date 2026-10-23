using QFramework;
using UnityEngine;
using System.Threading.Tasks;

namespace Game
{
    public interface IPlayerInputSystem : ISystem { }

    public class PlayerInputSystem : AbstractSystem, IPlayerInputSystem
    {
        private IPlayerModel mPlayerModel;
        private Vector2? mLastDir = null;

        protected override void OnInit()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            ActionKit.Repeat().Callback(ReadInput).StartGlobal();
        }

        private void ReadInput()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            var dir = new Vector2(h, v);
            if (dir.sqrMagnitude > 0.01f) dir.Normalize();

            if (mLastDir.HasValue && dir == mLastDir.Value) return;
            mLastDir = dir;

            mPlayerModel.Direction.Value = dir;
            mPlayerModel.IsMoving.Value = dir.sqrMagnitude > 0.01f;

            if (dir.x > 0.01f) mPlayerModel.Facing.Value = FacingDir.Right;
            else if (dir.x < -0.01f) mPlayerModel.Facing.Value = FacingDir.Left;
        }
    }
}
