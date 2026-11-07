using System.Collections.Generic;
using Game;
using QFramework;
using UnityEngine;

public interface ISkillModel : IModel
{
    void AddSkill(SkillInfo skill);
    SkillInfo GetSkill(int id);
    List<SkillInfo> GetSkillInfos(List<int> ids);
}

public class SkillModel : AbstractModel, ISkillModel
{
    private readonly Dictionary<int, SkillInfo> mSkills = new();

    public void AddSkill(SkillInfo skill)
    {
        mSkills[skill.Config.id] = skill;
    }

    public SkillInfo GetSkill(int id)
    {
        return mSkills.TryGetValue(id, out var skill) ? skill : null;
    }
    public List<SkillInfo> GetSkillInfos(List<int> ids)
    {
        var list = new List<SkillInfo>();
        for (int i = 0; i < ids.Count; i++)
        {
            if (mSkills.TryGetValue(ids[i], out var templateSkill))
            {
                list.Add(new SkillInfo(templateSkill.Config));
            }
        }
        return list;
    }
    protected override void OnInit() { }
}
