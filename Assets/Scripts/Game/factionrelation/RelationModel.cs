using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRelationModel : IModel
    {
        AttitudeType GetAttitude(Faction source, Faction target);
        bool IsHostile(Faction a, Faction b);
        void SetAttitude(Faction source, Faction target, AttitudeType newAttitude);
        void ResetToDefault(IRelationConfigModel configModel); // 重置回配置表初始状态
    }

    public class RelationModel : AbstractModel, IRelationModel
    {
        // ★ 唯一的运行时数据源
        private Dictionary<Faction, Dictionary<Faction, AttitudeType>> mRelations = new();

        protected override void OnInit() { }

        // 从配置表加载并重置
        public void ResetToDefault(IRelationConfigModel configModel)
        {
            mRelations.Clear();
            foreach (var config in configModel.Relations)
            {
                if (!mRelations.ContainsKey(config.FactionA))
                    mRelations[config.FactionA] = new Dictionary<Faction, AttitudeType>();

                mRelations[config.FactionA][config.FactionB] = config.AttitudeType;
            }
        }

        public AttitudeType GetAttitude(Faction source, Faction target)
        {
            if (source == target) return AttitudeType.Neutral;

            if (mRelations.TryGetValue(source, out var dict))
            {
                if (dict.TryGetValue(target, out var attitude))
                    return attitude;
            }
            return AttitudeType.Neutral;
        }

        public bool IsHostile(Faction a, Faction b)
        {
            if (a == b) return false;
            return GetAttitude(a, b) == AttitudeType.Hostile
                || GetAttitude(b, a) == AttitudeType.Hostile;
        }

        // ★ 直接修改当前状态（不管是运行时的事件改，还是控制台GM改）
        public void SetAttitude(Faction source, Faction target, AttitudeType newAttitude)
        {
            if (!mRelations.ContainsKey(source))
                mRelations[source] = new Dictionary<Faction, AttitudeType>();

            mRelations[source][target] = newAttitude;
            Debug.Log($"[外交] 修改：{source} 对 {target} 的态度为 {newAttitude}");
        }
    }
}
