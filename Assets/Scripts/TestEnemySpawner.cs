using UnityEngine;
using QFramework;

namespace Game
{
    public class TestEnemySpawner : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        public GameObject EnemyPrefab;
        public int SpawnRow = 5;
        public int SpawnCol = 5;

        void Start()
        {
            ActionKit.DelayFrame(1, () => {
                var pos = new GridPos(SpawnRow, SpawnCol);
                var fakeRoleInfo = this.GetSystem<IRoleFactorySystem>().CreateRole(RoleType.Enemy);

                this.SendCommand(new SpawnUnitCommand(fakeRoleInfo, pos, false));

                var unitData = this.GetModel<IBattleModel>().GetUnitAt(pos);
                if (unitData == null) return;

                var obj = Instantiate(EnemyPrefab);
                if (BlockViewRegistry.TryGetWorldPosition(pos, out var wp))
                    obj.transform.position = new Vector3(wp.x, wp.y, -1);

                obj.GetComponent<BattleUnitView>().UnitId = unitData.UnitId;
            }).StartGlobal();
        }
    }
}
