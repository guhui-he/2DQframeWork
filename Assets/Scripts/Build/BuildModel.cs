using System.Collections.Generic;
using UnityEngine;
using QFramework;


namespace Game
{
    public interface IBuildModel : IModel
    {
        int CurBuildId { get; set; }
        IReadOnlyList<BuildData> BuildDatas { get; }         // 遍历用（比如初始化、UI 列表）
        BuildData GetBuildData(int id);                      // 按 Id 查
        void AddBuildData(BuildData data);
    }
    /// <summary>
    /// 配置表信息
    /// </summary>
    public class BuildModel : AbstractModel, IBuildModel
    {

        private readonly List<BuildData> mBuildDatas = new List<BuildData>();
        private readonly Dictionary<int, BuildData> mBuildDataMap = new Dictionary<int, BuildData>();
        public IReadOnlyList<BuildData> BuildDatas => mBuildDatas;

        public int CurBuildId { get; set; }

        protected override void OnInit() { }

        public BuildData GetBuildData(int id)
        {
            return mBuildDataMap.TryGetValue(id, out var data) ? data : null;
        }

        public void AddBuildData(BuildData data)
        {
            if (mBuildDataMap.ContainsKey(data.Id))
            {
                mBuildDataMap[data.Id] = data;
                return;
            }
            mBuildDatas.Add(data);
            mBuildDataMap[data.Id] = data;
        }
    }
}

