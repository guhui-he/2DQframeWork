using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IEffectConfigModel : IModel
    {
        void AddEffect(EffectConfig config);
        EffectConfig GetEffect(int id);
        bool TryGetEffect(int id, out EffectConfig config);
    }

    public class EffectConfigModel : AbstractModel, IEffectConfigModel
    {
        private readonly Dictionary<int, EffectConfig> mEffects = new();

        protected override void OnInit() { }

        public void AddEffect(EffectConfig config)
        {
            if (config == null) return;
            mEffects[config.Id] = config;
        }

        public EffectConfig GetEffect(int id)
            => mEffects.TryGetValue(id, out var e) ? e : null;

        public bool TryGetEffect(int id, out EffectConfig config)
            => mEffects.TryGetValue(id, out config);
    }
}
