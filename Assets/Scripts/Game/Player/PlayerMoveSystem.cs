
using QFramework;
using UnityEngine;

namespace Game
{

    public interface IPlayerMoveSystem : ISystem
    {
        void SyncPhysicsPosition(Vector2 physicsPos);
    }

    public class PlayerMoveSystem : AbstractSystem, IPlayerMoveSystem
    {
        private IPlayerModel mPlayerModel;

        protected override void OnInit()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();

            ActionKit.OnFixedUpdate.Register(ComputeVelocity);

            this.RegisterEvent<PlayerPositionChangedEvent>(e =>
            {
                if (mPlayerModel.Position.Value == e.Position) return;
                mPlayerModel.Position.Value = e.Position;
            });
        }

        private void ComputeVelocity()
        {
            
            mPlayerModel.Velocity.Value = mPlayerModel.IsMoving.Value
                ? mPlayerModel.Direction.Value * mPlayerModel.MoveSpeed
                : Vector2.zero;
        }

        public void SyncPhysicsPosition(Vector2 physicsPos)
        {
           
        }
    }
}

