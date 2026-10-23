namespace Game
{
    // 分钟变化
    public struct MinuteChangedEvent
    {
        public int Day;
        public int Hour;
        public int Minute;
    }

    // 小时变化
    public struct HourChangedEvent
    {
        public int Day;
        public int Hour;
    }

    // 天变化
    public struct DayChangedEvent
    {
        public int Day;
    }
}
