using UnityEngine;
using System.Collections;
using Juicy;
using System;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31899)]
    [AddComponentMenu("JuicySDKInternal/JuicyAdsManager")]
    public abstract class AdsManagerBase : MonoBehaviour
    {
        public virtual bool BannerAvailable
        {
            get
            {
                return false;
            }
        }

        public virtual bool InterstitialAvailable
        {
            get
            {
                return false;
            }
        }

        public virtual bool RewardedAvailable
        {
            get
            {
                return false;
            }
        }

        public virtual void InitializeAdsManager(bool adsRemoved)
        {
            JuicySDKLog.Verbose("AdsManagerBase : InitializeAdsManager : adsRemoved = " + adsRemoved);
        }

        public virtual void UpdatePrivacySettings()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnPrivacySrettingsUpdate");
        }

        public virtual void ShowBanner()
        {
            JuicySDKLog.Verbose("AdsManagerBase : ShowBanner");
        }

        public virtual void HideBanner()
        {
            JuicySDKLog.Verbose("AdsManagerBase : HideBanner");
        }

        public virtual void ShowInterstitial()
        {
            JuicySDKLog.Verbose("AdsManagerBase : ShowInterstitial");
        }

        public virtual void ShowRewarded()
        {
            JuicySDKLog.Verbose("AdsManagerBase : ShowRewarded");
        }

        public virtual void ShowMediationTestSuite()
        {
            JuicySDKLog.Verbose("AdsManagerBase : ShowMediationTestSuite");
        }

        public virtual void OnRemoveAds()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnRemoveAds");
        }

        // Banner
        protected void OnBannerRequest()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnBannerRequest");
            JuicyAdsManager.Instance.NotifyBannerRequest();
        }

        protected void OnBannerCreated()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnBannerCreated");
            JuicyAdsManager.Instance.NotifyBannerCreated();
        }

        protected void OnBannerStart(params EventProperty[] properties)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnBannerStart");
                JuicyAdsManager.Instance.NotifyBannerStart(properties);
            });
        }

        protected void OnBannerLoadingFailure(string error)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnBannerLoadingFailure : " + error);
                JuicyAdsManager.Instance.NotifyBannerLoadingFailure(error);
            });
        }

        protected void OnBannerClick()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnBannerClick");
                JuicyAdsManager.Instance.NotifyBannerClick();
            });
        }

        // Interstitial
        protected void OnInterstitialRequest()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialRequest");
            JuicyAdsManager.Instance.NotifyInterstitialRequest();
        }

        protected void OnInterstitialLoaded()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialLoaded");
                JuicyAdsManager.Instance.NotifyInterstitialLoaded();
            });
        }

        protected void OnInterstitialLoadingFailure(string error)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialLoadingFailure :  | error = " + error);
                JuicyAdsManager.Instance.NotifyInterstitialLoadingFailure(error);
            });
        }

        protected void OnInterstitialStart(params EventProperty[] properties)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialStart");
                JuicyAdsManager.Instance.NotifyInterstitialStart(properties);
            });
        }

        protected void OnInterstitialEnd()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialEnd");
                JuicyAdsManager.Instance.NotifyInterstitialEnd();
            });
        }

        protected void OnInterstitialClick()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnInterstitialClick");
                JuicyAdsManager.Instance.NotifyInterstitialClick();
            });
        }

        // Rewarded
        protected void OnRewardedRequest()
        {
            JuicySDKLog.Verbose("AdsManagerBase : OnRewardedRequest");
            JuicyAdsManager.Instance.NotifyRewardedRequest();
        }

        protected void OnRewardedAvailable(bool available)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedAvailable : " + available);
                JuicyAdsManager.Instance.NotifyRewardedAvailable(available);
            });
        }

        protected void OnRewardedLoaded()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedLoaded");
                JuicyAdsManager.Instance.NotifyRewardedLoaded();
            });
        }

        protected void OnRewardedLoadingFailure(string error)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedLoadingFailure : "+ error);
                JuicyAdsManager.Instance.NotifyRewardedLoadingFailure(error);
            });
        }

        protected void OnRewardedStart(params EventProperty[] properties)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedStart");
                JuicyAdsManager.Instance.NotifyRewardedStart(properties);
            });
        }

        protected void OnRewardedEnd(bool success)
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedEnd : success = " + success);
                JuicyAdsManager.Instance.NotifyRewardedEnd(success);
            });
        }

        protected void OnRewardedClick()
        {
            UnityThread.executeInUpdate(() =>
            {
                JuicySDKLog.Verbose("AdsManagerBase : OnRewardedClick");
                JuicyAdsManager.Instance.NotifyRewardedClick();
            });
        }
    }
}