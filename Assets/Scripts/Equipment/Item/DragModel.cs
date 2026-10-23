using QFramework;

namespace Game
{
    public interface IDragModel : IModel
    {
        BindableProperty<ItemInfo> Dragging { get; }
        BindableProperty<AreaType> SourceArea { get; }
        //BindableProperty<int> SourceId { get; }   // 源所属 Id（商店 cityId、任务 id 等）
    }
    public class DragModel : AbstractModel, IDragModel
    {
        public BindableProperty<ItemInfo> Dragging { get; } = new BindableProperty<ItemInfo>(null);
        public BindableProperty<AreaType> SourceArea { get; } = new BindableProperty<AreaType>(AreaType.Bag);
        //public BindableProperty<int> SourceId { get; } = new BindableProperty<int>(0);

        protected override void OnInit() { }
    }
}
