namespace Game
{
    public static class ConfigConstants
    {
        public const string SpawnEnemy = "level";
        public const string Enemy = "enemy";
        public const string Hero = "player";
        public const string Town = "town";
        public const string Armor = "armor";
        public const string Boot = "boot";
        public const string Helmet = "helmet";
        public const string Shield = "shield";
        public const string Weapon = "weapon";
        public const string Supplie = "supplie";
        public const string Skill = "Skill";

        /// <summary>所有配置表名(用于统一加载)</summary>
        public static readonly string[] AllTables =
        {
            SpawnEnemy, Enemy, Hero, Town,
            Armor, Boot, Helmet, Shield,
            Weapon, Supplie, Skill,
        };
    }
}
