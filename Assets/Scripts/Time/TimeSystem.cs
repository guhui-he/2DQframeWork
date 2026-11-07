using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ITimeSystem : ISystem
    {
        void Pause();
        void Resume();
        void SetTimeScale(float scale);
        IUnRegister RegisterTick(Action<float> onTick);
        IUnRegister RegisterRealTick(Action<float> onRealTick);
    }

    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        private ITimeModel mModel;
        private int mLastDay;

        private event Action<float> mOnTick;
        private event Action<float> mOnRealTick;
        protected override void OnInit()
        {
            mModel = this.GetModel<ITimeModel>();
            mLastDay = mModel.Day;

            ActionKit.Repeat().Callback(Tick).StartGlobal();
        }

        private void Tick()
        {
            if (mModel.IsPaused.Value) return;

            float safeUnscaledDeltaTime = Mathf.Min(Time.unscaledDeltaTime, 0.1f);

            float addSeconds = safeUnscaledDeltaTime * mModel.SecondsPerRealSecond * mModel.TimeScale.Value;
            mModel.TotalGameSeconds.Value += addSeconds;
            mOnTick?.Invoke(addSeconds);
            mOnRealTick?.Invoke(safeUnscaledDeltaTime);
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
        public void SetTimeScale(float scale)
        {
            mModel.TimeScale.Value = scale;
        }
        public void Pause()
        {
            mModel.PauseCount++;
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
        public IUnRegister RegisterRealTick(Action<float> onRealTick)
        {
            mOnRealTick += onRealTick;
            return new CustomUnRegister(() => { mOnRealTick -= onRealTick; });
        }
    }

    public class CustomUnRegister : IUnRegister
    {
        private Action mOnUnRegister;
        public CustomUnRegister(Action onUnRegister) => mOnUnRegister = onUnRegister;
        public void UnRegister() => mOnUnRegister?.Invoke();
    }
}
