
using QFramework;

namespace Game
{
    public interface ITimeModel:IModel
    {
        BindableProperty<float> TotalGameSeconds { get; }
        BindableProperty<bool> IsPaused { get; }
        int PauseCount { get; set; }
        int Day { get; }
        int Hour { get; }
        int Minute { get; }

        float SecondsPerRealSecond { get; } // 现实1秒相当于游戏多少秒
    }
    public class TimeModel : AbstractModel, ITimeModel
    {
        public BindableProperty<float> TotalGameSeconds { get; } = new(0f);
        public BindableProperty<bool> IsPaused { get; } = new(false);
        public int PauseCount { get; set; } = 0;

        // 现实 1 秒 = 游戏 60 秒（1分钟）
        public float SecondsPerRealSecond => 60f;

        public int Day => (int)(TotalGameSeconds.Value / (24 * 3600)) + 1; // 从第1天开始
        public int Hour => (int)(TotalGameSeconds.Value / 3600) % 24;
        public int Minute => (int)(TotalGameSeconds.Value / 60) % 60;

        protected override void OnInit() { }
    }
}
