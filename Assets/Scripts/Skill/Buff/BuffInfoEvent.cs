using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public struct OpenBuffInfoPanelEvent
    {
        public BuffInfo info;
        public OpenBuffInfoPanelEvent(BuffInfo info)
        {
            this.info = info;
        }
    }
    public struct HideBuffInfoPanelEvent
    { }

}
