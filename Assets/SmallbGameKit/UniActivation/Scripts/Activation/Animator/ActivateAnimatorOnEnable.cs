using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivateAnimatorOnEnable")]
	public class ActivateAnimatorOnEnable : MonoBehaviour
	{
		public Animator animator;

		void OnEnable()
		{
			if(animator == null)
				animator = GetComponent<Animator>();

			animator.enabled = true;
		}
	}
}