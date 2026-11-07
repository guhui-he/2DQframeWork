
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class Hero : BattleUnitView
    {

        protected override void OnDamagedCompleted()
        {
           
        }
        protected override void OnDie()
        {
            ani.AnimationState.SetAnimation(0, "Die", true);
            Destroy(gameObject, 1.0f);
        }
    }
}
