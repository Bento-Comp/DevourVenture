using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SkinRewardUnlockProgressBar")]
	public class SkinRewardUnlockProgressBar : MonoBehaviour
	{
		public List<Image> fillImages;

		public Text progressText;

		public UniHapticFeedback.EHapticFeedbackType haptic_progress = UniHapticFeedback.EHapticFeedbackType.SelectionChange;
		public float haptic_progress_period = 0.1f;
		public UniHapticFeedback.EHapticFeedbackType haptic_end = UniHapticFeedback.EHapticFeedbackType.Heavy;

		float currentProgress;

		bool initialized;

		void Start()
		{
			if(SkinRewardManager.Instance != null && SkinRewardManager.Instance.SkinUnlockHasProgressed)
			{
				Initialize();
				OnSkinUnlockProgress();
			}
			else
			{
				SkinRewardManager.onSkinUnlockProgress += OnSkinUnlockProgress;
			}
		}

		void OnDestroy()
		{
			SkinRewardManager.onSkinUnlockProgress -= OnSkinUnlockProgress;
		}

		void Initialize()
		{
			if(initialized)
				return;

			initialized = true;

			SetProgress(SkinRewardManager.Instance.LastSkinUnlockProgressPercent);
		}

		void OnSkinUnlockProgress()
		{
			StartCoroutine(AnimateProgress(
				SkinRewardManager.Instance.SkinUnlockProgressPercent,
				SkinRewardManager.Instance.animateSkinUnlockProgressDuration));
		}

		IEnumerator AnimateProgress(float targetProgress, float duration)
		{
			float startProgress = currentProgress;

			float elapsedTime = 0.0f;
			float lastHapticTime = 0.0f;
			while(elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;

				float animationPercent;
				if(elapsedTime >= duration)
				{
					animationPercent = 1.0f;
				}
				else
				{
					animationPercent = elapsedTime/duration;
				}

				float progress = Mathf.Lerp(startProgress, targetProgress, animationPercent);

				SetProgress(progress);

				if((elapsedTime - lastHapticTime) >= haptic_progress_period)
				{
					lastHapticTime = elapsedTime;
					UniHapticFeedback.HapticFeedbackManager.TriggerHapticFeedback(haptic_progress);
				}

				yield return null;
			}

			UniHapticFeedback.HapticFeedbackManager.TriggerHapticFeedback(haptic_end);
		}

		void SetProgress(float progress)
		{
			currentProgress = progress;
			foreach(Image fillImage in fillImages)
			{
				fillImage.fillAmount = progress;
			}

			progressText.text = Mathf.RoundToInt(progress * 100.0f) + "%";
		}
	}
}