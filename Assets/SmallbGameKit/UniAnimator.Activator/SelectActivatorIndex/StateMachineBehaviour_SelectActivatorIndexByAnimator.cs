using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/StateMachineBehaviour_SelectActivatorIndexByAnimator")]
	public class StateMachineBehaviour_SelectActivatorIndexByAnimator : StateMachineBehaviour
	{
		public int activatorIndex = 0;

		SelectActivatorIndexByAnimator selectActivatorIndexByAnimator;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);

			if(selectActivatorIndexByAnimator == null)
				selectActivatorIndexByAnimator = animator.GetComponent<SelectActivatorIndexByAnimator>();

			selectActivatorIndexByAnimator.SelectActivatorIndex(activatorIndex);
		}
	}
}
