
using UnityEngine;

namespace Game
{
    public static class ItemPriceCalculator
    {
        public static int GetTotalPrice(ItemInfo item, ItemAffixConfig config)
        {
            int bonus = 0;
            foreach (var a in item.Affixes)
            {
                var cfg = config.GetValueConfig(a.Stat);
                if (cfg != null)
                    bonus += Mathf.RoundToInt(a.Value * cfg.PriceWeight);
            }
            return item.itemData.Price + bonus;
        }
    }
}
