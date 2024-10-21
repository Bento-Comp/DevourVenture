using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnLoadGame")]
	public class HapticFeedbackOnLoadGame : GameBehaviour
	{
		public EHapticFeedbackType feedbackType = EHapticFeedbackType.SelectionChange;

		protected override void OnLoadGame()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}