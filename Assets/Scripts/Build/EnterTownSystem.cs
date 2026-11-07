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
            var model=this.GetModel<IPlayerModel>();
            model.CurrentCityId.Value= buildId;
        }
    }
}
