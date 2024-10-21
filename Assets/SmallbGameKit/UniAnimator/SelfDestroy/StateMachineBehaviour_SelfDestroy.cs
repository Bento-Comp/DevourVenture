using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/StateMachineBehaviour_SelfDestroy")]
	public class StateMachineBehaviour_SelfDestroy : StateMachineBehaviour
	{
		SelfDestroyByAnimator selfDestroy;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);

			if(selfDestroy == null)
				selfDestroy = animator.GetComponent<SelfDestroyByAnimator>();

			selfDestroy.StartSelfDestroy();
		}
	}
}
