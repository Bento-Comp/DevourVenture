using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SkipButton")]
	public class SkipButton : MenuButton
	{
		public readonly static string skipButton_rewardedAdId = "skip_button";

		public override void OnClick()
		{
			UniAds.AdsManager.Instance.ShowRewardedAd(skipButton_rewardedAdId, OnRewardedEnd);
		}

		void OnRewardedEnd(bool success)
		{
			if(success)
			{
				Game.Instance.SkipCurrentLevel();
			}
		}
	}
}