using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IAffixGeneratorSystem : ISystem
    {
        void GenerateAffixes(ItemInfo item);
    }

    public class AffixGeneratorSystem : AbstractSystem, IAffixGeneratorSystem
    {
        private ItemAffixConfig config;

        protected override void OnInit()
        {
             config = ResLoader.Allocate().LoadSync<ItemAffixConfig>("ItemAffixConfig");

            if (config == null)
                Debug.LogError("找不到 ItemAffixConfig");
        }

        public void GenerateAffixes(ItemInfo item)
        {
            if (item == null || config == null) return;

            item.Affixes.Clear();

            // 1. 拿词条池
            var pool = config.GetPool(item.itemData.Type);
            if (pool == null || pool.Length == 0)
            {
                Debug.LogWarning($"[Affix] {item.itemData.Type} 没有词条池配置");
                return;
            }

            // 2. 决定数量
            var (min, max) = config.GetCountRange(item.itemData.Level);
            int count = Random.Range(min, max + 1);
            count = Mathf.Min(count, pool.Length);
            if (count <= 0) return;

            // 3. 洗牌取前 N
            var shuffled = pool.OrderBy(_ => Random.value).ToArray();

            // 4. 逐个生成
            for (int i = 0; i < count; i++)
            {
                int value = RollValue(shuffled[i], item.itemData.Level);
                item.Affixes.Add(new ItemAffix(shuffled[i], value));
            }
        }

        private int RollValue(AffixStatType stat, int level)
        {
            // 特殊:Step 概率型
            if (stat == AffixStatType.Step)
                return Random.value < 0.2f ? 1 : 0;

            // 特殊:AtkRange 用 Tools
            if (stat == AffixStatType.AtkRange)
                return Tools.Roll(1,3, 0.2f);

            var cfg = config.GetValueConfig(stat);
            if (cfg == null)
            {
                Debug.LogWarning($"[Affix] {stat} 没有数值配置");
                return 0;
            }

            float baseValue = Random.Range(cfg.MinBase, cfg.MaxBase);
            float levelMul = Mathf.Pow(1f + cfg.GrowthRate, level - 1);
            return Mathf.RoundToInt(baseValue * levelMul);
        }
    }
}
