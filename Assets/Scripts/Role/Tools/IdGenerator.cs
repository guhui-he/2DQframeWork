using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class IdGenerator
    {
        private static int mCounter = 0;

        public static int Next()
        {
            return ++mCounter;
        }

        /// <summary>存档时记录当前值，读档时恢复</summary>
        public static void SetCounter(int value)
        {
            mCounter = value;
        }

        public static int GetCounter() => mCounter;
    }
}
