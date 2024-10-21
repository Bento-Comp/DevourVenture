using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Animator")]
	public class ActivationController_Animator : ActivationControllerBase
	{
		public bool enableAnimatorOnActivation = true;

		public bool disableAnimatorOnDeactivation = false;

		public bool controlGameObjectActivation = true;

		public bool useActiveTrigger;

		public string activeTriggerName = "activate";

		public string deactiveTriggerName = "deactivate";

		public bool useEnableTime;

		public float enableTime;

		public bool useEnableTimeOnlyForFirstEnable;

		public Animator animator;

		bool firstEnable = true;

		protected override void OnSetFirstActiveState()
		{
			UpdateControlledObjectActivation();
			OnActiveChange();
		}

		public void DeactivateAnimationEnd()
		{
			NotifyRuntimeDeactivationEnd();
		}

		protected override void OnActiveChange()
		{
			if(Active)
			{
				UpdateControlledObjectActivation();

				if(useActiveTrigger && animator != null && animator.isActiveAndEnabled)
				{
					#if UNITY_EDITOR
					if(Application.isPlaying == false)
						return;
					#endif

					animator.ResetTrigger(deactiveTriggerName);
					animator.SetTrigger(activeTriggerName);
				}
			}
			else
			{
				if(FirstActive)
				{
					UpdateControlledObjectActivation();
				}
				else
				{
					if(animator != null && animator.isActiveAndEnabled)
					{
						#if UNITY_EDITOR
						if(Application.isPlaying == false)
							return;
						#endif

						animator.ResetTrigger(activeTriggerName);
						animator.SetTrigger(deactiveTriggerName);
					}
				}
			}
		}

		protected override void OnDeactivationEnd()
		{
			UpdateControlledObjectActivation();
		}

		#if UNITY_EDITOR
		protected override void OnEditorActiveChange()
		{
			UpdateControlledObjectActivation();
		}
		#endif

		void OnEnable()
		{
			if(useEnableTime)
			{
				if(firstEnable || useEnableTimeOnlyForFirstEnable == false)
				{
					firstEnable = false;

					animator.Play(0, 0, enableTime);
					animator.Update(0.0f);
				}
			}

			OnActiveChange();
		}

		void UpdateControlledObjectActivation()
		{
			if(animator != null)
			{
				bool active = Active;
				if(active && enableAnimatorOnActivation)
				{
					animator.enabled = true;
				}
				else if(active == false && disableAnimatorOnDeactivation)
				{
					animator.enabled = false;
				}
			}

			if(controlGameObjectActivation)
				gameObject.SetActive(Active);
		}
	}
}