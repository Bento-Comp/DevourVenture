using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/AnimationEventHapticFeedback")]
	public class AnimationEventHapticFeedback : MonoBehaviour
	{
		public EHapticFeedbackType feedbackType;

		public bool mute;

		public void HapticFeedback()
		{
			if(mute)
				return;

			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}