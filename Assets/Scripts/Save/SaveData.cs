using System.Collections.Generic;

namespace Game
{
    // ================= 主存档类 =================
    public class SaveData
    {
        public float TotalGameSeconds;
        public int CurrentHeroId;
        public int CurrentSettlementId;
        public int CurrentPartyId;
        //player位置
        public float PlayerX;
        public float PlayerY;
        public int curCityId;   
        // 英雄列表
        public List<RoleInfoSaveData> Heroes = new List<RoleInfoSaveData>();
        // 背包列表
        public List<ItemInfoSaveData> BagItems = new List<ItemInfoSaveData>();
        // 敌人队伍列表
        public List<EnemyGroupSaveData> Enemies = new List<EnemyGroupSaveData>();
        public List<SettlementSaveData> Settlements = new List<SettlementSaveData>();
        // 大地图敌人位置
        public List<PartySaveData> MapEnemies = new List<PartySaveData>();
        // 商店
        public List<ShopSaveData> Shops = new List<ShopSaveData>();
        //酒馆
        public List<SillyTavernSaveData> Taverns = new List<SillyTavernSaveData>();
    }

    // ================= 子结构类 =================
    public class SettlementSaveData
    {
        public int Id;
        public int Type; 
        public int PosX;
        public int PosY;
        public int RegionId;
        public int BuildId;
        public float SpawnTimer;
        public float SpawnInterval;
        public int MaxPartyCount;
    }
    // 大地图敌人队伍的纯数据格式（Vector3 拆解为 float）
    public class PartySaveData
    {
        public int PartyId;
        public int PartyConfigId; 
        public int Type;
        public int OwnerId;
        public int Faction;
        public List<int> RolePool; 
        public int MaxMemberCount;
        public bool HasSpawnedInitialMembers;
        public float CreatTimer;
        public int State;
        public float StateTimer;
        public float MoveSpeed;

        public float BornX, BornY, BornZ;
        public float CurrentX, CurrentY, CurrentZ;
        public float TargetX, TargetY, TargetZ;
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
