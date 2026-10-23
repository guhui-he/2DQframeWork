using System.Collections.Generic;

namespace Game
{
    // ================= 主存档类 =================
    public class SaveData
    {
        public float TotalGameSeconds;
        public int CurrentHeroId;

        //player位置
        public float PlayerX;
        public float PlayerY;
        // 英雄列表
        public List<RoleInfoSaveData> Heroes = new List<RoleInfoSaveData>();
        // 背包列表
        public List<ItemInfoSaveData> BagItems = new List<ItemInfoSaveData>();
        // 敌人队伍列表
        public List<EnemyGroupSaveData> Enemies = new List<EnemyGroupSaveData>();

        // 大地图敌人位置
        public List<MapEnemySaveData> MapEnemies = new List<MapEnemySaveData>();

        // 刷怪计时器
        public List<SpawnTimerSaveData> SpawnTimers = new List<SpawnTimerSaveData>();

        // 商店
        public List<ShopSaveData> Shops = new List<ShopSaveData>();

        //酒馆
        public List<SillyTavernSaveData> Taverns = new List<SillyTavernSaveData>();
    }

    // ================= 子结构类 =================

    // 大地图敌人队伍的纯数据格式（Vector3 拆解为 float）
    public class MapEnemySaveData
    {
        public int Id;
        public int EnemyId;
        public int SpawnId;
        public float CreatTimer;
        public bool HasSpawnedInitialMembers;
        public float BornX, BornY, BornZ;
        public float TargetX, TargetY, TargetZ;
        public float CurrentX, CurrentY, CurrentZ;
        public float LastKnownPlayerX, LastKnownPlayerY, LastKnownPlayerZ;

        public int PerceptionResult;
        public float MoveSpeed;
    }

    // 刷怪点的倒计时
    public class SpawnTimerSaveData
    {
        public int SpawnId;
        public float Timer;
    }

    public class ShopSaveData
    {
        public int ShopId;
        public float LastRefreshTime;
        public List<ItemInfoSaveData> Items = new List<ItemInfoSaveData>();
    }

    public class SillyTavernSaveData
    {
        public int TavernId;
        public float LastRefreshTime;
        public List<RoleInfoSaveData> Heroes = new List<RoleInfoSaveData>();
    }
}
