using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public struct AIPlan
    {
        public BattleUnitData Unit;
        public GridPos MoveTo;
        public bool ShouldMove;
        public SkillInfo SelectedSkill;
        public List<BattleUnitData> Targets;
        public GridPos TargetPos;
    }

    public interface IAISystem : ISystem { void StartEnemyTurn(); }

    public class AISystem : AbstractSystem, IAISystem
    {
        private IBattleModel mModel;
        private IMapModel mMapModel;
        private IPathfindingSystem mPath;

        protected override void OnInit()
        {
            mModel = this.GetModel<IBattleModel>();
            mMapModel = this.GetModel<IMapModel>();
            mPath = this.GetSystem<IPathfindingSystem>();
        }

        public void StartEnemyTurn()
        {
            var queue = this.GetSystem<IActionQueueSystem>();
            queue.Clear();
            queue.Enqueue(new WaitAction(0.8f));

            var enemies = new List<BattleUnitData>();
            foreach (var u in mModel.AllUnits)
                if (!u.IsPlayer && u.IsAlive) enemies.Add(u);

            ExecuteEnemyAction(enemies, 0, queue);
        }

        private void ExecuteEnemyAction(List<BattleUnitData> enemies, int index, IActionQueueSystem queue)
        {
            if (index >= enemies.Count)
            {
                queue.Enqueue(new ActionWrapper(() => {
                    this.GetSystem<IBattleTurnSystem>().StartPlayerTurn();
                }));
                return;
            }

            var enemy = enemies[index];
            if (!enemy.IsAlive)
            {
                ExecuteEnemyAction(enemies, index + 1, queue);
                return;
            }

            var plan = PlanAction(enemy);

            if (plan.ShouldMove)
            {
                queue.Enqueue(new MoveAction(enemy, plan.MoveTo));
                queue.Enqueue(new WaitAction(0.3f));
                enemy.StartPos = enemy.Pos;
                enemy.Pos = plan.MoveTo;
            }

            if (plan.SelectedSkill != null)
            {
                bool isTargetRequired = plan.SelectedSkill.Config.targetCategory == SkillTargetCategory.Enemy ||
                                        plan.SelectedSkill.Config.targetCategory == SkillTargetCategory.Ally;

                if (plan.Targets.Count > 0 || !isTargetRequired)
                {
                    var strategy = SkillCastStrategyFactory.GetStrategy(plan.SelectedSkill);
                    strategy.Cast(enemy, plan.SelectedSkill, plan.Targets, plan.TargetPos, queue);
                }
                else
                {
                    queue.Enqueue(new ActionWrapper(() => enemy.HasActed = true));
                }
            }
            else
            {
                queue.Enqueue(new ActionWrapper(() => enemy.HasActed = true));
            }

            if (plan.ShouldMove) enemy.Pos = enemy.StartPos;
            queue.Enqueue(new UnitTurnEndAction(enemy));
            queue.Enqueue(new ActionWrapper(() => {
                ExecuteEnemyAction(enemies, index + 1, queue);
            }));
        }

        private AIPlan PlanAction(BattleUnitData enemy)
        {
            var plan = new AIPlan { Unit = enemy, Targets = new List<BattleUnitData>() };
            var availableSkills = enemy.skills.FindAll(s => s.RemainingCd.Value <= 0);
            if (availableSkills.Count == 0) return plan;

            var startBlock = mMapModel.GetBlock(enemy.Pos.Row, enemy.Pos.Col);
            var reachable = mPath.GetReachableBlocks(startBlock, enemy.CurrentStep);
            reachable.Add(startBlock);

            int bestSkillScore = -1;
            GridPos bestMovePos = enemy.Pos;

            foreach (var b in reachable)
            {
                var testPos = new GridPos(b.Row, b.Col);
                if (mModel.GetUnitAt(testPos) != null && !testPos.Equals(enemy.Pos)) continue;

                foreach (var skill in availableSkills)
                {
                    var strategy = SkillCastStrategyFactory.GetStrategy(skill);
                    int score = -1;
                    GridPos targetPos = testPos;
                    List<BattleUnitData> tempTargets = new List<BattleUnitData>();

                    switch (skill.Config.targetCategory)
                    {
                        case SkillTargetCategory.Enemy:
                        case SkillTargetCategory.All:
                            var targetEnemy = FindNearestPlayer(enemy);
                            if (targetEnemy != null && strategy.CanCast(enemy, skill, testPos, targetEnemy.Pos))
                            {
                                tempTargets = BattleTargetUtility.CollectTargets(enemy, skill, targetEnemy, targetEnemy.Pos, mModel, mMapModel, mPath);
                                if (tempTargets.Count > 0)
                                {
                                    score = tempTargets.Count * 10;
                                    if (skill.Effects.Exists(e => e is DamageEffect || e is MultiHitDamageEffect)) score += 5;
                                    targetPos = targetEnemy.Pos;
                                }
                            }
                            if (skill.Config.targetCategory == SkillTargetCategory.All && score < 15)
                            {
                                var emptyScore = EvaluateGroundCast(enemy, skill, testPos, strategy, out var emptyTargets, out var emptyPos);
                                if (emptyScore > score) { score = emptyScore; tempTargets = emptyTargets; targetPos = emptyPos; }
                            }
                            break;

                        case SkillTargetCategory.Ally:
                            var targetAlly = FindLowestHpAlly(enemy);
                            if (targetAlly != null && strategy.CanCast(enemy, skill, testPos, targetAlly.Pos))
                            {
                                tempTargets = BattleTargetUtility.CollectTargets(enemy, skill, targetAlly, targetAlly.Pos, mModel, mMapModel, mPath);
                                if (tempTargets.Count > 0) { score = 20; targetPos = targetAlly.Pos; }
                            }
                            break;

                        case SkillTargetCategory.Self:
                            if (strategy.CanCast(enemy, skill, testPos, testPos))
                            {
                                score = 10;
                                targetPos = testPos;
                            }
                            break;

                        case SkillTargetCategory.Ground:
                            score = EvaluateGroundCast(enemy, skill, testPos, strategy, out tempTargets, out targetPos);
                            break;
                    }

                    if (score > bestSkillScore)
                    {
                        bestSkillScore = score;
                        bestMovePos = testPos;
                        plan.SelectedSkill = skill;
                        plan.Targets = tempTargets;
                        plan.TargetPos = targetPos;
                    }
                }
            }

            if (bestSkillScore > 0)
            {
                plan.ShouldMove = !bestMovePos.Equals(enemy.Pos);
                plan.MoveTo = bestMovePos;
            }
            else
            {
                var nearest = FindNearestPlayer(enemy);
                if (nearest != null)
                {
                    var fallbackBlocks = mPath.GetReachableBlocks(startBlock, enemy.CurrentStep);
                    GridPos closestPos = enemy.Pos;
                    int minDist = enemy.Pos.ManhattanTo(nearest.Pos);
                    foreach (var fb in fallbackBlocks)
                    {
                        var p = new GridPos(fb.Row, fb.Col);
                        if (mModel.GetUnitAt(p) != null) continue;
                        int d = p.ManhattanTo(nearest.Pos);
                        if (d < minDist) { minDist = d; closestPos = p; }
                    }
                    plan.ShouldMove = !closestPos.Equals(enemy.Pos);
                    plan.MoveTo = closestPos;
                }
            }
            return plan;
        }

        private int EvaluateGroundCast(BattleUnitData enemy, SkillInfo skill, GridPos fromPos, ISkillCastStrategy strategy, out List<BattleUnitData> targets, out GridPos pos)
        {
            targets = new List<BattleUnitData>();
            pos = fromPos;
            int[] dr = { 0, 1, 0, -1 };
            int[] dc = { 1, 0, -1, 0 };

            for (int step = 1; step <= skill.Config.atkRange; step++)
            {
                for (int i = 0; i < 4; i++)
                {
                    var check = new GridPos(fromPos.Row + dr[i] * step, fromPos.Col + dc[i] * step);
                    if (mMapModel.GetBlock(check.Row, check.Col) != null && mModel.GetUnitAt(check) == null)
                    {
                        if (strategy.CanCast(enemy, skill, fromPos, check))
                        {
                            pos = check;
                            targets = BattleTargetUtility.CollectTargets(enemy, skill, null, check, mModel, mMapModel, mPath);
                            return 15;
                        }
                    }
                }
            }
            return -1;
        }

        private BattleUnitData FindNearestPlayer(BattleUnitData enemy)
        {
            BattleUnitData nearest = null;
            int minDist = int.MaxValue;
            foreach (var u in mModel.AllUnits)
            {
                if (u.IsPlayer && u.IsAlive)
                {
                    int d = enemy.Pos.ManhattanTo(u.Pos);
                    if (d < minDist) { minDist = d; nearest = u; }
                }
            }
            return nearest;
        }

        private BattleUnitData FindLowestHpAlly(BattleUnitData enemy)
        {
            BattleUnitData lowest = null;
            float minHpRatio = 1.0f;
            foreach (var u in mModel.AllUnits)
            {
                if (!u.IsPlayer && u.IsAlive)
                {
                    float ratio = (float)u.Hp / u.SourceInfo.GetFinalStat().MaxHp;
                    if (ratio < minHpRatio) { minHpRatio = ratio; lowest = u; }
                }
            }
            return lowest;
        }
    }
}
