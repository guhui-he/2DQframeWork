using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public struct AIPlan
    {
        public BattleUnitData Unit;
        public BattleUnitData Target;
        public GridPos MoveTo;
        public bool ShouldMove;
        public bool ShouldAttack;
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
            queue.Enqueue(new WaitAction(0.8f)); // 敌方回合开始停顿

            var enemies = new List<BattleUnitData>();
            foreach (var u in mModel.AllUnits)
                if (!u.IsPlayer && u.IsAlive) enemies.Add(u);

            // 遍历所有敌人，把它们的动作依次加入队列
            foreach (var enemy in enemies)
            {
                var plan = PlanAction(enemy);
                if (plan.Target == null) continue;

                // 1. 移动
                if (plan.ShouldMove)
                {
                    // 注意：MoveAction 内部会监听真实移动动画结束事件！
                    queue.Enqueue(new MoveAction(enemy, plan.MoveTo));
                    queue.Enqueue(new WaitAction(0.3f)); // 走完停顿
                }
                else
                {
                    queue.Enqueue(new ActionWrapper(() => enemy.HasMoved = true));
                }

                // 2. 攻击
                if (plan.ShouldAttack)
                {
                    // 注意：AttackAction 内部包含了前摇、结算、后摇的完整时间轴
                    queue.Enqueue(new AttackAction(enemy, plan.Target));
                }
                else
                {
                    // 不攻击则标记行动结束
                    queue.Enqueue(new ActionWrapper(() => {
                        enemy.HasActed = true;
                        enemy.HasMoved = true;
                    }));
                }
            }

            // 敌人全部行动完，切回玩家回合
            queue.Enqueue(new ActionWrapper(() => {
                this.GetSystem<IBattleTurnSystem>().StartPlayerTurn();
            }));
        }

        private AIPlan PlanAction(BattleUnitData enemy)
        {
            BattleUnitData target = FindNearestPlayer(enemy);
            if (target == null) return default;

            var startBlock = mMapModel.GetBlock(enemy.Pos.Row, enemy.Pos.Col);
            var reachable = mPath.GetReachableBlocks(startBlock, enemy.CurrentStep);

            GridPos bestPos = enemy.Pos;
            int minDist = enemy.Pos.ManhattanTo(target.Pos);

            foreach (var b in reachable)
            {
                var pos = new GridPos(b.Row, b.Col);
                if (mModel.GetUnitAt(pos) != null) continue; // 跳过被占用的格子

                int d = pos.ManhattanTo(target.Pos);
                if (d < minDist) { minDist = d; bestPos = pos; }
            }

            return new AIPlan
            {
                Unit = enemy,
                Target = target,
                MoveTo = bestPos,
                ShouldMove = !bestPos.Equals(enemy.Pos),
                ShouldAttack = minDist == 1
            };
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
    }
}
