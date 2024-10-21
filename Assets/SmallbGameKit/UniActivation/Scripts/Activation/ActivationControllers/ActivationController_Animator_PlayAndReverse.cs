using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationController_Animator_PlayAndReverse")]
	public class ActivationController_Animator_PlayAndReverse : ActivationControllerBase
	{
		public bool controlGameObjectActivation = true;

		public Animator animator;

		public bool reverse;

		public float speed = 1.0f;

		public string speedParameterName = "speed";

		public string stateName = "";

		public float enableTime;

		public bool useEnableTimeOnlyForFirstEnable;

		bool firstPlay = true;

		bool firstEnable = true;

		protected override void OnSetFirstActiveState()
		{
			UpdateControlledObjectActivation();
		}

		protected override void OnActiveChange()
		{
			if(Active)
			{
				UpdateControlledObjectActivation();
				Play();
			}
			else
			{
				PlayReverse();
			}
		}

		float CurrentNormalizedTime
		{
			get
			{
				float time = CurrentNormalizedTime_Raw;

				if(reverse)
				{
					time = 1.0f - time;
				}

				return time;
			}
		}

		float CurrentNormalizedTime_Raw
		{
			get
			{
				if(animator.isInitialized == false || animator.isActiveAndEnabled == false)
					return 0.0f;
				
				return Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
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
			if(firstEnable || useEnableTimeOnlyForFirstEnable == false)
			{
				firstEnable = false;

				animator.Play(0, 0, enableTime);
				animator.SetFloat(speedParameterName, 0.0f);
				animator.Update(0.0f);
			}

			firstPlay = true;

			//UpdateControlledObjectActivation();
			OnActiveChange();
		}

		void UpdateControlledObjectActivation()
		{
			if(animator != null)
				animator.enabled = Active;

			if(controlGameObjectActivation)
				gameObject.SetActive(Active);
		}

		void LateUpdate()
		{
			if(Active == false && CurrentNormalizedTime <= 0.0f)
				NotifyRuntimeDeactivationEnd();

			if(ActivationInProgress && CurrentNormalizedTime >= 1.0f)
				NotifyRuntimeActivationEnd();
		}

		void Play()
		{
			if(animator.isInitialized == false || animator.isActiveAndEnabled == false)
				return;

			PlayState(1.0f);
		}

		void PlayReverse()
		{
			if(animator.isInitialized == false || animator.isActiveAndEnabled == false)
				return;
			
			PlayState(-1.0f);
		}

		void PlayState(float direction)
		{
			if(animator.isInitialized == false || animator.isActiveAndEnabled == false)
				return;

			if(reverse)
			{
				direction *= -1.0f;
			}

			float time;
			if(firstPlay)
			{
				if(direction >= 0.0f)
				{
					time = 0.0f;
				}
				else
				{
					time = 1.0f;
				}
				firstPlay = false;
			}
			else
			{
				time = CurrentNormalizedTime_Raw;
			}

			if(stateName == "")
			{
				animator.Play(0, 0, time);
			}
			else
			{
				animator.Play(stateName, 0, time);
			}
				
			animator.SetFloat(speedParameterName, direction * speed);
		}
	}
}