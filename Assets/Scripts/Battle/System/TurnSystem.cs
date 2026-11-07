using QFramework;
using UnityEngine;

namespace Game
{
    public interface IBattleTurnSystem : ISystem
    {
        void StartGame();
        void CheckTurnEnd();
        void StartPlayerTurn();
        void StartEnemyTurn();
        void CheckGameOver();
    }

    public class TurnSystem : AbstractSystem, IBattleTurnSystem
    {
        private IBattleModel mModel;

        protected override void OnInit() => mModel = this.GetModel<IBattleModel>();

        public void StartGame()
        {
            if (mModel.Phase.Value != BattlePhase.Ready) return;
            StartPlayerTurn();
        }

        public void CheckTurnEnd()
        {
            if (mModel.Phase.Value != BattlePhase.PlayerTurn) return;
            if (!AllPlayerActed()) return;
            StartEnemyTurn();
        }

        public void StartPlayerTurn()
        {
            if (mModel.Phase.Value == BattlePhase.GameOver) return;
            mModel.Phase.Value = BattlePhase.PlayerTurn;
            this.SendEvent(new PhaseChangedEvent { Phase = BattlePhase.PlayerTurn });

            // ★ 核心修复 1：重置 UI 交互状态，清空选中和路径高亮
            mModel.SelectedUnit.Value = null;
            mModel.Action.Value = ActionState.WaitInput;
            this.GetSystem<IPathPreviewSystem>().ClearAllGrids();

            var queue = this.GetSystem<IActionQueueSystem>();
            queue.Clear();

            // 遍历所有玩家，依次排队“回合开始”
            foreach (var u in mModel.AllUnits)
            {
                if (u.IsPlayer && u.IsAlive)
                {
                    queue.Enqueue(new UnitTurnStartAction(u));
                }
            }
        }

        public void StartEnemyTurn()
        {
            if (mModel.Phase.Value == BattlePhase.GameOver) return;
            mModel.Phase.Value = BattlePhase.EnemyTurn;
            this.SendEvent(new PhaseChangedEvent { Phase = BattlePhase.EnemyTurn });

            // ★ 同理，敌人回合开始时也要重置状态，防止上一回合的残留
            mModel.SelectedUnit.Value = null;
            mModel.Action.Value = ActionState.WaitInput;
            this.GetSystem<IPathPreviewSystem>().ClearAllGrids();

            var queue = this.GetSystem<IActionQueueSystem>();
            queue.Clear();

            // 遍历所有敌人，依次排队“回合开始”
            foreach (var u in mModel.AllUnits)
            {
                if (!u.IsPlayer && u.IsAlive)
                {
                    queue.Enqueue(new UnitTurnStartAction(u));
                }
            }

            // 等所有敌人的回合开始表现播完，再启动 AI
            queue.Enqueue(new ActionWrapper(() => this.GetSystem<IAISystem>().StartEnemyTurn()));
        }

        public void CheckGameOver()
        {
            bool anyPlayer = mModel.HasAliveUnit(true);
            bool anyEnemy = mModel.HasAliveUnit(false);

            if (!anyPlayer || !anyEnemy)
            {
                mModel.Phase.Value = BattlePhase.GameOver;
                this.SendEvent(new GameOverEvent { PlayerWin = !anyEnemy });
            }
        }

        private bool AllPlayerActed()
        {
            foreach (var u in mModel.AllUnits)
            {
                if (u.IsPlayer && u.IsAlive && !u.HasActed) return false;
            }
            return true;
        }
    }
}
