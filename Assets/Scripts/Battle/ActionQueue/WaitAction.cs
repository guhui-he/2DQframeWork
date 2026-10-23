

namespace Game
{
    public class WaitAction : IBattleAction
    {
        private float mTime;
        public WaitAction(float time) => mTime = time;
        public void OnStart() { }
        public bool OnUpdate(float dt) { mTime -= dt; return mTime <= 0; }
        public void OnCancel() { }
    }
}
