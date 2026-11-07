using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

namespace Game
{
    public interface IGamePlayEventModel:IModel
    {
        Dictionary<int,GamePlayEventConfig> mConfig { get; }
        void AddConfig(GamePlayEventConfig config);
        GamePlayEventConfig GetConfig(int id);
    }
    public class GamePlayEventModel : AbstractModel, IGamePlayEventModel
    {
        public Dictionary<int, GamePlayEventConfig> mConfig { get; }=new Dictionary<int, GamePlayEventConfig>();
        public void AddConfig(GamePlayEventConfig config)
        {
            mConfig[config.Id]=config;
        }

        public GamePlayEventConfig GetConfig(int id)
        {
            if(mConfig.ContainsKey(id)) return mConfig[id];
            return null;
        }

        protected override void OnInit() { }
    }
}
