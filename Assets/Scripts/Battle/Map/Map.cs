using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public partial class Map : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;

        void Start()
        {
            var result = MapDataExtractor.Extract(Ground, Wall);
            GenerateMapViews(result.rowCount, result.colCount);
            this.SendEvent(new MapEnterEvent(result.rowCount, result.colCount, result.data));
        }

        private void GenerateMapViews(int rowCount, int colCount)
        {
            int count = 0;
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    Vector3Int cellPos = new Vector3Int(
                        c + Ground.cellBounds.xMin,
                        r + Ground.cellBounds.yMin, 0);

                    var go = Instantiate(block, transform);
                    go.transform.position = Ground.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
                    go.SetActive(true);

                    var view = go.GetComponent<block>();
                    view.RowIndex = r;
                    view.ColIndex = c;

                    BlockViewRegistry.Register(r, c, view.transform);
                    count++;
                }
            }
        }

        private void OnDestroy() => BlockViewRegistry.Clear();
    }
}

