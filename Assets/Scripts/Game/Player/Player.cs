using Game;
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class Player : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private IPlayerModel mPlayerModel;
        private IPlayerMoveSystem mMoveSystem;

        void Start()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            mMoveSystem = this.GetSystem<IPlayerMoveSystem>();

            if (mPlayerModel.HasLoadedPosition)
            {
                ApplyTeleport(mPlayerModel.Position.Value);
                mPlayerModel.HasLoadedPosition = false;  
            }
            else
            {
                mPlayerModel.Position.Value = transform.position;
            }
            mPlayerModel.Position.Register(pos =>
            {
   
                if (Vector2.SqrMagnitude((Vector2)transform.position - pos) > 1f * 1f)
                    ApplyTeleport(pos);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            mPlayerModel.IsMoving
                .RegisterWithInitValue(moving =>
                    SelfAnimator.Play(moving ? "move" : "idle"))
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            mPlayerModel.Velocity
                .RegisterWithInitValue(v => SelfRigidbody2D.velocity = v)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            mPlayerModel.Facing
                .RegisterWithInitValue(facing =>
                    SelfSpriteRenderer.flipX = (facing == FacingDir.Left))
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>瞬移：同步 transform、物理位置、清零速度</summary>
        private void ApplyTeleport(Vector2 pos)
        {
            var target = new Vector3(pos.x, pos.y, transform.position.z);
            transform.position = target;
            SelfRigidbody2D.position = pos;       // 2D 物理位置必须单独同步
            SelfRigidbody2D.velocity = Vector2.zero;  // 防止物理惯性把角色又推走
        }
        private Vector2 mLastReported;
        void FixedUpdate()
        {
            var pos = (Vector2)transform.position;
            if (Vector2.SqrMagnitude(pos - mLastReported) < 0.0001f) return;
            mLastReported = pos;
            this.SendEvent(new PlayerPositionChangedEvent(pos));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.SendCommand<IPlayerTriggerEnterCommand>(new PlayerTriggerEnterCommand(collision));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.SendCommand<IPlayerTriggerExitCommand>(new PlayerTriggerExitCommand(collision));
        }
    }
}
