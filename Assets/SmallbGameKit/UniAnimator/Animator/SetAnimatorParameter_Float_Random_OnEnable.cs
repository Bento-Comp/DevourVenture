using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/SetAnimatorParameter_Float_Random_OnEnable")]
	public class SetAnimatorParameter_Float_Random_OnEnable : MonoBehaviour
	{
		public Animator animator;

		public string floatParameterName = "parameter";

		public float valueMin = 0.0f;

		public float valueMax = 0.0f;

		void OnEnable()
		{
			animator.SetFloat(floatParameterName, UnityEngine.Random.Range(valueMin, valueMax));
		}
	}
}