

namespace Game
{
    public interface ILootModel : IItemContainerModel
    {

    }
    public class LootModel : ItemContainerModel, ILootModel
    {
        protected override AreaType ContainerArea =>AreaType.Loot;
    }
}
