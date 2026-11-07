using System.Collections.Generic;
using System.Linq;
using QFramework;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public abstract class BattleUnitView : ViewController, IController,ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        public int UnitId;
        public SkeletonAnimation ani;

        protected BattleUnitData mUnitData;
        protected IBattleModel mModel;

        private readonly Queue<Vector3> mPathQueue = new Queue<Vector3>();
        private Vector3 mTargetPos;
        private bool mHasTarget = false;
        private float mSmoothSpeed = 4f;
        private bool mIsDashing = false;
        private float mDashSpeed = 20f;
        protected virtual void Start()
        {
            mModel = this.GetModel<IBattleModel>();
            mUnitData = mModel.AllUnits.FirstOrDefault(u => u.UnitId == UnitId);
            if (mUnitData == null) return;

            this.RegisterEvent<ForceTeleportEvent>(e => {
                if (e.Unit.UnitId == UnitId) TeleportTo(e.Unit.Pos);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            mModel.MovePath.Register(OnMovePathChanged).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitAttackEvent>(e =>
            {
                if (e.AtkId == UnitId) OnAttacked();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitDamagedEvent>(e => {
                if (e.TargetId == UnitId) OnDamaged(e.Damage);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitDamagedCompletedEvent>(e =>
            {
                if (e.TargetId == UnitId)
                    OnDamagedCompleted();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnitDieEvent>(e => {
                if (e.UnitId == UnitId) OnDie();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnMovePathChanged(UnitMoveStartEvent data)
        {
            if (data.UnitId != UnitId || data.Path == null || data.Path.Count <= 1) return;
            if (data.Style == MoveStyle.Dash)
            {
                var endGridPos = data.Path[data.Path.Count - 1];
                if (BlockViewRegistry.TryGetWorldPosition(endGridPos, out var endWorldPos))
                {
                    mPathQueue.Clear(); 
                    mTargetPos = new Vector3(endWorldPos.x, endWorldPos.y, -1);
                    mHasTarget = true;
                    mIsDashing = true; 
                    if (ani != null) ani.AnimationState.SetAnimation(0, "Idle", true);
                }
                return;
            }
            mIsDashing = false;
            mPathQueue.Clear();
            for (int i = 1; i < data.Path.Count; i++)
            {
                if (BlockViewRegistry.TryGetWorldPosition(data.Path[i], out var wp))
                    mPathQueue.Enqueue(new Vector3(wp.x, wp.y, -1));
            }
            ani.AnimationState.SetAnimation(0, "Walk", true);
            TryDequeueNext();
        }

        private void TryDequeueNext()
        {
            if (mPathQueue.Count == 0)
            {
                mHasTarget = false;
                ani.AnimationState.SetAnimation(0, "Idle", true);
                this.SendEvent(new UnitMoveCompletedEvent { UnitId = UnitId });
                return;
            }
            mTargetPos = mPathQueue.Dequeue();
            mHasTarget = true;
        }

        private void TeleportTo(GridPos pos)
        {
            if (BlockViewRegistry.TryGetWorldPosition(pos, out var wp))
                transform.position = new Vector3(wp.x, wp.y, -1);
            mHasTarget = false;
            mPathQueue.Clear();
        }

        protected virtual void Update()
        {
            if (!mHasTarget) return;
            if (mIsDashing)
            {
                transform.position = Vector3.MoveTowards(transform.position, mTargetPos, Time.deltaTime * mDashSpeed);

                if (Vector3.Distance(transform.position, mTargetPos) < 0.01f)
                {
                    transform.position = mTargetPos;
                    mHasTarget = false;
                    mIsDashing = false;
                    ani.AnimationState.SetAnimation(0, "Idle", true);
                    this.SendEvent(new UnitMoveCompletedEvent { UnitId = UnitId }); 
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, mTargetPos, Time.deltaTime * mSmoothSpeed);
                if (Vector3.Distance(transform.position, mTargetPos) < 0.01f)
                {
                    transform.position = mTargetPos;
                    mHasTarget = false;
                    TryDequeueNext();
                }
            }
        }

        protected virtual void OnAttacked()
        {
            ani.AnimationState.SetAnimation(0, "Attack", false);

            // 攻击动画播放完后，自动排队回到 "idle" 循环
            ani.AnimationState.AddAnimation(0, "Idle", true, 0);
        }
        
        protected virtual void OnDamaged(int damage)
        {
            Debug.Log(damage);
            
        }
        protected abstract void OnDamagedCompleted();
        protected abstract void OnDie();
    }
}
