using Game;

public class MonsterBehavior : BaseCombatBehavior
{
    public override void Update(PartyInfo party, float dt)
    {
        if (party.State == PartyState.Chase) return;

        var perceived = GameApp.Interface.GetSystem<IPerceptionSystem>().ScanNearbyTargets(party, 8f);
        var enemy = FindClosestEnemy(party, perceived);

        if (enemy != null)
        {
            party.State = PartyState.Chase;
            ChaseTarget(party, enemy, dt);
        }
        else
        {
            WanderAround(party, dt, false);
        }
    }
}
