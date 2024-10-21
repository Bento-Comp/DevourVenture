using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/EnhancedAnimatorController")]
	public class EnhancedAnimatorController : MonoBehaviour
	{
		public Animator animator;

		List<string> triggerToResetNames = new List<string>();

		public void SetTrigger(string name, bool resetOnNextFrame = false)
		{
			if(resetOnNextFrame)
				triggerToResetNames.Add(name);
			animator.SetTrigger(name);
		}

		public void SetBool(string name, bool value)
		{
			animator.SetBool(name, value);
		}

		public void SetFloat(string name, float value)
		{
			animator.SetFloat(name, value);
		}

		void LateUpdate()
		{
			foreach(string triggerToResetName in triggerToResetNames)
			{
				animator.ResetTrigger(triggerToResetName);
			}
			triggerToResetNames.Clear();
		}
	}
}