using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/ToggleButtonHapticFeedback")]
	public class ToggleButtonHapticFeedback : MonoBehaviour
	{
		public EHapticFeedbackType feedbackTypeOn;

		public EHapticFeedbackType feedbackTypeOff;

		Toggle button;

		void Awake()
		{
			button = GetComponent<Toggle>();
			button.onValueChanged.AddListener(OnValueChange); 
		}

		void OnDestroy()
		{
			if(button != null)
				button.onValueChanged.RemoveListener(OnValueChange);
		}

		void OnValueChange(bool value)
		{
			HapticFeedbackManager.TriggerHapticFeedback(value?feedbackTypeOn:feedbackTypeOff);
		}
	}
}