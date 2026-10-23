using QFramework;

namespace Game
{
    /// <summary>
    /// 所有打开时会自动暂停时间，关闭时自动恢复时间的 Panel 都可以继承此类。
    /// </summary>
    public abstract class TimePausingPanel : UIPanel, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
            // 面板打开时暂停时间
            this.GetSystem<ITimeSystem>().Pause();
        }

        protected override void OnClose()
        {
            
            // 面板关闭时恢复时间
            this.GetSystem<ITimeSystem>().Resume();
        }
    }
}
