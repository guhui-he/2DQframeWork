
using QFramework;
using UnityEngine;

namespace Game
{
    public struct BuffChangedEvent
    {
        public int TargetId;
        public int BuffId;
        public bool IsAdded;
    }
    public interface IBuffSystem : ISystem
    {
        void AddBuff(BattleUnitData target, int buffid);
        void RemoveBuff(BattleUnitData target, int buffid);
        void TriggerTick(BattleUnitData target,BuffTickTiming tickTiming);

        void DecreaseTurn(BattleUnitData target);   
    }

    public class BuffSystem : AbstractSystem, IBuffSystem
    {
        IBuffModel mBuffModel;
        IEffectConfigModel mEffectConfigModel;

        protected override void OnInit()
        {
            mBuffModel=this.GetModel<IBuffModel>();
            mEffectConfigModel=this.GetModel<IEffectConfigModel>();
        }
        public void AddBuff(BattleUnitData target, int buffid)
        {
            var config=mBuffModel.GetBuffConfig(buffid);
            var existing = target.buffs.Find(b => b.config.Id == buffid);
            if (existing != null)
            {
                existing.RemainingTurns = config.Duration;
                Debug.Log($"[BuffSystem] 刷新了 {target.SourceInfo.Name} 的 {config.Name} 持续时间");
            }
            else
            {
                target.buffs.Add(new BuffInfo(config));
                Debug.Log($"[BuffSystem] 给 {target.SourceInfo.Name} 添加了 {config.Name}");
            }
            this.SendEvent(new BuffChangedEvent { TargetId = target.UnitId, BuffId = buffid, IsAdded = true });
        }
        public void DecreaseTurn(BattleUnitData target)
        {
            for (int i = target.buffs.Count - 1; i >= 0; i--)
            {
                var buff = target.buffs[i];
                buff.RemainingTurns--;

                if (buff.RemainingTurns <= 0)
                {
                    Debug.Log($"[BuffSystem] {target.SourceInfo.Name} 的 {buff.config.Name} 消失了");
                    target.buffs.RemoveAt(i);
                    this.SendEvent(new BuffChangedEvent { TargetId = target.UnitId, BuffId = buff.config.Id, IsAdded = false });
                }
            }
        }

        public void RemoveBuff(BattleUnitData target, int buffid)
        {
            int removed=target.buffs.RemoveAll(b=>b.config.Id == buffid);
            if (removed > 0)
            {
                this.SendEvent(new BuffChangedEvent { TargetId = target.UnitId, BuffId = buffid, IsAdded = false });
            }
        }

        public void TriggerTick(BattleUnitData target, BuffTickTiming tickTiming)
        {
            for(int i=target.buffs.Count-1; i>=0; i--)
            {
                var buff=target.buffs[i];
                if (buff.config.TickTiming != tickTiming) continue;
                if(buff.config.TickEffectId<=0) continue;
                var effect = mEffectConfigModel.GetEffect(buff.config.TickEffectId);
                var combatSystem = GameApp.Interface.GetSystem<ICombatSystem>();
                if (effect.Type == EffectType.Damage)
                {
                    combatSystem.ApplyFixedDamage(target, (int)buff.config.TickValue);
                    Debug.Log($"[Buff] {target.SourceInfo.Name} 受到 {buff.config.Name} 伤害: {buff.config.TickValue}");
                }
                else if (effect.Type == EffectType.Heal)
                {
                    combatSystem.ApplyHeal(target, (int)buff.config.TickValue);
                    Debug.Log($"[Buff] {target.SourceInfo.Name} 触发 {buff.config.Name} 回复: {buff.config.TickValue}");
                }
                if (buff.config.TickVfxId > 0)
                {
                    GameApp.Interface.SendEvent(new PlayVfxEvent
                    {
                        VfxId = buff.config.TickVfxId,
                        TargetId = target.UnitId // 让特效跟随单位播放
                    });
                }
            }

        }

    }
}
