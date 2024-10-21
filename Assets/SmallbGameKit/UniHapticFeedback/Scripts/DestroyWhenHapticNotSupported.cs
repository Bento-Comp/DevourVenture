using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/DestroyWhenHapticNotSupported")]
	public class DestroyWhenHapticNotSupported : MonoBehaviour
	{
		public GameObject gameObjectToDestroy;

		void Start()
		{
			if(gameObjectToDestroy == null)
				return;
			
			if(HapticFeedbackManager.Instance.HapticFeedbackSupported == false || HapticFeedbackManager.Instance.hapticFeedbackEnabled == false)
				DestroyImmediate(gameObjectToDestroy);
		}
	}
}