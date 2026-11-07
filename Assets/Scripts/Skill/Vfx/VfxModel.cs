using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IVfxModel : IModel
    {
        Dictionary<int,VfxConfig> vfxConfig{get;}
        void AddVfxConfig(VfxConfig vfxConfig);
        VfxConfig GetVfxConfig(int id);
    }

    public class VfxModel : AbstractModel, IVfxModel
    {
        public Dictionary<int, VfxConfig> vfxConfig { get; } = new Dictionary<int, VfxConfig>();

        public void AddVfxConfig(VfxConfig config)
        {
            vfxConfig[config.Id]=config;
        }

        public VfxConfig GetVfxConfig(int id)
        {
            if(vfxConfig.ContainsKey(id))
            {
                return vfxConfig[id];
            }
            return null;
        }

        protected override void OnInit()
        {
            
        }
    }
}
