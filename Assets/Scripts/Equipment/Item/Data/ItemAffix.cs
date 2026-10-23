

namespace Game
{
    public class ItemAffix
    {
        public AffixStatType Stat;
        public int Value;
        public ItemAffix(AffixStatType stat, int value)
        {
            Stat = stat;
            Value = value;
        }

        public string DisplayName
        {
            get
            {
                string name;
                switch (Stat)
                {
                    case AffixStatType.Attack: name = "攻击"; break;
                    case AffixStatType.Defense: name = "防御"; break;
                    case AffixStatType.MaxHp: name = "生命"; break;
                    case AffixStatType.Crit: name = "暴击"; break;
                    case AffixStatType.Avd: name = "闪避"; break;
                    case AffixStatType.Step: name = "行动力"; break;
                    case AffixStatType.AtkRange: name = "攻击范围"; break;
                    default: name = Stat.ToString(); break;
                }
                return $"{name} +{Value}";
            }
        }
    }
}
