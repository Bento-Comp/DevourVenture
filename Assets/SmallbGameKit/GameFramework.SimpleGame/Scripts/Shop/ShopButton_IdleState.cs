using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ShopButton_IdleState")]
	public class ShopButton_IdleState : StateMachineBehaviour 
	{
		public float firstIdleDuration = 1.0f;

		public float nextIdleDuration = 3.0f;

		public string idleEndTrigger = "jiggle";

		bool firstIdle = true;

		float remainingTime;

		public float IdleDuration
		{
			get
			{
				if(firstIdle)
				{
					return firstIdleDuration;
				}
				else
				{
					return nextIdleDuration;
				}
			}
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			remainingTime = IdleDuration;
			firstIdle = false;
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);

			remainingTime -= Time.deltaTime;

			if(remainingTime <= 0.0f)
				animator.SetTrigger(idleEndTrigger);
		}
	}
}