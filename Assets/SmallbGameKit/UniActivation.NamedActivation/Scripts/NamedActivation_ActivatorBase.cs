using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/NamedActivation_ActivatorBase")]
	public abstract class NamedActivation_ActivatorBase : MonoBehaviour
	{
		public Activator activator;

		[SerializeField]
		NamedActivationsRegister localTo;

		protected NamedActivationsRegister register;

		protected abstract int ComputeActivationIndex();

		#if UNITY_EDITOR
		protected virtual void OnEditorUpdate_EditMode(){}
		#endif

		NamedActivationsRegister RetrieveRegister()
		{
			if(localTo != null)
				return localTo;

			return NamedActivationsManager.Instance;
		}

		void OnEnable()
		{
			TryToRegister();

			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				Editor_Update();
			}
			#endif

			UpdateActivation();
		}

        void OnDisable()
		{
			TryToUnregister();
		}

		void OnNamedActivationChanged()
		{
			UpdateActivation();
		}

		void UpdateActivation()
		{
			if(enabled == false)
				return;

			if(activator == null)
				return;

			if(register == null)
				return;

			int selectedIndex = ComputeActivationIndex();

			activator.SetFirstActiveState(selectedIndex);
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
            if (Application.isPlaying)
                return;

			Editor_Update();
		}

		void Editor_Update()
		{
			UpdateRegister();

            UpdateActivation();

			OnEditorUpdate_EditMode();
		}

		void UpdateRegister()
		{
			TryToUnregister();
			TryToRegister();
		}
		#endif

		void TryToRegister()
		{
			register = RetrieveRegister();

			if(register == null)
				return;

			register.onNamedActivationChanged += OnNamedActivationChanged;
		}

		void TryToUnregister()
		{
			if(register == null)
				return;

			register.onNamedActivationChanged -= OnNamedActivationChanged;

			register = null;
		}
	}
}