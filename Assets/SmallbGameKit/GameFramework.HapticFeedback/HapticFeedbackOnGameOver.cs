using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnGameOver")]
	public class HapticFeedbackOnGameOver : GameBehaviour
	{
		public EHapticFeedbackType feedbackType = EHapticFeedbackType.Failure;

		protected override void OnGameOver()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}