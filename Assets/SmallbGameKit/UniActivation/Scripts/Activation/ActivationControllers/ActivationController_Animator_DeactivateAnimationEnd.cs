using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Animator_DeactivateAnimationEnd")]
	public class ActivationController_Animator_DeactivateAnimationEnd : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			ActivationController_Animator controller = animator.GetComponent<ActivationController_Animator>();
			if(controller != null)
				controller.DeactivateAnimationEnd();
		}
	}
}