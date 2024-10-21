using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/Animator/ResetTransformToInitialValuesOnDisable")]
	public class ResetTransformToInitialValuesOnDisable : MonoBehaviour
	{
		Vector3 localPosition;
		Quaternion localRotation;
		Vector3 localScale;

		void Awake()
		{
			localPosition = transform.localPosition;
			localRotation = transform.localRotation;
			localScale = transform.localScale;
		}

		void OnDisable()
		{
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			transform.localScale = localScale;
		}
	}
}