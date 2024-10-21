using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/ForceAnimatorUpdateOnEnable")]
	public class ForceAnimatorUpdateOnEnable : MonoBehaviour
	{
		Animator animator;
		void Awake()
		{
			animator = GetComponent<Animator>();
		}

		void OnEnable()
		{
			animator.Update(0.0f);
		}
	}
}