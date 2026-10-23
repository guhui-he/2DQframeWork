using UnityEngine;
using QFramework;
using System.Linq;

namespace Game
{
    public partial class InputController : ViewController, IController, ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface;
        private IBattleModel mModel;
        private GridPos? mLastHoverPos;

        void Start() => mModel = this.GetModel<IBattleModel>();

        void Update()
        {
            if (this.GetSystem<IActionQueueSystem>().IsRunning) return;
            //撤回按钮
            if (Input.GetMouseButtonDown(1))
            {
                this.SendCommand(new CancelActionCommand());
                return;
            }
            //显隐A星路径
            if (mModel.Action.Value == ActionState.Moving) UpdateHoverPath();
            else if (mLastHoverPos.HasValue)
            {
                this.GetSystem<IPathPreviewSystem>().ClearPathPreview();
                mLastHoverPos = null;
            }


            if (!Input.GetMouseButtonDown(0)) return;
            var col = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition);
            if (col == null) return;
            var unitView = col.GetComponent<BattleUnitView>();
            if (unitView != null)
                this.SendEvent(new ShowRoleInfoEvent
                {
                    Unit = mModel.AllUnits.FirstOrDefault(u => u.UnitId == unitView.UnitId)
                });
            var blockView = col.GetComponent<block>();
            GridPos? pos = blockView != null ? new GridPos(blockView.RowIndex, blockView.ColIndex) : (GridPos?)null;



            switch (mModel.Action.Value)
            {
                case ActionState.WaitInput:
                    if (unitView != null)
                    {
                        var unit = mModel.AllUnits.FirstOrDefault(u => u.UnitId == unitView.UnitId);
                        if (unit != null && unit.IsPlayer) this.SendCommand(new SelectUnitCommand(unit));
                    }
                    break;

                case ActionState.Moving:
                    var selected = mModel.SelectedUnit.Value;
                    if (selected == null) return;

                    if (unitView != null && unitView.UnitId == selected.UnitId)
                    {
                        // 点自己 = 原地待机
                        this.SendCommand(new MoveCommand(selected.Pos));
                    }
                    else if (pos.HasValue)
                    {
                        this.SendCommand(new MoveCommand(pos.Value));
                    }
                    // 点空白：什么也不做（避免 NPE）
                    break;

                case ActionState.Attacking:
                    if (unitView != null)
                    {
                        var target = mModel.AllUnits.FirstOrDefault(u => u.UnitId == unitView.UnitId);
                        if (target != null && !target.IsPlayer) this.SendCommand(new AttackCommand(target));
                    }
                    else this.SendCommand(new CancelActionCommand());
                    break;
            }
        }

        private void UpdateHoverPath()
        {
            var col = Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition);
            GridPos? hover = null;

            if (col != null && col.GetComponent<block>() != null)
            {
                var b = col.GetComponent<block>();
                hover = new GridPos(b.RowIndex, b.ColIndex);
            }

            if (mLastHoverPos.HasValue && hover.HasValue && mLastHoverPos.Value.Equals(hover.Value)) return;
            mLastHoverPos = hover;

            var preview = this.GetSystem<IPathPreviewSystem>();
            if (hover.HasValue) preview.ShowPathPreview(hover.Value);
            else preview.ClearPathPreview();
        }
    }
}
