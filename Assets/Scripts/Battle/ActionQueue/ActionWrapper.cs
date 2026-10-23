

namespace Game
{
    public class ActionWrapper : IBattleAction
    {
        private System.Action mAction;
        public ActionWrapper(System.Action action) => mAction = action;

        public void OnCancel() { }

        public void OnStart() => mAction?.Invoke();
        public bool OnUpdate(float dt) => true;
    }
}
