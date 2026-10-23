using QFramework;

namespace Game
{
    internal class EnterTownCommand : AbstractCommand
    {
        private readonly int mBuildId;
        public EnterTownCommand(int buildId) => mBuildId = buildId;

        protected override void OnExecute()
        {
            this.GetSystem<IEnterTownSystem>().EnterTown(mBuildId);
        }
    }
}
