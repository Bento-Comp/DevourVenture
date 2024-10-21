using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnGameStart")]
	public class HapticFeedbackOnGameStart : GameBehaviour
	{
		public EHapticFeedbackType feedbackType = EHapticFeedbackType.SelectionChange;

		protected override void OnGameStart()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}