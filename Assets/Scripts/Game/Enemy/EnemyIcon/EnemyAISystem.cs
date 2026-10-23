using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public enum EnemyState { Idle, Patrol, Chase }

    public interface IEnemyBattleAISystem : ISystem { }

    public class EnemyBattleAISystem : AbstractSystem, IEnemyBattleAISystem
    {
        private IEnemyIconModel mEnemyModel;
        // 每个敌人一个 FSM
        private readonly Dictionary<int, FSM<EnemyState>> mFSMs = new Dictionary<int, FSM<EnemyState>>();

        private const float IdleDuration = 3f;
        private const float PatrolRadius = 10f;
        private const float ArriveThreshold = 0.1f;

        protected override void OnInit()
        {
            mEnemyModel = this.GetModel<IEnemyIconModel>();

            this.RegisterEvent<EnemyCreatedEvent>(e =>
            {
                var enemy = mEnemyModel.GetEnemy(e.EnemyId);
                if (enemy != null) BuildFSMFor(enemy);
            });

            this.RegisterEvent<EnemyRemovedEvent>(e =>
            {
                mFSMs.Remove(e.EnemyId);
            });

            ActionKit.Repeat().Callback(Tick).StartGlobal();
        }

        private void BuildFSMFor(EnemyIconData enemy)
        {
            var fsm = new FSM<EnemyState>();

            // === Idle ===
            fsm.State(EnemyState.Idle)
                .OnEnter(() => enemy.TargetPosition.Value = enemy.CurrentPosition.Value)
                .OnUpdate(() =>
                {
                    if (fsm.FrameCountOfCurrentState >= 60 * IdleDuration)
                        fsm.ChangeState(EnemyState.Patrol);
                });

            // === Patrol ===
            fsm.State(EnemyState.Patrol)
                .OnEnter(() => enemy.TargetPosition.Value = RandomPatrolPos(enemy))
                .OnUpdate(() =>
                {
                    if (ArrivedTarget(enemy))
                        fsm.ChangeState(EnemyState.Idle);
                });

            // === Chase ===
            fsm.State(EnemyState.Chase)
                .OnEnter(() => enemy.TargetPosition.Value = enemy.LastKnownPlayerPos.Value)
                .OnUpdate(() =>
                {
                    enemy.TargetPosition.Value = enemy.LastKnownPlayerPos.Value;
                })
                .OnExit(() => enemy.TargetPosition.Value = enemy.CurrentPosition.Value);

            // 订阅感知变化
            enemy.PerceptionResult.RegisterWithInitValue(result =>
            {
                switch (result)
                {
                    case PerceptionType.Detected:
                        if (fsm.CurrentStateId != EnemyState.Chase)
                            fsm.ChangeState(EnemyState.Chase);
                        break;
                    case PerceptionType.None:
                        if (fsm.CurrentStateId == EnemyState.Chase)
                            fsm.ChangeState(EnemyState.Idle);
                        break;
                }
            });

            fsm.StartState(EnemyState.Idle);
            mFSMs[enemy.Id] = fsm;
        }

        private void Tick()
        {
            foreach (var fsm in mFSMs.Values)
                fsm.Update();
        }

        private bool ArrivedTarget(EnemyIconData enemy)
            => Vector3.Distance(enemy.CurrentPosition.Value, enemy.TargetPosition.Value) < ArriveThreshold;

        private Vector3 RandomPatrolPos(EnemyIconData enemy)
        {
            var offset = Random.insideUnitCircle * PatrolRadius;
            return enemy.BornPosition + new Vector3(offset.x, offset.y, 0);
        }
    }
}
