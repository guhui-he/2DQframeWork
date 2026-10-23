

namespace Game
{
    public enum StatType { MaxHp, Atk, Def, Step, Crit, Avd }
    public  class RoleConfigData
    {
        public int Id;
        public string Name;
        public string Icon;
        public string Model;
        public string Info;

        // 基础属性范围
        public int HpMin, HpMax;
        public int AtkMin, AtkMax;
        public int DefMin, DefMax;
        public int StepMin, StepMax;
        public int CritMin, CritMax;
        public int AvdMin, AvdMax;
    }
}
