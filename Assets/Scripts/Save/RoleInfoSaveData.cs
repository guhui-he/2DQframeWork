using System.Collections.Generic;

namespace Game
{
    // 辅助结构：用于替代 Dictionary<int, int>
    public class IntIntPair
    {
        public int Key;
        public int Value;
    }

    // 辅助结构：用于替代 Dictionary<int, ItemInfoSaveData>
    public class IntItemPair
    {
        public int Key; // ItemType (int)
        public ItemInfoSaveData Value;
    }

    // 辅助结构：用于替代 Dictionary<int, List<RoleInfoSaveData>>
    public class EnemyGroupSaveData
    {
        public int EnemyId;
        public List<RoleInfoSaveData> Roles = new List<RoleInfoSaveData>();
    }

    // ================= 角色存档类 =================
    public class RoleInfoSaveData
    {
        public int Id;
        public string Name;
        public string Info;
        public string Icon;
        public string Model;

        public List<AffixSaveData> Stats = new List<AffixSaveData>();
        public int CurHp;

        public bool isBuy;
        public int Level;
        public int Curexp;
        public int PendingPoints;
        public int SkillPoints;
        public List<IntIntPair> Talents = new List<IntIntPair>();
        public List<IntItemPair> Equipments = new List<IntItemPair>();
        public List<int> SkillIds= new List<int>();
    }

    public class ItemInfoSaveData
    {
        public int ConfigId;
        public int Area;
        public List<AffixSaveData> Affixes = new List<AffixSaveData>();
    }

    public class AffixSaveData
    {
        public int Stat;
        public int Value;
    }
}
