using System;
using UnityEngine;

namespace JuicyInternal
{
    public class JuicyAdsEmulationManager : AdsManagerBase
    {
        JuicyAdsEmulation emulation { get { return JuicyAdsEmulation.Instance; } }

        JuicyEmulatedBanner banner;
        JuicyEmulatedInterstitial interstitial;
        JuicyEmulatedRewarded rewarded;

        //Pseudo simulate a variable refetch time for the ads
        float interstitialFetchTime { get { return UnityEngine.Random.Range(3, 5); } }
        float rewardedFetchTime { get { return UnityEngine.Random.Range(3, 5); } }

        bool adsRemoved;

        #region Herited
        public override bool BannerAvailable
        {
            get
            {
                return banner.IsReady;
            }
        }

        public override bool InterstitialAvailable
        {
            get
            {
                return interstitial.IsReady;
            }
        }

        public override bool RewardedAvailable
        {
            get
            {
                return rewarded.IsReady;
            }
        }

        public override void InitializeAdsManager(bool adsRemoved)
        {
            base.InitializeAdsManager(adsRemoved);
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Initialize");
            this.adsRemoved = adsRemoved;

            if (!adsRemoved)
            {
                banner = emulation.CreateEmulatedBanner();
                banner.OnLoaded += BannerLoadEvent;
                interstitial = emulation.CreateEmulatedInterstitial();
                interstitial.OnLoaded += InterstitialLoadEvent;
                interstitial.OnOpened += InterstitialOpenEvent;
                interstitial.OnClosed += InterstitialCloseEvent;
            }
            rewarded = emulation.CreateEmulatedReawarded();
            rewarded.OnLoaded += RewardedLoadEvent;
            rewarded.OnOpened += RewardedOpenEvent;
            rewarded.OnClosedSuccess += RewardedCloseEvent;

            UpdatePrivacySettings();
            if (!adsRemoved)
            {
                Invoke("FetchBanner",3.0f);
                RefetchInterstitial();
            }
            RefetchRewarded();
        }

        public override void UpdatePrivacySettings()
        {
            base.UpdatePrivacySettings();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Update Privacy Settings");
        }

        public override void ShowBanner()
        {
            base.ShowBanner();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Show Banner");
            banner?.Show();
        }

        public override void HideBanner()
        {
            base.HideBanner();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Hide Banner");
            banner?.Close();
        }

        public override void ShowInterstitial()
        {
            base.ShowInterstitial();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Show Interstitial");
            interstitial?.Show();
        }

        public override void ShowRewarded()
        {
            base.ShowRewarded();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Show Rewarded");
            rewarded?.Show();
        }

        public override void ShowMediationTestSuite()
        {
            base.ShowMediationTestSuite();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Show Mediation Test Suite : This only works in build");
        }

        public override void OnRemoveAds()
        {
            base.OnRemoveAds();
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Remove Ads");
            adsRemoved = true;
            if(banner != null)
                Destroy(banner.gameObject);
        }
        #endregion
        #region Banner
        void FetchBanner()
        {
            if (JuicySDKSettings.Instance.SkipAdsInEditor)
                return;

            banner.Load();
            OnBannerRequest();
            OnBannerCreated();
        }

        void BannerLoadEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Banner loaded");
            OnBannerStart();
        }
        #endregion
        #region Interstitial
        void FetchInterstitial()
        {
            OnInterstitialRequest();
            interstitial.Load();
        }

        void RefetchInterstitial()
        {
            if (adsRemoved)
                return;

            Invoke("FetchInterstitial", interstitialFetchTime);
        }

        void InterstitialLoadEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Interstitial loaded");
            OnInterstitialLoaded();
        }

        void InterstitialOpenEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Interstitial opened");
            OnInterstitialStart();
            JuicyAdsManager.OnAdDisplayed?.Invoke();
        }

        void InterstitialCloseEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Interstitial closed");
            OnInterstitialEnd();
            RefetchInterstitial();
            JuicyAdsManager.OnAdClosed?.Invoke();
        }

        #endregion
        #region Rewarded
        void FetchRewarded()
        {
            OnRewardedRequest();
            rewarded.Load();
        }

        void RefetchRewarded()
        {
            Invoke("FetchRewarded",rewardedFetchTime);
        }

        void RewardedLoadEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Rewarded loaded");
            OnRewardedLoaded();
            OnRewardedAvailable(true);
        }

        void RewardedOpenEvent()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Rewarded opened");
            OnRewardedStart();
            OnRewardedAvailable(false);
            JuicyAdsManager.OnAdDisplayed?.Invoke();
        }

        void RewardedCloseEvent(bool success)
        {
            JuicySDKLog.Verbose("Juicy Emulation : Ads : Rewarded closed : Reward given " + success);
            OnRewardedEnd(success);
            RefetchRewarded();
            JuicyAdsManager.OnAdClosed?.Invoke();
        }
        #endregion
    }
}

