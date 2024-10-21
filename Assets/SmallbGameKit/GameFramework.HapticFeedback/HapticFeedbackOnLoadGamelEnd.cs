using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnLoadGameEnd")]
	public class HapticFeedbackOnLoadGameEnd : GameBehaviour
	{
		public EHapticFeedbackType feedbackType = EHapticFeedbackType.SelectionChange;

		protected override void OnLoadGameEnd(bool reloadSceneAfter)
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}