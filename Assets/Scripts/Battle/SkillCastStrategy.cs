using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ISkillCastStrategy
    {
        bool CanCast(BattleUnitData caster, SkillInfo skill, GridPos castPos, GridPos targetPos);
        void Cast(BattleUnitData caster, SkillInfo skill, List<BattleUnitData> targets, GridPos targetPos, IActionQueueSystem queue);
    }

    // ================= 1. 默认策略（单体、AOE、自身） =================
    public class DefaultCastStrategy : ISkillCastStrategy
    {
        public bool CanCast(BattleUnitData caster, SkillInfo skill, GridPos castPos, GridPos targetPos)
        {
            int maxRange = skill.Config.atkRange + caster.SourceInfo.GetFinalStat().AtkRange;
            return castPos.ManhattanTo(targetPos) <= maxRange;
        }

        public void Cast(BattleUnitData caster, SkillInfo skill, List<BattleUnitData> targets, GridPos targetPos, IActionQueueSystem queue)
        {
            queue.Enqueue(new AttackAction(caster, targets, skill, targetPos));
        }
    }

    // ================= 2. 射线策略（十字方向，原地瞬发） =================
    public class LaserCastStrategy : ISkillCastStrategy
    {
        public bool CanCast(BattleUnitData caster, SkillInfo skill, GridPos castPos, GridPos targetPos)
        {
            int maxRange = skill.Config.atkRange + caster.SourceInfo.GetFinalStat().AtkRange;
            // 射线必须同排或同列，且不能点自己脚下
            if (castPos.Row != targetPos.Row && castPos.Col != targetPos.Col) return false;
            if (castPos.Equals(targetPos)) return false;

            return castPos.ManhattanTo(targetPos) <= maxRange;
        }

        public void Cast(BattleUnitData caster, SkillInfo skill, List<BattleUnitData> targets, GridPos targetPos, IActionQueueSystem queue)
        {
            // 射线不移动，原地直接出招
            queue.Enqueue(new AttackAction(caster, targets, skill, targetPos));
        }
    }

    // ================= 3. 冲锋策略（位移到目标面前） =================
    public class ChargeCastStrategy : ISkillCastStrategy
    {
        public bool CanCast(BattleUnitData caster, SkillInfo skill, GridPos castPos, GridPos targetPos)
        {
            int maxRange = skill.Config.atkRange + caster.SourceInfo.GetFinalStat().AtkRange;
            if (castPos.Row != targetPos.Row && castPos.Col != targetPos.Col) return false;
            if (castPos.ManhattanTo(targetPos) > maxRange) return false;

            GridPos dest = CalculateChargeDest(castPos, targetPos);
            return !dest.Equals(castPos); // 算出的落点不能是原地
        }

        public void Cast(BattleUnitData caster, SkillInfo skill, List<BattleUnitData> targets, GridPos targetPos, IActionQueueSystem queue)
        {
            if (targets == null || targets.Count == 0) return;

            GridPos dest = CalculateChargeDest(caster.Pos, targetPos);
            if (dest.Equals(caster.Pos)) return;

            queue.Enqueue(new MoveAction(caster, dest, MoveStyle.Dash, isSkillMove: true));
            queue.Enqueue(new WaitAction(0.2f));
            queue.Enqueue(new AttackAction(caster, targets, skill, targetPos));
        }

        private GridPos CalculateChargeDest(GridPos start, GridPos targetPos)
        {
            var map = GameApp.Interface.GetModel<IMapModel>();
            var pathSys = GameApp.Interface.GetSystem<IPathfindingSystem>();

            int dr = targetPos.Row - start.Row;
            int dc = targetPos.Col - start.Col;

            if (dr != 0 && dc != 0) return start;
            if (dr == 0 && dc == 0) return start;

            var startBlock = map.GetBlock(start.Row, start.Col);
            var lineBlocks = pathSys.GetLinePath(startBlock, dr, dc, 99, stopAtUnit: true);

            if (lineBlocks.Count < 2) return start;
            var destBlock = lineBlocks[lineBlocks.Count - 2]; // 停在目标前一格

            if (destBlock.Row == start.Row && destBlock.Col == start.Col) return start;

            return new GridPos(destBlock.Row, destBlock.Col);
        }
    }

    // ================= 4. 策略工厂 =================
    public static class SkillCastStrategyFactory
    {
        // ★ 核心解耦：只看 CastMode，不再去猜 Effects 里有什么
        public static ISkillCastStrategy GetStrategy(SkillInfo skill)
        {
            switch (skill.Config.CastMode)
            {
                case SkillCastMode.Laser: return new LaserCastStrategy();
                case SkillCastMode.Charge: return new ChargeCastStrategy();
                default: return new DefaultCastStrategy();
            }
        }
    }
}
