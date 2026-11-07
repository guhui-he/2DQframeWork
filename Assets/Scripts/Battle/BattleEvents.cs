using System.Collections.Generic;
using QFramework;

namespace Game
{
    public enum ShowPathType { None, _BFS, _A, Attack }
    public enum BattlePhase { Ready, PlayerTurn, EnemyTurn, GameOver }
    public enum ActionState { WaitInput, Moving, Attacking,TargetConfirm, Inspecting, Busy }

    public struct BlockUIStateChangedEvent
    {
        public int Row; public int Col; public ShowPathType Type;
        public BlockUIStateChangedEvent(int r, int c, ShowPathType t) { Row = r; Col = c; Type = t; }
    }
    public struct UnitMoveStartEvent
    {
        public int UnitId;
        public List<GridPos> Path;  // 逐格路径
        public GridPos From;        // 起点
        public GridPos To;          // 终点
        public MoveStyle Style;     // 样式
    }

    public struct UnitSpawnedEvent
    {
        public int UnitId;
        public string ModelPath; // 预制体路径
        public string AniPath;
        public GridPos Pos;      // 生成位置
    }
    public struct UnitMoveCompletedEvent
    {
        public int UnitId;
    }

    public struct ForceTeleportEvent { public BattleUnitData Unit; }
    public struct UnitAttackEvent { public int AtkId; public int SkillId; }
    public struct UnitDamagedCompletedEvent { public int TargetId; }
    public struct UnitDamagedEvent { public int TargetId; public int Damage;public int HitCount; }
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
