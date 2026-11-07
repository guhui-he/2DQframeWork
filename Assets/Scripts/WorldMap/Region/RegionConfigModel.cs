using System;
using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IRegionConfigModel:IModel
    {
        Dictionary<int,RegionConfig> RegionConfigs { get; }
        void AddRegion(RegionConfig config);
        RegionConfig GetRegion(int regionId);
    }
    public class RegionConfigModel : AbstractModel, IRegionConfigModel
    {
        public Dictionary<int, RegionConfig> RegionConfigs { get; } = new();
        public void AddRegion(RegionConfig config)
        {
            RegionConfigs[config.RegionId] = config;
        }

        public RegionConfig GetRegion(int regionId)
        {
            if (RegionConfigs.ContainsKey(regionId))
                return RegionConfigs[regionId];
            return null;
        }

        protected override void OnInit()
        {
        }
    }
}
