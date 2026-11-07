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
                var fakeRoleInfo = this.GetSystem<IRoleFactorySystem>().CreateRole();

                this.SendCommand(new SpawnUnitCommand(fakeRoleInfo, pos, false));
            }).StartGlobal();
            ActionKit.DelayFrame(1, () => {
                var pos = new GridPos(SpawnRow+1, SpawnCol);
                var fakeRoleInfo = this.GetSystem<IRoleFactorySystem>().CreateRole();

                this.SendCommand(new SpawnUnitCommand(fakeRoleInfo, pos, false));
            }).StartGlobal();
        }
    }
}
