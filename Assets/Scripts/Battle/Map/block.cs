using QFramework;
using UnityEngine;

namespace Game
{
    public partial class block : ViewController, IController
    {
        public int RowIndex;
        public int ColIndex;

        private static readonly Color sBfsColor = new Color(1f, 1f, 0f, 0.35f);
        private static readonly Color sAPathColor = new Color(0f, 1f, 0.3f, 0.55f);

        private void Awake()
        {
            this.RegisterEvent<BlockUIStateChangedEvent>(OnUIStateChanged)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnUIStateChanged(BlockUIStateChangedEvent e)
        {
            if (e.Row != RowIndex || e.Col != ColIndex) return;
            RefreshUI(e.Type);
        }

        private void RefreshUI(ShowPathType state)
        {
            switch (state)
            {
                case ShowPathType.None:
                    grid.enabled = false;
                    break;
                case ShowPathType._BFS:
                    grid.enabled = true;
                    grid.color = sBfsColor;
                    break;
                case ShowPathType._A:
                    grid.enabled = true;
                    grid.color = sAPathColor;
                    break;
                case ShowPathType.Attack:
                    grid.enabled = true;
                    grid.color = Color.red;
                    break;
            }
        }

        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}
