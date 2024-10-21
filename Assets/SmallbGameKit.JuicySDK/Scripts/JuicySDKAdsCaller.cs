using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GameFramework;

using UniAds;

using Juicy;

using System;

namespace SmallbGameKit
{
	[AddComponentMenu("SmallbGameKit/JuicySDK/JuicySDKAdsCaller")]
	public class JuicySDKAdsCaller : AdsCaller
	{
		public bool debug_logEnabled;

		bool Debug_LogEnabled
		{
			get
			{
				return isActiveAndEnabled && debug_logEnabled;
			}
		}

		public override bool BannerAvailable
		{
			get
			{
				return true;
			}
		}

		public override bool InterstitialAvailable
		{
			get
			{
				return true;
			}
		}

		public override bool RewardedAdAvailable
		{
			get
			{
				bool rewardedAvailable = JuicySDK.IsRewardedAvailable;
				Log("RewardedAdAvailable : rewardedAvailable = " + rewardedAvailable);
				return rewardedAvailable;
			}
		}

		public override void ShowBanner(bool visible)
		{
			Log("ShowBanner : " + visible);
			JuicySDK.ShowBanner();
		}

		public override void DestroyBanner()
		{
			Log("DestroyBanner");
			JuicySDK.HideBanner();
		}

		public override void DestroyInterstitial()
		{
			Log("DestroyInterstitial");
		}

		public override void NotifyInterstitialOpportunity()
		{
			Log("ShowInterstitial");
			JuicySDK.NotifyInterstitialOpportunity();
		}

		public override void NotifyRewardedOpportunityStart(string rewardedId)
		{
			Log("NotifyRewardedOpportunityStart");
			JuicySDK.NotifyRewardedOpportunityStart(rewardedId);
		}

		public override void NotifyRewardedOpportunityEnd(string rewardedId)
		{
			Log("NotifyRewardedOpportunityEnd");
			JuicySDK.NotifyRewardedOpportunityEnd(rewardedId);
		}

        public override void ShowRewardedAd(string rewardedAdId, System.Action<bool> onRewardedEnd)
		{
			Log("ShowRewardedAd : rewardedAdId = " + rewardedAdId);
			JuicySDK.ShowRewarded(rewardedAdId, onRewardedEnd);
		}

		public override void Initialize(AdsManager adsManager)
		{
			Log("Initialize");

			base.Initialize(adsManager);
		}

		public override void Terminate()
		{
			Log("Terminate");

			base.Terminate();
		}

		void Awake()
		{
			JuicySDK.AddRewardedAvailableListener(OnRewardedAvailable);
		}

		void OnDestroy()
		{
			JuicySDK.RemoveRewardedAvailableListener(OnRewardedAvailable);
		}

		void OnRewardedAvailable(bool available)
		{
			NotifyRewardedAvailable(available);
		}

		void Log(string message)
		{
#if UNITY_EDITOR
			if(Debug_LogEnabled == false)
				return;

			Debug.Log("JuicySDKAdsCaller : " + message);
#endif
		}
	}
}
