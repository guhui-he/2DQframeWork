
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game
{
    public interface ILoadingSystem : ISystem
    {
        void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
    }
    public class LoadingSystem : AbstractSystem, ILoadingSystem
    {
        private ResLoader mResLoader;

        protected override void OnInit()
        {
            mResLoader = ResLoader.Allocate();
        }

        public void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            //this.SendEvent(new SceneLoadStartEvent(sceneName));
            this.SendEvent(new OpenPanelEvent { PanelId = UIPanelId.LoadingPanel });
            mResLoader.LoadSceneAsync(sceneName, mode,
              onStartLoading: (Operation) =>
               {
                   Operation.completed += asyncOperation =>
                   {
                       this.SendEvent(new SceneLoadCompleteEvent(sceneName));
                           
                   };
               });
        }

        protected override void OnDeinit()
        {
            // 系统销毁时回收场景资源
            mResLoader?.Recycle2Cache();
            mResLoader = null;
        }
    }
}
