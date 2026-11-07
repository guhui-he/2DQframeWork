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
        public string Ani;
        // 属性
        public int CurHp;
        public List<StatAffix> stat = new();
        public bool isBuy;
        // 成长
        public int Level;
        public int Curexp;
        public BindableProperty<int> PendingPoints { get; } = new();
        public BindableProperty<int> SkillPoints { get; } = new();
        // 天赋星级
        public Dictionary<AffixStatType, int> Talents = new Dictionary<AffixStatType, int>();
        // 装备
        public HeroEquipment Equipment = new();
        // 技能
        public List<int> mSkillIds = new();
    }
}
