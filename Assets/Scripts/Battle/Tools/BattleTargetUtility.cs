using System.Collections.Generic;

namespace Game
{
    public static class BattleTargetUtility
    {
        public static List<BattleUnitData> CollectTargets(
            BattleUnitData attacker,
            SkillInfo skill,
            BattleUnitData primaryTarget,
            GridPos targetPos,
            IBattleModel model,
            IMapModel map,
            IPathfindingSystem path)
        {
            var targets = new List<BattleUnitData>();
            if (attacker == null || skill == null) return targets;

            if (skill.Config.targetCategory == SkillTargetCategory.Ground) return targets;

            bool isAllyTarget = skill.Config.targetCategory == SkillTargetCategory.Ally;
            // 阵营过滤：打敌人还是打友军
            System.Func<BattleUnitData, bool> isValid = (u) =>
                u != null && u.IsAlive && (isAllyTarget ? u.IsPlayer == attacker.IsPlayer : u.IsPlayer != attacker.IsPlayer);

            if (skill.Config.targetType == SkillTargetType.Self)
            {
                if (isValid(attacker)) targets.Add(attacker);
            }
            else if (skill.Config.targetType == SkillTargetType.Single)
            {
                if (primaryTarget != null && isValid(primaryTarget)) targets.Add(primaryTarget);
            }
            else if (skill.Config.targetType == SkillTargetType.Line)
            {
                int dr = targetPos.Row - attacker.Pos.Row;
                int dc = targetPos.Col - attacker.Pos.Col;
                int maxRange = skill.Config.atkRange + attacker.SourceInfo.GetFinalStat().AtkRange;

                var startBlock = map.GetBlock(attacker.Pos.Row, attacker.Pos.Col);
                if (startBlock == null) return targets;
                bool stopAtUnit = skill.Config.CastMode == SkillCastMode.Charge;
                var lineBlocks = path.GetLinePath(startBlock, dr, dc, maxRange, stopAtUnit);

                foreach (var block in lineBlocks)
                {
                    var gridPos = new GridPos(block.Row, block.Col);
                    if (gridPos.Equals(attacker.Pos)) continue;

                    var u = model.GetUnitAt(gridPos);
                    if (isValid(u))
                    {
                        targets.Add(u);
                        // 冲锋或单体射线，命中一个就停；如果是穿透激光，则继续收集后面的敌人
                        if (skill.Config.CastMode == SkillCastMode.Charge)
                            break;
                    }
                }
            }
            else if (skill.Config.targetType == SkillTargetType.Area)
            {
                var centerBlock = map.GetBlock(targetPos.Row, targetPos.Col);
                if (centerBlock == null) return targets;

                int maxRange = skill.Config.atkRange + attacker.SourceInfo.GetFinalStat().AtkRange;
                var aoeBlocks = path.GetAttackRangeBlocks(centerBlock, maxRange);

                foreach (var b in aoeBlocks)
                {
                    var u = model.GetUnitAt(new GridPos(b.Row, b.Col));
                    if (isValid(u)) targets.Add(u);
                }
            }
            return targets;
        }
    }
}
