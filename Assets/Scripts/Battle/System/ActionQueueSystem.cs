using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IActionQueueSystem : ISystem
    {
        bool IsRunning { get; }
        void Enqueue(IBattleAction action);
        void Clear();
    }

    public class ActionQueueSystem : AbstractSystem, IActionQueueSystem
    {
        private readonly Queue<IBattleAction> mActionQueue = new Queue<IBattleAction>();
        private IBattleAction mCurrentAction;
        private IBattleModel mBattleModel;

        private bool mIsUpdating = false; // ★ 防重入锁

        public bool IsRunning => mCurrentAction != null || mActionQueue.Count > 0;

        protected override void OnInit()
        {
            mBattleModel = this.GetModel<IBattleModel>();
            ActionKit.Repeat().Callback(UpdateQueue).StartGlobal();
        }

        public void Enqueue(IBattleAction action)
        {
            if (action != null) mActionQueue.Enqueue(action);
        }

        public void Clear()
        {
            mCurrentAction?.OnCancel();
            while (mActionQueue.Count > 0) mActionQueue.Dequeue().OnCancel();
            mCurrentAction = null;
        }

        private void UpdateQueue()
        {
            // ★ 防重入保护
            if (mIsUpdating) return;
            mIsUpdating = true;

            try
            {
                if (mBattleModel == null || mBattleModel.Phase.Value == BattlePhase.GameOver)
                {
                    if (mCurrentAction != null || mActionQueue.Count > 0) Clear();
                    return;
                }

                if (mCurrentAction == null)
                {
                    if (mActionQueue.Count == 0) return;
                    mCurrentAction = mActionQueue.Dequeue();

                    if (mCurrentAction != null)
                    {
                        mCurrentAction.OnStart();
                    }
                }

                // ★ 如果 OnStart 内部引发了 Clear() 或其他操作，这里判空
                if (mCurrentAction == null) return;

                if (mCurrentAction.OnUpdate(Time.deltaTime))
                {
                    mCurrentAction = null;
                    if (mActionQueue.Count == 0)
                    {
                        // ★ 安全获取 System
                        var turnSystem = this.GetSystem<IBattleTurnSystem>();
                        if (turnSystem != null) turnSystem.CheckTurnEnd();
                    }
                }
            }
            finally
            {
                mIsUpdating = false;
            }
        }
    }
}
