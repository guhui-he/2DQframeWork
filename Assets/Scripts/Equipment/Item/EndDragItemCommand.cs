using QFramework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class EndDragItemCommand : AbstractCommand
    {
        private readonly DropTargetInfo mTarget;
        public EndDragItemCommand(DropTargetInfo target) => mTarget = target;

        protected override void OnExecute()
        {
            var dragModel = this.GetModel<IDragModel>();
            var item = dragModel.Dragging.Value;
            var sourceArea = dragModel.SourceArea.Value;
            //int sourceId = dragModel.SourceId.Value;

            // 清理拖拽状态
            dragModel.Dragging.Value = null;

            if (item == null) return;
            if (!mTarget.IsValid) return;
            Debug.Log("ddd");
            this.GetSystem<IDragDropRouterSystem>()
                .Route(item, sourceArea, mTarget);

            this.SendEvent(new DragEndedEvent());
        }
    }
}
