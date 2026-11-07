using System.Collections.Generic;
using UnityEngine;
using QFramework;


namespace Game
{
    public interface ICityConfigModel : IModel
    {
        int CurCityId { get; set; }
        IReadOnlyList<CityConfig> CityConfigs { get; }         // 遍历用（比如初始化、UI 列表）
        CityConfig GetCityConfig(int id);                      // 按 Id 查
        void AddCityConfig(CityConfig data);
    }
    /// <summary>
    /// 配置表信息
    /// </summary>
    public class CityConfigModel : AbstractModel, ICityConfigModel
    {

        private readonly List<CityConfig> mCityConfigs = new List<CityConfig>();
        private readonly Dictionary<int, CityConfig> mBuildDataMap = new Dictionary<int, CityConfig>();
        public IReadOnlyList<CityConfig> CityConfigs => mCityConfigs;

        public int CurCityId { get; set; }

        protected override void OnInit() { }

        public CityConfig GetCityConfig(int id)
        {
            return mBuildDataMap.TryGetValue(id, out var data) ? data : null;
        }

        public void AddCityConfig(CityConfig data)
        {
            if (mBuildDataMap.ContainsKey(data.Id))
            {
                mBuildDataMap[data.Id] = data;
                return;
            }
            mCityConfigs.Add(data);
            mBuildDataMap[data.Id] = data;
        }
    }
}

