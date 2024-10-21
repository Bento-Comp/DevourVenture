using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniHapticFeedback;

namespace GameFramework.HapticFeedback
{
	[AddComponentMenu("GameFramework/HapticFeedbackOnLevelCompleted")]
	public class HapticFeedbackOnLevelCompleted : GameBehaviour
	{
		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			GameScreenButton gameScreenButton = GameScreenButton.Instance;
			if(gameScreenButton != null
				&&
				( gameScreenButton.levelCompletedScreenAfterGameOver && Game.IsGameOver )
				)
			{
				HapticFeedbackManager.TriggerHapticFeedback(EHapticFeedbackType.Success);
				return;
			}

			HapticFeedbackManager.TriggerHapticFeedback(success?EHapticFeedbackType.Success:EHapticFeedbackType.Failure);
		}
	}
}