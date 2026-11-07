using System.Collections.Generic;

namespace Game
{
    // 播放角色身上的特效（施法、命中）
    public struct PlayVfxEvent
    {
        public int VfxId;
        public int TargetId; // 如果挂在角色身上，传角色ID；如果是地面，传GridPos
        public GridPos TargetPos;
    }
    public struct SpawnProjectileEvent
    {
        public int VfxId;
        public int FromId;
        public int Range;
        public GridPos ToPos;
    }
}
