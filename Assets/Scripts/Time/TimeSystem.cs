using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ITimeSystem : ISystem
    {
        void Pause();
        void Resume();

        IUnRegister RegisterTick(Action<float> onTick);
    }

    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        private ITimeModel mModel;
        private int mLastDay;

        private event Action<float> mOnTick;

        protected override void OnInit()
        {
            mModel = this.GetModel<ITimeModel>();
            mLastDay = mModel.Day;

            ActionKit.Repeat().Callback(Tick).StartGlobal();
        }

        private void Tick()
        {
            if (mModel.IsPaused.Value) return;

            float addSeconds = Time.deltaTime * mModel.SecondsPerRealSecond;
            mModel.TotalGameSeconds.Value += addSeconds;
            mOnTick?.Invoke(addSeconds);

            DetectTimeBoundary();
        }

        private void DetectTimeBoundary()
        {
            var currentDay = mModel.Day;
            if (currentDay != mLastDay)
            {
                mLastDay = currentDay;
                this.SendEvent(new DayChangedEvent { Day = currentDay });
            }
        }

        public void Pause()
        {
            mModel.PauseCount++;
            Debug.Log(mModel.PauseCount);
            if (mModel.PauseCount > 0) mModel.IsPaused.Value = true;
        }

        public void Resume()
        {
            mModel.PauseCount--;
            if (mModel.PauseCount <= 0)
            {
                mModel.PauseCount = 0;
                mModel.IsPaused.Value = false;
            }
        }
        public IUnRegister RegisterTick(Action<float> onTick)
        {
            mOnTick += onTick;
            return new CustomUnRegister(() => { mOnTick -= onTick; });
        }
    }

    public class CustomUnRegister : IUnRegister
    {
        private Action mOnUnRegister;
        public CustomUnRegister(Action onUnRegister) => mOnUnRegister = onUnRegister;
        public void UnRegister() => mOnUnRegister?.Invoke();
    }
}
