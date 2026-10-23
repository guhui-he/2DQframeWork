using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEnterTownSystem : ISystem
    {
        public void EnterTown(int buildId);
    }
    internal class EnterTownSystem : AbstractSystem, IEnterTownSystem
    {
        protected override void OnInit()
        {
            
        }
        public void EnterTown(int buildId)
        {
            this.SendEvent(new TownEnterEvent(buildId));
        }
    }
}
