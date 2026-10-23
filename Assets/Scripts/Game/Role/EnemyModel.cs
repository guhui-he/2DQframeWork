using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEnemyModel : IModel
    {
        public Dictionary<int,List<RoleInfo>> Roles { get; }
        void AddEnemy(int enemyId, RoleInfo roleInfo);
        List<RoleInfo> GetRoleInfos(int enemyId);
    }
    public class EnemyModel : AbstractModel, IEnemyModel
    {
        public Dictionary<int , List<RoleInfo>> Roles { get; }=new ();
        protected override void OnInit()
        {
            
        }
        public void AddEnemy(int enemyId,RoleInfo roleInfo)
        {
            if (!Roles.TryGetValue(enemyId, out var list))
            {
                list = new List<RoleInfo>();
                Roles[enemyId] = list;
            }
            list.Add(roleInfo);
            Debug.Log(enemyId);
        }
        public List<RoleInfo> GetRoleInfos(int enemyId)
        {
            return Roles.TryGetValue(enemyId, out List<RoleInfo> roleInfos) ? roleInfos : null;
        }
    }
}
