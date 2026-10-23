using QFramework;

namespace Game
{
    public class BeginDragItemCommand : AbstractCommand
    {
        private readonly ItemInfo mItem;
        private readonly AreaType mSourceArea;
        private readonly int mSourceId;

        public BeginDragItemCommand(ItemInfo item, AreaType sourceArea)
        {
            mItem = item;
            mSourceArea = sourceArea;
            //mSourceId = sourceId;
        }

        protected override void OnExecute()
        {
            var dragModel = this.GetModel<IDragModel>();
            dragModel.Dragging.Value = mItem;
            dragModel.SourceArea.Value = mSourceArea;
            //dragModel.SourceId.Value = mSourceId;

            this.SendEvent(new DragStartedEvent(mItem));
        }
    }
}
