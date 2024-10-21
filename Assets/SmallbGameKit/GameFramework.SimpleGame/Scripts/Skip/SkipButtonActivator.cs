using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UniAds;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SkipButtonActivator")]
	public class SkipButtonActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		void OnEnable()
		{
			activator.SetFirstActiveState(0);

			OnRewardedAvailable(UniAds.AdsManager.Instance.RewardedAdAvailable);

			AdsManager.onRewardedAvailable += OnRewardedAvailable;

			AdsManager.Instance.NotifyRewardedOpportunityStart(SkipButton.skipButton_rewardedAdId);
		}

		void OnDisable()
		{
			if(AdsManager.Instance != null)
			{
				// TODO : SEV : can not be call at scene reload if the ads manager instance is already null
				AdsManager.Instance.NotifyRewardedOpportunityEnd(SkipButton.skipButton_rewardedAdId);
			}

			AdsManager.onRewardedAvailable -= OnRewardedAvailable;
		}

		void OnRewardedAvailable(bool available)
		{
			Debug.Log("SkipButtonActivator : OnRewardedAvailable : available = " + available);
			if(available)
			{
				activator.SelectedIndex = 1;
			}
			else
			{
				activator.SelectedIndex = 0;
			}
		}
	}
}