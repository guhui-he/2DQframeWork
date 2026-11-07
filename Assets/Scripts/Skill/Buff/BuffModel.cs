using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using Unity.VisualScripting.FullSerializer;

namespace Game
{
    public interface IBuffModel : IModel
    {
        Dictionary<int,BuffConfig> Config { get; }

        void AddBuffConfig(BuffConfig config);
        BuffConfig GetBuffConfig(int id);
    }

    public class BuffModel : AbstractModel, IBuffModel
    {
        public Dictionary<int, BuffConfig> Config { get; }=new Dictionary<int, BuffConfig>();

        public void AddBuffConfig(BuffConfig config)
        {
            Config[config.Id] = config;
        }

        public BuffConfig GetBuffConfig(int id)
        {
            if (Config.ContainsKey(id))
            {
                return Config[id];
            }
            return null;
        }

        protected override void OnInit()
        {
            
        }
    }
}
