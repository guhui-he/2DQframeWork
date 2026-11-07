using QFramework;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Game
{
	public partial class Enemy : BattleUnitView
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
