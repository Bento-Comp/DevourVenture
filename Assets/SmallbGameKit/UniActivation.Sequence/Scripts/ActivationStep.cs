using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/ActivationStep")]
	public class ActivationStep : MonoBehaviour
	{
		public bool mute;

		public Activator activator;

		public int beforeStateIndex = 0;
		public int duringStateIndex = 1;
		public int afterStateIndex = 0;

		[SerializeField]
		ActivationStepState state;

		[SerializeField]
		ActivationSequence sequenceOwner;

		public ActivationStepState State
		{
			get => state;

			set
			{
				if(state == value)
					return;

				state = value;

				UpdateActivation();
			}
		}

		public void Exit()
		{
			if(State != ActivationStepState.During)
				return;

			if(sequenceOwner == null)
			{
				State = ActivationStepState.After;
				return;
			}

			if(sequenceOwner.CurrentStep != this)
				return;

			++sequenceOwner.SelectedIndex;
		}

		public void SetFirstActiveState(ActivationStepState state)
		{
			this.state = state; 
			activator.SetFirstActiveState(GetStateIndex(state));
		}

		public void SetSequenceOwner(ActivationSequence sequence)
		{
			this.sequenceOwner = sequence;
		}

		void OnEnable()
		{
			if(sequenceOwner == null)
				return;

			sequenceOwner.NotifyStepEnable();
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			UpdateActivation();
		}
		#endif

		void UpdateActivation()
		{
			if(mute)
				return;

			if(activator == null)
				return;

			activator.SelectedIndex = GetStateIndex(state);
		}

		int GetStateIndex(ActivationStepState state)
		{
			switch(state)
			{
				case ActivationStepState.Before:
				{
					return beforeStateIndex;
				}

				case ActivationStepState.During:
				{
					return duringStateIndex;
				}

				default:
				case ActivationStepState.After:
				{
					return afterStateIndex;
				}
			}
		}
	}
}