using System.Collections.Generic;

namespace Game
{
    public enum SkillCastMode { Normal, Laser, Charge }
    public enum SkillTargetType { Single, Self, Area, Line }
    public enum SkillTargetCategory
    {
        Enemy,     
        Ally,      
        Self,     
        Ground,    
        All        
    }

    public class SkillConfig
    {
        public int id;
        public string name;
        public string icon;
        public SkillCastMode CastMode;
        public SkillTargetType targetType;
        public SkillTargetCategory targetCategory;
        public int cooldown;
        public int atkRange;
        public int maxLevel;

        public float DamagePercent; // 伤害百分比
        public int CastVfxId;
        public int ProjectileVfxId;
        public int HitVfxId;

        public string des; 
        public List<EffectConfig> Effects = new List<EffectConfig>();
    }
}
