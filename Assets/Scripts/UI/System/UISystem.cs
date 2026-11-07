// IUISystem.cs
using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;


namespace Game
{
    public interface IUISystem : ISystem { }


    public class UISystem : AbstractSystem, IUISystem
    {

        protected override void OnInit()
        {
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
            this.RegisterEvent<ShowEventPanelEvent>(e => UIKit.OpenPanel<GamePlayEventPanel>(new GamePlayEventPanelData { data = e.Data }));
            this.RegisterEvent<HideEventPanelEvent>(e => UIKit.ClosePanel<GamePlayEventPanel>());
            this.RegisterEvent<StoreEnterEvent>(e => UIKit.OpenPanel<StorePanel>());

            this.RegisterEvent<ShowItemInfoEvent>(e => UIKit.OpenPanel<ItemInfoPanel>(new ItemInfoPanelData { item = e.item }));
            this.RegisterEvent<HideItemInfoEvent>(e => UIKit.ClosePanel<ItemInfoPanel>());

            this.RegisterEvent<MapEnterEvent>(e =>
            {
                UIKit.OpenPanel<BattleStartPanel>();
            });
            this.RegisterEvent<OpenBuffInfoPanelEvent>(e =>UIKit.OpenPanel<BuffInfoPanel>(new BuffInfoPanelData { info = e.info }));
            this.RegisterEvent<HideBuffInfoPanelEvent>(e => UIKit.ClosePanel<BuffInfoPanel>());
            this.RegisterEvent<GameOverEvent>(e =>
            {
                UIKit.OpenPanel<GameOverPanel>();
            });
            this.RegisterEvent<OpenPanelEvent>(OnOpenPanel);
            this.RegisterEvent<ClosePanelEvent>(OnClosePanel);
        }

        private void OnOpenPanel(OpenPanelEvent e)
        {
            var config = this.GetModel<IUIPanelConfigModel>().GetConfig(e.PanelId);
            IUIData uiData = new GlobalUIData(e.Params);

            var panel = UIKit.OpenPanel(config.PrefabName, UILevel.Common,uiData: uiData);
            if (panel != null && e.OnClosed != null)
            {
                panel.OnPanelClosed(e.OnClosed);
            }
        }

        private void OnClosePanel(ClosePanelEvent e)
        {
            var config = this.GetModel<IUIPanelConfigModel>().GetConfig(e.PanelId);
            if (config != null)
            {
                UIKit.ClosePanel(config.PrefabName);
            }
        }
    }
}
