using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace Game
{ 
    public interface IPartyConfigModel:IModel
    {
        void AddConfig(PartyConfig config);
        PartyConfig GetConfig(int id);
    }
    internal class PartyConfigModel : AbstractModel, IPartyConfigModel
    {
        private readonly Dictionary<int, PartyConfig> mParty = new Dictionary<int, PartyConfig>();
        public void AddConfig(PartyConfig config)
        {
            mParty[config.Id] = config;
        }

        public PartyConfig GetConfig(int id)
        {
            if(mParty.ContainsKey(id))
                return mParty[id];
            return null;
        }

        protected override void OnInit() { }
    }
}
