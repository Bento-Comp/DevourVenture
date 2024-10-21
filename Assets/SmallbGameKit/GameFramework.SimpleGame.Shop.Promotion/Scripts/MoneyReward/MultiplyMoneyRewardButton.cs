using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MultiplyMoneyRewardButton")]
	public class MultiplyMoneyRewardButton : MenuButton
	{
		public Text label;

		public ClaimMoneyRewardActivator claimRewardActivator;

		static readonly string rewardedAdId_multiplyMoney = "multiply_money";

		public override void OnClick()
		{
			base.OnClick();

			UniAds.AdsManager.Instance.ShowRewardedAd(rewardedAdId_multiplyMoney, OnRewardedAdEnd);
		}

		void OnEnable()
		{
			label.text = "GET X" + MoneyRewardManager.Instance.CurrentAvailableMultiply;

			UniAds.AdsManager.Instance.NotifyRewardedOpportunityStart(rewardedAdId_multiplyMoney);
		}

		void OnDisable()
		{
			if(UniAds.AdsManager.Instance != null)
				UniAds.AdsManager.Instance.NotifyRewardedOpportunityEnd(rewardedAdId_multiplyMoney);
		}

		void OnRewardedAdEnd(bool success)
		{
			if(success == false)
				return;

			MoneyRewardManager.Instance.MultiplyCurrentReward();
			claimRewardActivator.NotifyRewardObtained();
		}
	}
}
