using QFramework;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Game
{
	public partial class Enemy : BattleUnitView
	{
        //protected override void OnAttacked()
        //{
        //    body.Play("Attack");
        //}
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
