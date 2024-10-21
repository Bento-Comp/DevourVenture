using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Editor_AutofillActivator")]
	public class Editor_AutofillActivator : MonoBehaviour
	{
		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			AutoFill();
		}

		void AutoFill()
		{
			Activator activator = gameObject.GetComponent<Activator>();

			if(activator == null)
				return;

			List<ActivationGroup> activationGroups = activator.activationGroups;

			activationGroups.Clear();

			foreach(Transform child in transform)
			{
				ActivationGroup activationGroup = new ActivationGroup();

				activationGroup.gameObjects = new GameObject[]{child.gameObject};

				activationGroups.Add(activationGroup);
			}

			activator.activationGroups = activationGroups;
		}
		#endif
	}
}
