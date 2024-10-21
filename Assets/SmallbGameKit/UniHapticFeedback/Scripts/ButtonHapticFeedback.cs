using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/ButtonHapticFeedback")]
	public class ButtonHapticFeedback : MonoBehaviour
	{
		public EHapticFeedbackType feedbackType;

		Button button;

		void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick); 
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);
		}

		void OnClick()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}