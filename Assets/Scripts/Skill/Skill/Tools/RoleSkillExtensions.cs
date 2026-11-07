

namespace Game
{ 
    public static class RoleSkillExtensions
    {
        public static bool HasLearnedSkill(this RoleInfo role, int skillId)
        {
            return role.mSkillIds.Contains(skillId);
        }
        public static bool CanLearnSkill(this RoleInfo role, SkillConfig config)
        {
            if (role.mSkillIds.Contains(config.id)) return false; // 已学
            //if (role.Level < config.RequiredLevel) return false; // 等级不够
            if (role.SkillPoints.Value <= 0) return false; // 点数不够
            return true;
        }
    }
}
