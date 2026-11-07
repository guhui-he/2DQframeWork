using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRelationConfigModel : IModel
    {
        void AddConfig(FactionRelationConfig relationConfig);
        List<FactionRelationConfig> Relations { get; }
    }

    public class RelationConfigModel : AbstractModel, IRelationConfigModel
    {
        public List<FactionRelationConfig> Relations { get; } = new();

        public void AddConfig(FactionRelationConfig relationConfig)
        {
           Relations.Add(relationConfig);
        }

        protected override void OnInit()
        {
        }
    }
}
