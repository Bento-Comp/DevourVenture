using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteAlways()]
	[AddComponentMenu("UniActivation/GameObjectDisabler")]
	public class GameObjectDisabler : MonoBehaviour
	{
		public GameObject controlledGameObject;

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false && controlledGameObject == null)
				return;
			#endif
			
			controlledGameObject.SetActive(false);
		}

		void OnDisable()
		{
			if(controlledGameObject == null)
				return;

			controlledGameObject.SetActive(true);
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(controlledGameObject == null)
				return;

			controlledGameObject.SetActive(false);
		}
#endif
	}
}