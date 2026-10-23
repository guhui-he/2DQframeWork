namespace Game
{
    public interface IBattleAction
    {
        void OnStart();
        bool OnUpdate(float dt);   // true 代表动作结束
        void OnCancel();           // ★ 新增：队列被 Clear 时调用，用于清理订阅
    }
}
