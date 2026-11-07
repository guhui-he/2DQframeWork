using System.Linq;
using Game;

public class GuardBehavior : BaseCombatBehavior
{
    public override void Update(PartyInfo party, float dt)
    {
        if (party.State == PartyState.Chase) return;

        var perceived = GameApp.Interface.GetSystem<IPerceptionSystem>().ScanNearbyTargets(party, 10f); // 卫队视野更广

        var enemy = FindClosestEnemy(party, perceived);

        if (enemy != null)
        {
            party.State = PartyState.Chase;
            ChaseTarget(party, enemy, dt);
        }
        else
        {
            var friendlyCity = perceived.FirstOrDefault(t =>
                t.Category == TargetCategory.Settlement &&
                !RelationModel.IsHostile(party.Faction, t.Faction));

            if (friendlyCity != null && friendlyCity.Distance < 3f)
            {
                // 靠近己方城市，可能回城休息
            }
            else
            {
                WanderAround(party, dt, true);
            }
        }
    }
}
