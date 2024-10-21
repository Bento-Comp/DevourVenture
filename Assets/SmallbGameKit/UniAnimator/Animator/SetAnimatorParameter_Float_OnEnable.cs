using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/SetAnimatorParameter_Float_OnEnable")]
	public class SetAnimatorParameter_Float_OnEnable : MonoBehaviour
	{
		public Animator animator;

		public string floatParameterName = "parameter";

		public float value = 0.0f;

		void OnEnable()
		{
			animator.SetFloat(floatParameterName, value);
		}
	}
}