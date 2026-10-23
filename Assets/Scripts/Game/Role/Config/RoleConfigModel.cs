
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace Game
{
    public enum RoleType
    {
        Hero,
        Enemy
    }

    public interface IRoleConfigModel : IModel
    {
        void AddRole(RoleType type, RoleConfigData data);
        RoleConfigData GetRole(RoleType type, int id);
        Dictionary<int, RoleConfigData> GetRoles(RoleType type);
    }
    public  class RoleConfigModel:AbstractModel, IRoleConfigModel
    {
        private readonly Dictionary<RoleType, Dictionary<int,RoleConfigData>> mRole
            = new();

        protected override void OnInit()
        {
            
        }

        public  void AddRole(RoleType type, RoleConfigData data)
        {
            if (!mRole.TryGetValue(type, out var dic))
            {
                dic = new Dictionary<int, RoleConfigData>();
                mRole[type] = dic;
            }
            dic[data.Id]=data;
        }

        public RoleConfigData GetRole(RoleType type,int id)
        {
            if (mRole.TryGetValue(type, out var dic) && dic.TryGetValue(id, out var data))
                return data;
            return null;
        }

        public Dictionary<int, RoleConfigData> GetRoles(RoleType type)
        {
            return (mRole.TryGetValue(type,out var dic)) ? dic : null;
        }
    }
}
  