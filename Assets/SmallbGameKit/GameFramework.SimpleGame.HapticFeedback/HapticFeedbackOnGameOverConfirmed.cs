using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.SimpleGame.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnGameOverConfirmed")]
	public class HapticFeedbackOnGameOverConfirmed : MonoBehaviour
	{
		public EHapticFeedbackType feedbackType = EHapticFeedbackType.Failure;

		void Awake()
		{
			ContinueManager.onGameOverConfirmed += OnGameOverConfirmed;
		}

		void OnDestroy()
		{
			ContinueManager.onGameOverConfirmed -= OnGameOverConfirmed;
		}

		void OnGameOverConfirmed()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);
		}
	}
}