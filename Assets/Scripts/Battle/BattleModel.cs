using QFramework;

namespace Game
{
    public interface IBattleModel : IModel
    {
        BindableProperty<BattlePhase> Phase { get; }
        BindableProperty<ActionState> Action { get; }
        BindableProperty<BattleUnitData> SelectedUnit { get; }
        BindableList<BattleUnitData> AllUnits { get; }
        BindableProperty<UnitMoveStartEvent> MovePath { get; }
        BindableProperty<int> TurnCount { get; }
        BindableProperty<int> SelectedSkillId { get; }
        BindableProperty<GridPos> PendingTargetPos { get; }
        void AddUnit(BattleUnitData unit);
        void RemoveUnit(BattleUnitData unit);
        BattleUnitData GetUnitAt(GridPos pos);
        BattleUnitData GetUnitAt(int id);
        bool HasAliveUnit(bool isPlayer);         
    }

    public class BattleModel : AbstractModel, IBattleModel
    {
        public BindableProperty<BattlePhase> Phase { get; } = new(BattlePhase.Ready);
        public BindableProperty<ActionState> Action { get; } = new(ActionState.WaitInput);
        public BindableProperty<BattleUnitData> SelectedUnit { get; } = new(null);
        public BindableList<BattleUnitData> AllUnits { get; } = new();
        public BindableProperty<UnitMoveStartEvent> MovePath { get; } = new();
        public BindableProperty<int> TurnCount { get; } = new(0);
        public BindableProperty<int> SelectedSkillId { get; } = new(0);
        public BindableProperty<GridPos> PendingTargetPos { get; } = new();
        protected override void OnInit() { }

        public void AddUnit(BattleUnitData unit) => AllUnits.Add(unit);
        public void RemoveUnit(BattleUnitData unit) => AllUnits.Remove(unit);

        public BattleUnitData GetUnitAt(GridPos pos)
        {
            foreach (var u in AllUnits)
                if (u.IsAlive && u.Pos.Equals(pos)) return u;
            return null;
        }
        public BattleUnitData GetUnitAt(int id)
        {
            foreach (var u in AllUnits)
                if (u.IsAlive && u.UnitId.Equals(id)) return u;
            return null;
        }
        public bool HasAliveUnit(bool isPlayer)
        {
            foreach (var u in AllUnits)
                if (u.IsAlive && u.IsPlayer == isPlayer) return true;
            return false;
        }
    }
}
