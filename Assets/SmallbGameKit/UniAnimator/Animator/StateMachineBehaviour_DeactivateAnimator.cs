using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/StateMachineBehaviour_DeactivateAnimator")]
	public class StateMachineBehaviour_DeactivateAnimator : StateMachineBehaviour
	{
		public bool forceUpdateBeforeDeactivation;

		bool forceUpdate;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
		{
			if(forceUpdate)
				return;
			
			if(forceUpdateBeforeDeactivation)
			{
				forceUpdate = true;
				animator.Update(0.0f);
				forceUpdate = false;
			}
			
			animator.enabled = false;
		}
	}
}