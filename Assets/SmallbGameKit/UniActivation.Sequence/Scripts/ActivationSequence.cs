using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniActivation
{
	[ExecuteAlways]
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("UniActivation/ActivationSequence")]
	public class ActivationSequence : MonoBehaviour
	{
		[SerializeField]
		int selectedIndex;

		public List<ActivationStep> steps = new List<ActivationStep>();

		public bool useStartIndex = true;
		public int startIndex = 0;

		[SerializeField]
		ActivationStep currentStep;

		public int SelectedIndex
		{
			get => selectedIndex;

			set
			{
				int clampedValue = ClampIndex(value);
				if(clampedValue != selectedIndex)
				{
					selectedIndex = clampedValue;
					UpdateActivation();
				}
			}
		}

		int CurrentActiveStepCount
		{
			get
			{
				int count = 0;
				foreach(ActivationStep step in steps)
				{
					if(IsStepActive(step) == false)
						continue;

					++count;
				}

				return count;
			}
		}

		public ActivationStep CurrentStep => currentStep;

		public void NotifyStepEnable()
		{
			InitializeActivation();
		}

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			if(useStartIndex)
			{
				selectedIndex = startIndex;
			}

			InitializeActivation();
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			InitializeActivation();
		}
		#endif

		void SetSequenceOwner()
		{
			foreach(ActivationStep step in steps)
				step.SetSequenceOwner(this);
		}

		bool IsStepActive(ActivationStep step)
		{
			if(step == null || step.mute || step.isActiveAndEnabled == false)
				return false;

			return true;
		}

		void InitializeActivation()
		{
			SetSequenceOwner();

			selectedIndex = ClampIndex(selectedIndex);

			UpdateActivation(true);
		}

		void UpdateActivation()
		{
			UpdateActivation(false);
		}

		void UpdateActivation(bool setFirstActiveState)
		{
			int activeStepIndex = 0;
			for(int i = 0; i < steps.Count; ++i)
			{
				ActivationStep step = steps[i];

				if(IsStepActive(step) == false)
					continue;

				if(activeStepIndex == selectedIndex)
					currentStep = step;

				if(setFirstActiveState)
				{
					step.SetFirstActiveState(GetState(activeStepIndex, selectedIndex));
				}
				else
				{
					step.State = GetState(activeStepIndex, selectedIndex);
				}

				++activeStepIndex;
			}
		}

		ActivationStepState GetState(int stepIndex, int currentIndex)
		{
			if(currentIndex < stepIndex)
				return ActivationStepState.Before;

			if(currentIndex == stepIndex)
				return ActivationStepState.During;

			return ActivationStepState.After;
		}

		int ClampIndex(int index)
		{
			int clampedIndex = Mathf.Clamp(index, 0, CurrentActiveStepCount - 1);

			if(clampedIndex < 0)
				clampedIndex = 0;

			return clampedIndex;
		}
	}
}