using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum AttitudeType
    {
        Neutral,  // 中立
        Friendly, // 友好（会给玩家发任务，不会主动攻击）
        Hostile   // 敌对（会主动攻击）
    }
    public class FactionRelationConfig
    {
        public int Id;
        public Faction FactionA;
        public Faction FactionB;
        public AttitudeType AttitudeType;
    }
}
