using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;
using UIAnimatorCore;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SkinRewardUnlockedScreenActivator")]
	public class SkinRewardUnlockedScreenActivator : MonoBehaviour
	{
		public Activator activator;

		public DeactivateIfSkinUnlockedActivator deactivateIfSkinUnlockedActivator;

		public UIAnimator popUpAnimator;

		public void CloseSkinRewardScreen()
		{
			popUpAnimator.PlayAnimation(AnimSetupType.Outro, OnCloseAnimationFinish);
		}

		void OnCloseAnimationFinish()
		{
			activator.SelectedIndex = 0;
			deactivateIfSkinUnlockedActivator.OnCloseSkinRewardScreen();
		}

		void Start()
		{
			activator.SelectedIndex = 0;
			if(SkinRewardManager.Instance != null && SkinRewardManager.Instance.SkinHasBeenUnlocked)
			{
				OnSkinUnlocked();
			}
			else
			{
				SkinRewardManager.onSkinUnlocked += OnSkinUnlocked;
			}
		}

		void OnDestroy()
		{
			SkinRewardManager.onSkinUnlocked -= OnSkinUnlocked;
		}

		void OnSkinUnlocked()
		{
			StartCoroutine(SkinUnlockedSequence(SkinRewardManager.Instance.unlockDelay));
		}

		IEnumerator SkinUnlockedSequence(float delay)
		{
			yield return new WaitForSeconds(delay);

			activator.SelectedIndex = 1;
		}

	}
}