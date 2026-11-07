using QFramework;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Game
{
    public interface IWorldSpawnSystem : ISystem
    {
        void GenerateInitialWorld();
        void OnNewDay(int currentDay);
    }

    public class WorldSpawnSystem : AbstractSystem, IWorldSpawnSystem
    {
        private IWorldMapModel model;
        private IRegionConfigModel regionConfigModel;
        private ISettlementModel settlementModel;
        private ICityConfigModel cityConfigModel;
        private IGlobalModel globalModel;
        protected override void OnInit()
        {
            model = this.GetModel<IWorldMapModel>();
            regionConfigModel = this.GetModel<IRegionConfigModel>();
            settlementModel = this.GetModel<ISettlementModel>();
            cityConfigModel = this.GetModel<ICityConfigModel>();
            globalModel = this.GetModel<IGlobalModel>();
            settlementModel.AddSettlement(new SettlementInfo { Id = 999, Type = SettlementType.Wilderness,
                SpawnInterval = 10f,BuildId=3001, config=cityConfigModel.GetCityConfig(3001)});
        }

        public void GenerateInitialWorld()
        {
            foreach (var regionId in model.SpawnableCellsByRegion.Keys)
            {
                var regionConfig = regionConfigModel.GetRegion(regionId);
                if (regionConfig == null) continue;

                int targetInitCount = Mathf.Min(2, regionConfig.MaxNestCount);
                for (int i = 0; i < targetInitCount; i++)
                {
                    SpawnNestInRegion(regionId);
                }
            }
        }

        public void OnNewDay(int currentDay)
        {
            var allRegions = model.SpawnableCellsByRegion.Keys.ToList();
            foreach (var r in allRegions)
            {
                var config = regionConfigModel.GetRegion(r);
                int currentCount = settlementModel.Settlements.Values.Count(s => s.Type == SettlementType.Nest && s.RegionId == r);
                if (config != null && currentCount < config.MaxNestCount)
                {
                    SpawnNestInRegion(r);
                }
            }
        }

        private void SpawnNestInRegion(int regionId)
        {
            var regionConfig = regionConfigModel.GetRegion(regionId);
            if (regionConfig == null || regionConfig.NestPool.Count == 0) return;

            var validCells = model.GetSpawnableCells(regionId);
            if (validCells == null) return;

            for (int i = 0; i < 20; i++)
            {
                var pos = validCells[Random.Range(0, validCells.Count)];
                if (!settlementModel.IsPosOccupied(pos))
                {
                    int randomNestId = regionConfig.NestPool[Random.Range(0, regionConfig.NestPool.Count)];

                    settlementModel.AddSettlement(new SettlementInfo
                    {
                        Id = globalModel.GetNextSettlementId(),
                        Type = SettlementType.Nest,
                        Pos = pos,
                        RegionId = regionId,
                        BuildId = randomNestId,
                        config=cityConfigModel.GetCityConfig(randomNestId),
                    });
                    break;
                }
            }
        }
    }
}
