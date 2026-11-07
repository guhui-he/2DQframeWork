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
        }

        public void GenerateAffixes(ItemInfo item)
        {
            if (item == null || config == null) return;

            item.Affixes.Clear();

            var pool = config.GetPool(item.itemData.Type);

            var (min, max) = config.GetCountRange(item.itemData.Level);
            int count = Random.Range(min, max + 1);
            count = Mathf.Min(count, pool.Length);
            if (count <= 0) return;

            var shuffled = pool.OrderBy(_ => Random.value).ToArray();

            for (int i = 0; i < count; i++)
            {
                int value = RollValue(shuffled[i], item.itemData.Level);
                item.Affixes.Add(new StatAffix(shuffled[i], value));
            }
        }

        private int RollValue(AffixStatType stat, int level)
        {
            // 特殊:Step 概率型
            if (stat == AffixStatType.Step)
                return Tools.Roll(1, 3, 0.2f);

            // 特殊:AtkRange 用 Tools
            if (stat == AffixStatType.AtkRange)
                return Tools.Roll(1,3, 0.2f);

            var cfg = config.GetValueConfig(stat);
            float baseValue = Random.Range(cfg.MinBase, cfg.MaxBase);
            float levelMul = Mathf.Pow(1f + cfg.GrowthRate, level - 1);
            return Mathf.RoundToInt(baseValue * levelMul);
        }
    }
}
