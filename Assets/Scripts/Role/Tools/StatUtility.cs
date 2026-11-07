using System.Collections.Generic;
using Game;

namespace Game
{
    /// <summary>装备提供的属性汇总</summary>
    public struct Stat
    {
        public int Attack;
        public int Defense;
        public int MaxHp;
        public int Crit;
        public int Avd;
        public int Step;
        public int AtkRange;

        public int Get(AffixStatType type) => type switch
        {
            AffixStatType.Attack => Attack,
            AffixStatType.Defense => Defense,
            AffixStatType.MaxHp => MaxHp,
            AffixStatType.Crit => Crit,
            AffixStatType.Avd => Avd,
            AffixStatType.Step => Step,
            AffixStatType.AtkRange => AtkRange,
            _ => 0
        };

        public static Stat operator +(Stat a, Stat b) => new Stat
        {
            Attack = a.Attack + b.Attack,
            Defense = a.Defense + b.Defense,
            MaxHp = a.MaxHp + b.MaxHp,
            Crit = a.Crit + b.Crit,
            Avd = a.Avd + b.Avd,
            Step = a.Step + b.Step,
            AtkRange = a.AtkRange + b.AtkRange,
        };
    }

    public static class StatUtility
    {
        public static Stat GetFinalStat(this RoleInfo role)
        {
            return role.GetRoleStat() + Sum(role.Equipment);
        }

        public static Stat GetRoleStat(this RoleInfo role)
        {
            var result = new Stat();
            foreach (var stat in role.stat)
            {
                AddAffix(ref result, stat.Stat,stat.Value);
            }
            return result;
        }
        
        // ========== 整套装备 ==========
        /// <summary>汇总所有已装备物品的词条</summary>
        public static Stat Sum(HeroEquipment equip)
        {
            var result = new Stat();
            if (equip?.Slots == null) return result;
            foreach (var kv in equip.Slots)
            {
                var item = kv.Value?.Value;
                if (item == null) continue;
                foreach (var a in item.Affixes)
                    AddAffix(ref result, a.Stat, a.Value);
            }
            return result;
        }

        /// <summary>取整套装备某属性的词条总和</summary>
        public static int GetValue(HeroEquipment equip, AffixStatType stat)
        {
            if (equip?.Slots == null) return 0;
            int total = 0;
            foreach (var kv in equip.Slots)
            {
                var item = kv.Value?.Value;
                if (item?.Affixes == null) continue;
                foreach (var a in item.Affixes)
                    if (a.Stat == stat) total += a.Value;
            }
            return total;
        }

        // ========== 内部 ==========
        private static void AddAffix(ref Stat s, AffixStatType stat, int value)
        {
            switch (stat)
            {
                case AffixStatType.Attack: s.Attack += value; break;
                case AffixStatType.Defense: s.Defense += value; break;
                case AffixStatType.MaxHp: s.MaxHp += value; break;
                case AffixStatType.Crit: s.Crit += value; break;
                case AffixStatType.Avd: s.Avd += value; break;
                case AffixStatType.Step: s.Step += value; break;
                case AffixStatType.AtkRange: s.AtkRange += value; break;
            }
        }
    }
}
