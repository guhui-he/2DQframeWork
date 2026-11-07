
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace Game
{
    public interface IRoleConfigModel : IModel
    {
        void AddRole(RoleConfigData data);
        RoleConfigData GetRole(int id);
        Dictionary<int, RoleConfigData> GetRoles();
    }
    public  class RoleConfigModel:AbstractModel, IRoleConfigModel
    {
        private readonly Dictionary<int,RoleConfigData> mRole
            = new();

        protected override void OnInit()
        {
            
        }

        public  void AddRole(RoleConfigData data)
        {
            mRole[data.Id] = data;
        }

        public RoleConfigData GetRole(int id)
        {
            if (mRole.TryGetValue(id, out var data))
                return data;
            return null;
        }

        public Dictionary<int, RoleConfigData> GetRoles()
        {
            return mRole;
        }
    }
}
  