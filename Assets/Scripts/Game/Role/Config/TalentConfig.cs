using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatGrowthEntry
    {
        public StatType Stat;
        [Tooltip("0~3 星，每档的成长范围 (min, max)")]
        public Vector2Int[] Ranges = new Vector2Int[4];
    }

    [CreateAssetMenu(fileName = "TalentConfig", menuName = "Game/Talent Config")]
    public class TalentConfig : ScriptableObject
    {
        [Header("成长值表")]
        public List<StatGrowthEntry> GrowthTable = new List<StatGrowthEntry>();

        [Header("天赋星级分布")]
        [Tooltip("挑几个属性提升为高星")]
        public int HighStarCount = 3;

        [Tooltip("高星的星级范围")]
        public Vector2Int HighStarRange = new Vector2Int(2, 3);

        [Tooltip("低星的星级范围")]
        public Vector2Int LowStarRange = new Vector2Int(0, 1);

        private Dictionary<StatType, Vector2Int[]> cache;

        /// <summary>获取某属性某星级的成长范围</summary>
        public Vector2Int GetGrowthRange(StatType stat, int star)
        {
            if (cache == null)
            {
                cache = new Dictionary<StatType, Vector2Int[]>();
                foreach (var e in GrowthTable)
                    cache[e.Stat] = e.Ranges;
            }
            if (!cache.TryGetValue(stat, out var ranges)) return new Vector2Int(1, 2);
            star = Mathf.Clamp(star, 0, ranges.Length - 1);
            return ranges[star];
        }

        void OnValidate() { cache = null; }
    }
}
