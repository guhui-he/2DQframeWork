using System;
using System.Collections.Generic;

namespace Game
{
    public enum AffixStatType
    {
        Attack,     // 攻击
        Defense,    // 防御
        MaxHp,      // 生命上限
        Crit,       // 暴击率
        Avd,        // 闪避率
        Step,       // 行动力
        AtkRange    // 攻击范围
    }
    public class ItemInfo
    {
        public ItemConfigData itemData;
        public List<ItemAffix> Affixes = new List<ItemAffix>();
        public AreaType area=AreaType.Bag;
        public ItemInfo() { }

        public void SetBaseData(ItemConfigData data)
        {
            itemData = data;
            Affixes.Clear();
        }

        //词条信息
        public string GetAffixDescription()
        {
            if (Affixes.Count == 0) return "";
            var sb = new System.Text.StringBuilder();
            foreach (var a in Affixes)
                sb.AppendLine(a.DisplayName);
            return sb.ToString();
        }
    }
}
