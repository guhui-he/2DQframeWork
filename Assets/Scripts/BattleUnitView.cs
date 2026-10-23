using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public abstract class BattleUnitView : ViewController, IController,ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        public int UnitId;
        public Animator ani;

        protected BattleUnitData mUnitData;
        protected IBattleModel mModel;

        private readonly Queue<Vector3> mPathQueue = new Queue<Vector3>();
        private Vector3 mTargetPos;
        private bool mHasTarget = false;
        private float mSmoothSpeed = 4f;

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
            mPathQueue.Clear();
            for (int i = 1; i < data.Path.Count; i++)
            {
                if (BlockViewRegistry.TryGetWorldPosition(data.Path[i], out var wp))
                    mPathQueue.Enqueue(new Vector3(wp.x, wp.y, -1));
            }
            ani.Play("move");
            TryDequeueNext();
        }

        private void TryDequeueNext()
        {
            if (mPathQueue.Count == 0)
            {
                mHasTarget = false;
                if (ani != null) ani.Play("idle");
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
            transform.position = Vector3.MoveTowards(transform.position, mTargetPos, Time.deltaTime * mSmoothSpeed);
            if (Vector3.Distance(transform.position, mTargetPos) < 0.01f)
            {
                transform.position = mTargetPos;
                mHasTarget = false;
                TryDequeueNext();
            }
        }

        protected virtual void OnAttacked()
        {
            ani.SetTrigger("Attack");
        }
        
        protected abstract void OnDamaged(int damage);
        protected abstract void OnDamagedCompleted();
        protected abstract void OnDie();
    }
}
