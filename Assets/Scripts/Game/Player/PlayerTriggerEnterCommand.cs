
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPlayerTriggerEnterCommand : ICommand
    {

    }
    public class PlayerTriggerEnterCommand : AbstractCommand, IPlayerTriggerEnterCommand
    {
        private readonly Collider2D mOther;
        public PlayerTriggerEnterCommand(Collider2D other)
        {
            mOther = other;
        }
        protected override void OnExecute()
        {
            if (mOther.CompareTag("Build"))
            {               
                this.GetSystem<IBuildTriggerSystem>().PlayerEnterBuild(1002);
            }
            else if (mOther.CompareTag("Enemy"))
            {
                this.GetSystem<IEnemyTriggerSystem>().PlayerEnterEnemy(mOther);
            }
        }
    }
}
