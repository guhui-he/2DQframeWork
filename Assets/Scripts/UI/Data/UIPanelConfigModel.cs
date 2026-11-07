using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IUIPanelConfigModel : IModel
    {
        UIPanelConfig GetConfig(UIPanelId panelId);
        void AddConfig(UIPanelConfig config);
    }

    public class UIPanelConfigModel : AbstractModel, IUIPanelConfigModel
    {
        private Dictionary<UIPanelId, UIPanelConfig> mConfigs = new();

        protected override void OnInit()
        {
           
        }
        public void AddConfig(UIPanelConfig config)
        {
            mConfigs[config.Id] = config;
            Debug.Log(config.Id);
        }
        public UIPanelConfig GetConfig(UIPanelId panelId)
        {
            return mConfigs.TryGetValue(panelId, out var config) ? config : null;
        }
    }
}
