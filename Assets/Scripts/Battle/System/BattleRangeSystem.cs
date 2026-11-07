using System;
using System.Collections.Generic;
using QFramework;

namespace Game
{
    public interface IBattleRangeSystem : ISystem
    {
        List<GridPos> GetSkillRange(BattleUnitData caster, SkillInfo skill, int maxRange);
    }

    public class BattleRangeSystem : AbstractSystem, IBattleRangeSystem
    {
        private IMapModel mMapModel;
        private IPathfindingSystem mPath;
        private static readonly int[] Dr = { 0, 1, 0, -1 };
        private static readonly int[] Dc = { 1, 0, -1, 0 };

        protected override void OnInit()
        {
            mMapModel = this.GetModel<IMapModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
        }

        public List<GridPos> GetSkillRange(BattleUnitData caster, SkillInfo skill, int maxRange)
        {
            var result = new List<GridPos>();
            var startBlock = mMapModel.GetBlock(caster.Pos.Row, caster.Pos.Col);
            if (startBlock == null) return result;

            // 1. 自身技能
            if (skill.Config.targetType == SkillTargetType.Self)
            {
                result.Add(caster.Pos);
                return result;
            }

            // 2. 直线技能（含冲锋与射线）
            if (skill.Config.targetType == SkillTargetType.Line)
            {
                bool stopAtUnit = skill.Config.CastMode != SkillCastMode.Laser;
                    for (int i = 0; i < 4; i++)
                {
                    var lineBlocks = mPath.GetLinePath(startBlock, Dr[i], Dc[i], maxRange, stopAtUnit);
                    foreach (var b in lineBlocks)
                    {
                        var pos = new GridPos(b.Row, b.Col);
                        if (!pos.Equals(caster.Pos) && !result.Contains(pos))
                        {
                            result.Add(pos);
                        }
                    }
                }
                return result;
            }

            // 3. 其他形状（单体、AOE）
            var blocks = mPath.GetAttackRangeBlocks(startBlock, maxRange);
            foreach (var b in blocks)
            {
                var pos = new GridPos(b.Row, b.Col);
                if (!pos.Equals(caster.Pos) && !result.Contains(pos))
                {
                    result.Add(pos);
                }
            }

            return result;
        }
    }
}
