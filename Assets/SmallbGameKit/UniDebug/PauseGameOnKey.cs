using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniDebugLog
{
	[AddComponentMenu("UniDebug/PauseGameOnKey")]
	public class PauseGameOnKey : MonoBehaviour 
	{
		public KeyCode key = KeyCode.T;

		bool pause;

		#if UNITY_EDITOR
		void Update()
		{
			if(Input.GetKeyDown(key))
			{
				pause = !pause;
				if(pause)
				{
					Time.timeScale = 0.0f;
				}
				else
				{
					Time.timeScale = 1.0f;
				}
			}
		}
		#endif
	}
}
