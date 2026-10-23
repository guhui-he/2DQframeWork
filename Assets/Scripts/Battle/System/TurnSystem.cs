using QFramework;

namespace Game
{
    public interface IBattleTurnSystem : ISystem
    {
        void StartGame();
        void CheckTurnEnd();
        void StartPlayerTurn();
        void CheckGameOver();
    }

    public class TurnSystem : AbstractSystem, IBattleTurnSystem
    {
        private IBattleModel mModel;

        protected override void OnInit() => mModel = this.GetModel<IBattleModel>();

        public void StartGame()
        {
            if (mModel.Phase.Value != BattlePhase.Ready) return;
            foreach (var u in mModel.AllUnits)
            {
                u.HasActed = false;
                u.HasMoved = false;
                u.CurrentStep = u.SourceInfo.Step;
            }

            // 3. 状态切为玩家回合
            mModel.Phase.Value = BattlePhase.PlayerTurn;
            this.SendEvent(new PhaseChangedEvent { Phase = BattlePhase.PlayerTurn });
        }
        public void CheckTurnEnd()
        {
            if (mModel.Phase.Value != BattlePhase.PlayerTurn) return;
            if (!AllPlayerActed()) return;

            // 切到敌方回合
            mModel.Phase.Value = BattlePhase.EnemyTurn;
            this.SendEvent(new PhaseChangedEvent { Phase = BattlePhase.EnemyTurn });

            // 重置敌方状态
            foreach (var u in mModel.AllUnits)
                if (!u.IsPlayer && u.IsAlive) { u.HasActed = false; u.HasMoved = false; u.CurrentStep = u.SourceInfo.Step; }

            this.GetSystem<IAISystem>().StartEnemyTurn();
        }

        public void StartPlayerTurn()
        {
            if (mModel.Phase.Value == BattlePhase.GameOver) return;

            mModel.Phase.Value = BattlePhase.PlayerTurn;
            this.SendEvent(new PhaseChangedEvent { Phase = BattlePhase.PlayerTurn });

            // 重置玩家状态
            foreach (var u in mModel.AllUnits)
                if (u.IsPlayer && u.IsAlive) { u.HasActed = false; u.HasMoved = false; u.CurrentStep = u.SourceInfo.Step; }
        }

        public void CheckGameOver()
        {
            bool anyPlayer = false, anyEnemy = false;
            foreach (var u in mModel.AllUnits)
            {
                if (!u.IsAlive) continue;
                if (u.IsPlayer) anyPlayer = true;
                else anyEnemy = true;
            }

            if (!anyPlayer || !anyEnemy)
            {
                mModel.Phase.Value = BattlePhase.GameOver;
                this.SendEvent(new GameOverEvent { PlayerWin = !anyEnemy });
            }
        }

        private bool AllPlayerActed()
        {
            foreach (var u in mModel.AllUnits)
                if (u.IsPlayer && u.IsAlive && !u.HasActed) return false;
            return true;
        }
    }
}
