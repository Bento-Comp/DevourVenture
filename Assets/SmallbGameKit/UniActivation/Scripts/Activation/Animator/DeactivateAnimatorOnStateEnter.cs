using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/DeactivateAnimatorOnStateEnter")]
	public class DeactivateAnimatorOnStateEnter : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			animator.Update(0.0f);
			animator.enabled = false;
		}
	}
}