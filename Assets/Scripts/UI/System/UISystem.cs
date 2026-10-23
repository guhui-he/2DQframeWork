// IUISystem.cs
using UnityEngine;
using QFramework;


namespace Game
{
    public interface IUISystem : ISystem { }


    public class UISystem : AbstractSystem, IUISystem
    {
        protected override void OnInit()
        {
 
            this.RegisterEvent<SceneLoadStartEvent>(e => UIKit.OpenPanel<LoadingPanel>());

            this.RegisterEvent<SceneLoadCompleteEvent>(e => UIKit.ClosePanel<LoadingPanel>());
            this.RegisterEvent< SceneEnterEvent>(e =>
            {
                UIKit.ClosePanel<GamePanel>(); 
            });

            this.RegisterEvent<BuildTriggerEnteredEvent>(e=> 
            {
                UIKit.OpenPanel<BuildEnterPanel>(new BuildEnterPanelData { id = e.BuildId });
            });

            this.RegisterEvent<BuildTriggerExitedEvent>(e => UIKit.ClosePanel<BuildEnterPanel>());

            this.RegisterEvent<TownEnterEvent>(e =>
            {
                UIKit.OpenPanel<TownPanel>(new TownPanelData {id=e.BuildId});
            });
            this.RegisterEvent<TownExitEvent>(e =>
            {
                
                UIKit.ClosePanel<TownPanel>();
            });
            this.RegisterEvent<StoreEnterEvent>(e => UIKit.OpenPanel<StorePanel>(new StorePanelData { buildId = e.BuildId }));

            this.RegisterEvent<ShowItemInfoEvent>(e => UIKit.OpenPanel<ItemInfoPanel>(new ItemInfoPanelData { item = e.item }));
            this.RegisterEvent<HideItemInfoEvent>(e => UIKit.ClosePanel<ItemInfoPanel>());

            this.RegisterEvent<MapEnterEvent>(e =>
            {
                UIKit.OpenPanel<BattleStartPanel>();
            });

            this.RegisterEvent<ShowActionMenuEvent>(e =>
            {
                UIKit.OpenPanel<ActionMenuPanel>();
            });
            this.RegisterEvent<ShowRoleInfoEvent>(e =>
            {
                UIKit.ClosePanel<HeroDesPanel>();
                UIKit.OpenPanel<HeroDesPanel>(new HeroDesPanelData {data=e.Unit });
            });
            this.RegisterEvent<GameOverEvent>(e =>
            {
                UIKit.OpenPanel<GameOverPanel>();
            });
        }
    }
}
