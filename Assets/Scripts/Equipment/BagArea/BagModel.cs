// BagModel.cs
using QFramework;

namespace Game
{
    public interface IBagModel : IItemContainerModel
    {

    }

    public class BagModel : ItemContainerModel, IBagModel
    {
        protected override AreaType ContainerArea => AreaType.Bag;
    }
}
