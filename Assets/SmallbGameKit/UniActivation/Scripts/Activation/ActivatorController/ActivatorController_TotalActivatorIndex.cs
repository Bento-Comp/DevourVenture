using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	[DefaultExecutionOrder(-31997)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/ActivatorController_TotalActivatorIndex")]
	public class ActivatorController_TotalActivatorIndex : MonoBehaviour
	{
		public Activator activator;

		public List<Activator> activators;

		int ActivationIndex
		{
			get
			{
				int activationIndexTotal = 0;

				foreach(Activator activator in activators)
				{
					if(activator == null)
						continue;

					activationIndexTotal += activator.SelectedIndex;
				}

				return activationIndexTotal;
			}
		}

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			activator.SetFirstActiveState(ActivationIndex);

			foreach(Activator activatorPart in activators)
				activatorPart.onUpdateActivation += OnUpdateActivation;
		}

		void OnDisable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			foreach(Activator activatorPart in activators)
				activatorPart.onUpdateActivation -= OnUpdateActivation;
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(activator == null)
				return;

			UpdateActivation();
		}
		#endif

		void OnUpdateActivation()
		{
			UpdateActivation();
		}

		void UpdateActivation()
		{
			activator.SelectedIndex = ActivationIndex;
		}
	}
}
