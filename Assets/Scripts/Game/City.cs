using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public partial class City : ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        public int CityId;
        public Tilemap groundTilemap;

        void Start()
        {
            Vector3Int cellPos = groundTilemap.WorldToCell(transform.position);
            transform.position = groundTilemap.GetCellCenterWorld(cellPos);
            Debug.Log(cellPos.ToString());
            this.GetModel<ISettlementModel>().AddSettlement(new SettlementInfo
            {
                Id = CityId,
                Type = SettlementType.City,
                Pos = new Vector2Int(cellPos.x, cellPos.y),
                SpawnInterval = 30f,
                BuildId=CityId,
                config=this.GetModel<ICityConfigModel>().GetCityConfig(CityId)
            });
        }
    }
}
