using QFramework;

namespace Game
{
    internal class EnterCityCommand : AbstractCommand
    {
        private readonly int mBuildId;
        public EnterCityCommand(int buildId) => mBuildId = buildId;

        protected override void OnExecute()
        {
            this.GetSystem<IEnterTownSystem>().EnterTown(mBuildId);
        }
    }
}
