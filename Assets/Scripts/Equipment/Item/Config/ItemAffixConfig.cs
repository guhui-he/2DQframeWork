using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>单条词条的数值配置</summary>
    [Serializable]
    public class AffixValueEntry
    {
        public AffixStatType Stat;

        [Tooltip("1级最小值")]
        public float MinBase = 10;
        [Tooltip("1级最大值")]
        public float MaxBase = 30;
        [Tooltip("每级乘算增长率,如 0.15 表示每级 ×1.15")]
        public float GrowthRate = 0.15f;
        [Tooltip("估价权重,每点数值值多少金币")]
        public float PriceWeight = 5;
    }

    /// <summary>装备部位 → 词条池</summary>
    [Serializable]
    public class AffixPoolEntry
    {
        public ItemType Type;
        public AffixStatType[] Stats;
    }

    /// <summary>等级 → 词条数量</summary>
    [Serializable]
    public class AffixCountEntry
    {
        public int MinLevel = 1;
        public int MaxLevel = 2;
        public int MinCount = 0;
        public int MaxCount = 1;
    }

    /// <summary>全局词条配置(整个游戏一份)</summary>
    [CreateAssetMenu(fileName = "ItemAffixConfig", menuName = "Game/Item Affix Config")]
    public class ItemAffixConfig : ScriptableObject
    {
        [Header("词条数值配置")]
        public List<AffixValueEntry> ValueConfigs = new List<AffixValueEntry>();

        [Header("各部位词条池")]
        public List<AffixPoolEntry> PoolConfigs = new List<AffixPoolEntry>();

        [Header("等级 → 词条数量")]
        public List<AffixCountEntry> CountConfigs = new List<AffixCountEntry>();

        // 运行时缓存
        private Dictionary<AffixStatType, AffixValueEntry> valueCache;
        private Dictionary<ItemType, AffixStatType[]> poolCache;

        /// <summary>查询词条数值配置</summary>
        public AffixValueEntry GetValueConfig(AffixStatType stat)
        {
            if (valueCache == null)
            {
                valueCache = new Dictionary<AffixStatType, AffixValueEntry>();
                foreach (var e in ValueConfigs)
                    valueCache[e.Stat] = e;
            }
            return valueCache.TryGetValue(stat, out var cfg) ? cfg : null;
        }

        /// <summary>查询装备部位可用词条池</summary>
        public AffixStatType[] GetPool(ItemType type)
        {
            if (poolCache == null)
            {
                poolCache = new Dictionary<ItemType, AffixStatType[]>();
                foreach (var e in PoolConfigs)
                    poolCache[e.Type] = e.Stats;
            }
            return poolCache.TryGetValue(type, out var pool) ? pool : Array.Empty<AffixStatType>();
        }

        /// <summary>查询等级的条数范围</summary>
        public (int min, int max) GetCountRange(int level)
        {
            foreach (var e in CountConfigs)
            {
                if (level >= e.MinLevel && level <= e.MaxLevel)
                    return (e.MinCount, e.MaxCount);
            }
            return (0, 0);
        }

        void OnValidate()
        {
            // 编辑器里改完自动清缓存
            valueCache = null;
            poolCache = null;
        }
    }
}
