

namespace Game
{
    public  class BuffInfo
    {
        public BuffConfig config;
        public int RemainingTurns;

        public BuffInfo(BuffConfig config)
        {
            this.config = config;
            RemainingTurns = config.Duration;
        }
    }
}
