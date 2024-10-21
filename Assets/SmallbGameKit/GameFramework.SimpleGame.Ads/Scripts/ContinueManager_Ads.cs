using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/Ads/ContinueManager")]
	public class ContinueManager_Ads : ContinueManager 
	{
		static string continue_rewardedAdId = "continue";

		public bool continueEnabled = false;

		protected override void NotifyContinueOpportunityStart(System.Action<bool> onContinueCanBeCalled)
		{
			if(continueEnabled && UniAds.AdsManager.Instance.RewardedAdAvailable)
            {
                onContinueCanBeCalled?.Invoke(true);
				UniAds.AdsManager.Instance.NotifyRewardedOpportunityStart(continue_rewardedAdId);
            }
            else
            {
				onContinueCanBeCalled?.Invoke(false);
            }
		}

		protected override void NotifyContinueOpportunityEnd()
		{
			UniAds.AdsManager.Instance.NotifyRewardedOpportunityEnd(continue_rewardedAdId);
		}

		protected override void OnAskForContinue()
		{
			if(UniAds.AdsManager.Instance == null)
			{
				OnRewardedAdEnd(true);
				return;
			}
				
			UniAds.AdsManager.Instance.ShowRewardedAd(continue_rewardedAdId, OnRewardedAdEnd);
		}

		void OnRewardedAdEnd(bool success)
		{
			NotifyAskForContinueAnswer(success);
		}
	}
}
