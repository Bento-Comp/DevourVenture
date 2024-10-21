using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Animator_ControlEnable")]
	public class ActivationController_Animator_ControlEnable : ActivationControllerBase
	{
		public Animator animator;

		public bool resetOnDisable;

		public string resetState;

		protected override void OnSetFirstActiveState()
		{
			OnActiveChange();
		}

		protected override void OnActiveChange()
		{
			if(Active == false && resetOnDisable)
			{
				animator.Play(resetState, 0, 0.0f);
				animator.Update(0.0f);
			}

			UpdateActivation();
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			if(animator != null)
				UpdateActivation();
		}
		#endif

		void UpdateActivation()
		{
			animator.enabled = Active;
		}
	}
}