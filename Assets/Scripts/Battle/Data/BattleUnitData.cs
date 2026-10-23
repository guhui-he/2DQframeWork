namespace Game
{
    public class BattleUnitData
    {
        public int UnitId;
        public RoleInfo SourceInfo;
        public bool IsPlayer;

        public int Hp;
        public int CurrentStep;

        public GridPos Pos;
        public GridPos StartPos;

        public bool HasMoved;
        public bool HasActed;

        public bool IsAlive => Hp > 0;   // ★ 新增

        public BattleUnitData(int uid, RoleInfo info, bool isPlayer, GridPos pos)
        {
            UnitId = uid;
            SourceInfo = info;
            IsPlayer = isPlayer;
            Hp = info.MaxHp;
            CurrentStep = info.Step;
            Pos = pos;
            StartPos = pos;
        }

        /// <summary>回合开始时统一重置（由 TurnSystem 调用）</summary>
        public void ResetForTurn()
        {
            HasMoved = false;
            HasActed = false;
            CurrentStep = SourceInfo.Step;
            StartPos = Pos;
        }
    }
}
