using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum GamePlayEventType { AcceptQuest,GiveItem,RecruitPartner,OpenPanel}
    public class GamePlayEventConfig
    {
        public int Id;
        public GamePlayEventType type;
        public int Value1;
        public int Value2;
        public string Des;
    }
}
