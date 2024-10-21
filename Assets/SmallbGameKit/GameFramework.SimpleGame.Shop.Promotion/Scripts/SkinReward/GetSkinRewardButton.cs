using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/GetSkinRewardButton")]
	public class GetSkinRewardButton : MenuButton
	{
		public ClaimSkinRewardActivator claimRewardActivator;

		static readonly string rewardedAdId_getSkin = "get_skin";

		public override void OnClick()
		{
			base.OnClick();

			UniAds.AdsManager.Instance.ShowRewardedAd(rewardedAdId_getSkin, OnRewardedAdEnd);
		}

		void OnRewardedAdEnd(bool success)
		{
			if(success == false)
				return;

			claimRewardActivator.NotifyRewardObtained();
		}

		void OnEnable()
		{
			UniAds.AdsManager.Instance.NotifyRewardedOpportunityStart(rewardedAdId_getSkin);	
		}

		void OnDisable()
		{
			if(UniAds.AdsManager.Instance != null)
				UniAds.AdsManager.Instance.NotifyRewardedOpportunityEnd(rewardedAdId_getSkin);
		}
	}
}
