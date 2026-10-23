using System.Collections.Generic;
using QFramework;

namespace Game
{
    public enum ShowPathType { None, _BFS, _A, Attack }
    public enum BattlePhase { Ready, PlayerTurn, EnemyTurn, GameOver }
    public enum ActionState { WaitInput, Moving, ActionMenu, Attacking }
    public enum MoveResult { Arrived, Failed, Interrupted }   // ★ 新增

    public struct BlockUIStateChangedEvent
    {
        public int Row; public int Col; public ShowPathType Type;
        public BlockUIStateChangedEvent(int r, int c, ShowPathType t) { Row = r; Col = c; Type = t; }
    }

    public struct ShowActionMenuEvent { public BattleUnitData Unit; }
    public struct HideActionMenuEvent { }
    public struct ShowRoleInfoEvent 
    {
        public BattleUnitData Unit;    
    }
    public struct UnitMoveStartEvent { public int UnitId; public List<GridPos> Path; }

    /// <summary>★ 改为携带 Result，供等待方判断成功/失败</summary>
    public struct UnitMoveCompletedEvent
    {
        public int UnitId;
        public MoveResult Result;
    }

    public struct ForceTeleportEvent { public BattleUnitData Unit; }
    public struct UnitAttackEvent { public int AtkId; }
    public struct UnitDamagedCompletedEvent { public int TargetId; }
    public struct UnitDamagedEvent { public int TargetId; public int Damage; }
    public struct UnitDieEvent { public int UnitId; }

    /// <summary>★ 携带 TurnCount，便于 UI / 回放</summary>
    public struct PhaseChangedEvent
    {
        public BattlePhase Phase;
        public int TurnCount;
    }

    /// <summary>★ 新增：胜负结算事件</summary>
    public struct GameOverEvent
    {
        public bool PlayerWin;
    }
}
