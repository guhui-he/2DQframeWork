using System.Collections.Generic;
using QFramework;

namespace Game
{
    public class SkillInfo
    {
        public SkillConfig Config;
        public List<ISkillEffect> Effects { get; } = new List<ISkillEffect>();
        public BindableProperty<int> RemainingCd { get; } = new();
        public SkillInfo(SkillConfig config)
        {
            Config = config;
            Effects = SkillEffectFactory.CreateEffects(config);
            RemainingCd.Value = 0;
        }
    }
}
