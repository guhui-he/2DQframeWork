using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public enum SettlementType { Nest, City, Wilderness }

    public class SettlementInfo
    {
        public int Id;
        public SettlementType Type;
        public Vector2Int Pos;
        public int RegionId;  
        public int BuildId;   
        public int Level = 1;
        public float SpawnTimer = 5f;
        public float SpawnInterval = 20f;
        public CityConfig config;
    }
    public class RegionConfig
    {
        public int RegionId;
        public string RegionName;
        public List<int> NestPool=new List<int>();
        public int MaxNestCount;
    }
}
