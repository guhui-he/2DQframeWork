using System.Collections.Generic;
using QFramework;

namespace Game
{
    /// <summary>角色运行时数据（纯数据，无生成/升级/存档逻辑）</summary>
    public class RoleInfo
    {
        // 基础
        public int Id;
        public string Name;
        public string Info;
        public string Icon;
        public string Model;
        // 属性
        public int MaxHp;
        public int CurHp;
        public int Atk;
        public int Def;
        public int Step;
        public int Crit;
        public int Avd;
        public int AtkRange;

        /// <summary>
        /// 是否已经购买
        /// </summary>
        public bool isBuy;
        // 成长
        public int Level;
        public int Curexp;
        public int PendingPoints;//加点数

        // 天赋星级
        public Dictionary<StatType, int> Talents = new Dictionary<StatType, int>();

        public HeroEquipment Equipment = new();
       
    }
}
