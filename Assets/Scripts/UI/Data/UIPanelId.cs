namespace Game
{
    public enum UIPanelId
    {
        None = 0,

        // 核心及系统级 UI
        GamePlayevent = 1001,
        GamePanel = 1010,
        GameStartPanel = 1011,
        LoadingPanel = 1014,
        GameOverPanel = 1009,

        // 游戏功能 UI
        Bag = 1002,
        Dialogue = 1008,
        Store = 1017,
        Town = 1018,
        Skill = 1016,
        SillyTavern = 1015,
        HeroDes = 1012,

        // 战斗及附属 UI
        Battle = 1003,
        BattleStart = 1005,
        ActionMenu = 1004,

        // 弹窗及提示 UI
        BuffInfo = 1006,
        BuildEnter = 1007,
        ItemInfo = 1013,
    }
}
