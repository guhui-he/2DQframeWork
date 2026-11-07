using System.Linq;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public partial class InputController : ViewController, IController
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private IBattleModel mModel;

        void Start() => mModel = this.GetModel<IBattleModel>();

        void Update()
        {
            if (this.GetSystem<IActionQueueSystem>().IsRunning) return;
            Debug.Log(11);
            if (Input.GetMouseButtonDown(1))
            {
                this.SendCommand(new CancelActionCommand());
                return;
            }

            if (mModel.Action.Value == ActionState.Moving)
                UpdateHoverPath();
            else
                this.GetSystem<IPathPreviewSystem>().ClearPathPreview();

            if (!Input.GetMouseButtonDown(0)) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            var col = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition);
            if (col == null) return;

            GridPos? clickedPos = null;
            int targetUnitId = -1;
            var unitView = col.GetComponent<BattleUnitView>();
            if (unitView != null)
            {
                targetUnitId = unitView.UnitId;
                var unitData = mModel.AllUnits.FirstOrDefault(u => u.UnitId == targetUnitId);
                if (unitData != null) clickedPos = unitData.Pos;
            }
            var blockView = col.GetComponent<block>();
            if (blockView != null) clickedPos = new GridPos(blockView.RowIndex, blockView.ColIndex);
            if (!clickedPos.HasValue) return;
            this.SendCommand(new BattleClickCommand(clickedPos.Value, targetUnitId));
        }

        private void UpdateHoverPath()
        {
            var col = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition);
            GridPos? hover = null;

            if (col != null)
            {
                var b = col.GetComponent<block>();
                if (b != null) hover = new GridPos(b.RowIndex, b.ColIndex);
            }

            var preview = this.GetSystem<IPathPreviewSystem>();
            if (hover.HasValue) preview.ShowPathPreview(hover.Value);
            else preview.ClearPathPreview();
        }
    }
}
