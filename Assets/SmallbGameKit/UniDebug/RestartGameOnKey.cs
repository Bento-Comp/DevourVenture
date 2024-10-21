using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace UniDebugLog
{
	[AddComponentMenu("UniDebug/RestartGameOnKey")]
	public class RestartGameOnKey : MonoBehaviour 
	{
		public KeyCode key = KeyCode.R;

		bool pause;

		#if UNITY_EDITOR
		void Update()
		{
			if(Input.GetKeyDown(key))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
		#endif
	}
}
