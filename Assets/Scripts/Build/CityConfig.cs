using System.Collections.Generic;
using QFramework;

namespace Game
{
    public  class CityConfig
    {
        public int Id;
        public string Name;
        public string Des;
        public string SceneName;
        public List<int> RolePool = new List<int>();
        public List<int> PartyPool=new List<int>();
        public int PartyNum;
        public Faction Faction;
    }
}
