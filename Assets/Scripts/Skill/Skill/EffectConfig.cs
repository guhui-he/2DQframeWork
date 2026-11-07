

namespace Game
{
    public enum EffectType
    {
        Damage,          // 单次伤害
        MultiHitDamage,  // 多段伤害（如三连击）
        Heal,            // 治疗
        AddBuff,         // 附加 Buff / Debuff
        Summon ,          // 召唤单位
        Charge
    }

    public class EffectConfig
    {
        public int Id;
        public EffectType Type;

        // 通用参数，根据类型自行解释
        public float Value1;    // 比如：伤害倍率(1.5)、BuffId(2001)、召唤物Id(3001)
        public float Value2;    // 比如：多段伤害的段数(3)、Buff层数(1)
        public int Duration;    // Buff 持续的回合数
        public string Params;   // 其他特殊配置
    }
}
