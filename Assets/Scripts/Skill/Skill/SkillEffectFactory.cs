using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class SkillEffectFactory
    {
        public static List<ISkillEffect> CreateEffects(SkillConfig config)
        {
            var list = new List<ISkillEffect>();
            if (config.Effects == null) return list;

            foreach (var e in config.Effects)
            {
                switch (e.Type)
                {
                    case EffectType.Damage:
                    case EffectType.Charge: 
                        list.Add(new DamageEffect { Ratio = e.Value1 });
                        break;
                    case EffectType.MultiHitDamage:
                        list.Add(new MultiHitDamageEffect { Ratio = e.Value1, HitCount = (int)e.Value2 });
                        break;
                    case EffectType.Heal:
                        list.Add(new HealEffect { Ratio = e.Value1 });
                        break;
                    case EffectType.AddBuff:
                        list.Add(new AddBuffEffect { BuffId = (int)e.Value1, Duration = e.Duration });
                        break;
                    case EffectType.Summon:
                        list.Add(new SummonEffect { SummonUnitId = (int)e.Value1 });
                        break;
                }
            }
            return list;
        }
    }
}
