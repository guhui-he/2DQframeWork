
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPlayerTriggerExitCommand : ICommand { }
    public class PlayerTriggerExitCommand : AbstractCommand, IPlayerTriggerExitCommand
    {
        private Collider2D mOther;

        public PlayerTriggerExitCommand(Collider2D other)
        {
            mOther = other;
        }
        protected override void OnExecute()
        {
            if (mOther.CompareTag("Build"))
            {
                this.GetSystem<IBuildTriggerSystem>().PlayerExitBuild(1);
            }
            else if (mOther.CompareTag("Enemy"))
            {
                this.GetSystem<IEnemyTriggerSystem>().PlayerExitEnemy(mOther);
            }
        }
    }
}
