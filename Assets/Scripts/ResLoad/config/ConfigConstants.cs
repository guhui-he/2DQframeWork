namespace Game
{
    public static class ConfigConstants
    {
        public const string Region = "region";
        public const string UIPanel = "uiPanel";
        public const string Role = "role";
        public const string City = "city";
        public const string FactionRelation = "factionrelation";
        public const string Party = "party";
        public const string Armor = "armor";
        public const string Boot = "boot";
        public const string Helmet = "helmet";
        public const string Shield = "shield";
        public const string Weapon = "weapon";
        public const string Supplie = "supplie";
        public const string Skill = "skill";
        public const string Effect = "effect";
        public const string Vfx = "vfx";
        public const string Buff = "buff";
        public const string Event = "gamePlayEvent";
        public const string Dialogue = "dialogue";
        /// <summary>所有配置表名(用于统一加载)</summary>
        public static readonly string[] AllTables =
        {
            Region,Role, City,Party,FactionRelation,UIPanel,
            Armor, Boot, Helmet, Shield,Dialogue,
            Weapon, Supplie, Skill,Effect,Vfx,Buff,Event,
        };
    }
}
