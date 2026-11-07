using System;
using System.Collections.Generic;

namespace Game
{
    public enum BuffTickTiming { None, TurnStart, TurnEnd }
    public enum StatModifyType { Flat, Percent }
    public class StatModifier
    {
        public AffixStatType StatType; 
        public StatModifyType Type;
        public float Value;
    }
    public class BuffConfig
    {
        public int Id;
        public string Name;

        // ★ 新增：UI 表现
        public string Icon;      // 图标名（比如 icon_poison）
        public string Des;       // 描述文本

        public bool IsDebuff;
        public int Duration;

        public List<StatModifier> Modifiers = new List<StatModifier>();

        public BuffTickTiming TickTiming;
        public int TickEffectId;
        public float TickValue;
        public int TickVfxId;
    }
}
