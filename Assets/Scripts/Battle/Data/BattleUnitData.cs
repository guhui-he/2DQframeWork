using System.Collections.Generic;
using Unidux.Example.List;

namespace Game
{
    public class BattleUnitData
    {
        public int UnitId;
        public RoleInfo SourceInfo;
        public bool IsPlayer;

        public int Hp;
        public int CurrentStep;
        public List<SkillInfo> skills=new ();

        public GridPos Pos;
        public GridPos StartPos;

        public bool HasMoved;
        public bool HasActed;
        public bool IsAlive => Hp > 0;   
        public List<BuffInfo> buffs = new ();
        public BattleUnitData(int uid, RoleInfo info, bool isPlayer, GridPos pos)
        {
            UnitId = uid;
            SourceInfo = info;
            IsPlayer = isPlayer;
            var stats = info.GetFinalStat();
            Hp = stats.MaxHp;
            CurrentStep = stats.Step;
            Pos = pos;
            StartPos = pos;
        }
    }
}
