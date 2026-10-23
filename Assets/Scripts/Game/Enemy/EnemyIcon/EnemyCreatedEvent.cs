namespace Game
{
    public struct EnemyCreatedEvent
    {
        public int EnemyId;
        public EnemyCreatedEvent(int id) => EnemyId = id;
    }

    public struct EnemyRemovedEvent
    {
        public int EnemyId;
        public EnemyRemovedEvent(int id) => EnemyId = id;
    }
    public struct SpawnEnemyTriggerEvent { public int SpawnId; public SpawnEnemyTriggerEvent(int id) => SpawnId = id; }
}
