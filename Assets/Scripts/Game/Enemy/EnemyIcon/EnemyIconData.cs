using UnityEngine;
using QFramework;

namespace Game
{
    public  class EnemyIconData
    {
        /// <summary>
        /// 队伍实例id
        /// </summary>
        public int Id;
        /// <summary>敌人类型 id（指向 Enemy 配置表）</summary>
        public int EnemyId;
        public int SpawnId;     
        public Vector3 BornPosition;
        public float CreatTimer = 20f;

        public bool HasSpawnedInitialMembers = false;
        public BindableProperty<Vector3> TargetPosition { get; } = new BindableProperty<Vector3>(Vector3.zero);
        public BindableProperty<Vector3> CurrentPosition { get; } = new BindableProperty<Vector3>(Vector3.zero);
        public BindableProperty<PerceptionType> PerceptionResult { get; } = new BindableProperty<PerceptionType>(PerceptionType.None);
        public BindableProperty<Vector3> LastKnownPlayerPos { get; } = new BindableProperty<Vector3>(Vector3.zero);
        public float MoveSpeed;

        public EnemyIconData(int id, int enemyId, int spawnId, Vector3 bornPos, float speed)
        {
            Id = id;
            EnemyId = enemyId;
            SpawnId = spawnId;
            BornPosition = bornPos;
            MoveSpeed = speed;

            TargetPosition.Value = bornPos;
            CurrentPosition.Value = bornPos;
            LastKnownPlayerPos.Value = bornPos;
        }
    }
}
