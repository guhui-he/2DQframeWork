
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class Hero : BattleUnitView
    {

        protected override void OnDamaged(int damage)
        {
            body.color = Color.red;
            
        }
        protected override void OnDamagedCompleted()
        {
            body.color = Color.white;
           
        }
        protected override void OnDie()
        {
            ani.Play("die");
            Destroy(gameObject, 1.0f);
        }
    }
}
