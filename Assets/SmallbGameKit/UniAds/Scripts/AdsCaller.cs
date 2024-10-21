using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniAds
{
	[AddComponentMenu("UniAds/AdsCaller")]
	public abstract class AdsCaller : MonoBehaviour
	{

		AdsManager adsManager;

		public virtual bool BannerAvailable
		{
			get
			{
				return true;
			}
		}

		public virtual bool InterstitialAvailable
		{
			get
			{
				return true;
			}
		}

		public virtual bool RewardedAdAvailable
		{
			get
			{
				return true;
			}
		}

		public virtual void NotifyRewardedOpportunityStart(string rewardedId)
		{
		}

		public virtual void NotifyRewardedOpportunityEnd(string rewardedId)
		{
		}

		public virtual void NotifyInterstitialOpportunity()
		{
		}

		public virtual void ShowBanner(bool visible)
		{
		}

		public virtual void DestroyBanner()
		{
		}

		public virtual void DestroyInterstitial()
		{
		}

        public virtual void ShowRewardedAd()
        {
        }

		public virtual void ShowRewardedAd(string rewardedAdId, System.Action<bool> onRewardedEnd)
        {
            ShowRewardedAd(rewardedAdId, onRewardedEnd);
        }

		public virtual void Initialize(AdsManager adsManager)
		{
			this.adsManager = adsManager;
		}

		public virtual void Terminate()
		{
			adsManager = null;
		}

		protected void NotifyRewardedAvailable(bool available)
		{
			if(adsManager != null)
				adsManager.NotifyRewardedAvailable(available);
		}
	}
}
