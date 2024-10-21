#if !noJuicyCompilation
using System;
using Juicy;
using UnityEngine;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31899)]
    [AddComponentMenu("JuicySDKInternal/MediationManager")]
    public class MediationManager : AdsManagerBase
    {
        bool adsRemoved;

        bool bannerExist;

        bool rewardedAdSuccessBeforeClose;

        bool waitToReceiveReward;

        static float refetchPeriodAfterError = 3.0f;
        static float waitToReceiveRewardDuration = 5.0f;

        static string applovinSDKKey = "FBFn1IH4kf1UL9_Oz8SttuSw1m3Hl3Q-EgmG-2LhiMB3WBRTKvKCoo6qX95HCWGRB0hA31smO2pvDFMX8hlxTr";

        public string BannerID { get { return JuicySDKSettings.Instance.MediationConfig.BannerID; } }
        public string InterstitialID { get { return JuicySDKSettings.Instance.MediationConfig.InterstitialID; } }
        public string RewardedID { get { return JuicySDKSettings.Instance.MediationConfig.RewardedID; } }

        public MaxSdk.AdInfo InterstitialAdInfo { get; private set; } = null;
        public MaxSdk.AdInfo RewardedAdInfo { get; private set; } = null;

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
                return MaxSdk.IsInterstitialReady(InterstitialID);
            }
        }

        public override bool RewardedAvailable
        {
            get
            {
                return MaxSdk.IsRewardedAdReady(RewardedID);
            }
        }

        public override void InitializeAdsManager(bool adsRemoved)
        {
            base.InitializeAdsManager(adsRemoved);

            this.adsRemoved = adsRemoved;

            JuicySDKLog.Log("MediationManager : AppLovin : Initialize : applovinSDKKey = " + applovinSDKKey);

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            MaxSdk.SetSdkKey(applovinSDKKey);
            MaxSdk.InitializeSdk();
        }

        public override void UpdatePrivacySettings()
        {
            base.UpdatePrivacySettings();
            JuicySDKLog.Verbose("MediationManager : UpdatePrivacySettings");

            //GDPR stuff
            MaxSdk.SetHasUserConsent(JuicyPrivacyManager.IsAllowedToPersonalizedAds);
            MaxSdk.SetIsAgeRestrictedUser(!JuicyPrivacyManager.IsAgeAllowed);
            MaxSdk.SetDoNotSell(!JuicyPrivacyManager.IsAllowedToTrackData);

            //California stuff
            if (!JuicyPrivacyManager.IsAllowedToPersonalizedAds)
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { "LDU" }, 1, 1000);
            else
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });

            //Facebook ios14 stuff
#if UNITY_IOS
            if (Juicy.IOS.JuicyIOSPlugin.IsIOS14OrAbove())
            {
                bool adTrackingEnabled = !JuicyPrivacyManager.GetLATStatus() && JuicyPrivacyManager.IsAllowedToPersonalizedAds;
                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(adTrackingEnabled);
            }
#endif
        }

        public override void ShowBanner()
        {
            base.ShowBanner();
            MaxSdk.ShowBanner(BannerID);
        }

        public override void HideBanner()
        {
            base.HideBanner();
            MaxSdk.HideBanner(BannerID);
        }

        public override void ShowInterstitial()
        {
            if (!InterstitialAvailable)
            {
                JuicySDKLog.Verbose("Mediation Manager : ShowInterstitial : No Inter loaded");
                return;
            }

            string networkName = "Unknown";
            string revenue = "0";
            if (RewardedAdInfo == null)
            {
                networkName = InterstitialAdInfo.NetworkName;
                revenue = InterstitialAdInfo.Revenue.ToString();
            }
            OnInterstitialStart(new EventProperty("network", networkName), new EventProperty("revenue", revenue));
            JuicyAdsManager.OnAdDisplayed?.Invoke();

            base.ShowInterstitial();
            MaxSdk.ShowInterstitial(InterstitialID);
        }

        public override void ShowRewarded()
        {
            if (!RewardedAvailable)
            {
                JuicySDKLog.Verbose("Mediation Manager : ShowRewarded : No Rewarded loaded");
                return;
            }

            string networkName = "Unknown";
            string revenue = "0";
            if (RewardedAdInfo == null)
            {
                networkName = RewardedAdInfo.NetworkName;
                revenue = RewardedAdInfo.Revenue.ToString();
            }

            OnRewardedStart(new EventProperty("network", networkName), new EventProperty("revenue", revenue));
            OnRewardedAvailable(false);
            JuicyAdsManager.OnAdDisplayed?.Invoke();

            base.ShowRewarded();
            rewardedAdSuccessBeforeClose = false;
            MaxSdk.ShowRewardedAd(RewardedID);
        }

        public override void ShowMediationTestSuite()
        {
            base.ShowMediationTestSuite();
            MaxSdk.ShowMediationDebugger();
        }

        public override void OnRemoveAds()
        {
            base.OnRemoveAds();
            JuicySDKLog.Verbose("MediationManager : OnRemoveAds");

            adsRemoved = true;

            if (bannerExist)
            {
                JuicySDKLog.Verbose("MediationManager : OnRemoveAds : Destroy Banner");
                MaxSdk.DestroyBanner(BannerID);
                bannerExist = false;
            }

            CancelInvoke("RefetchInterstitial");
        }

        void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            UpdatePrivacySettings();

            if (adsRemoved == false)
            {
                JuicySDKLog.Verbose("MediationManager : OnSdkInitializedEvent : banner_unitID = " + BannerID);
                InitializeBannerAds();
                InitializeInterstitialAds();
            }

            InitializeRewardedAds();

            if (adsRemoved == false)
            {
                FetchBanner();
                FetchInterstitial();
            }
            FetchRewarded();
        }

        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
        {
            string adPlatform = "Applovin";
            string networkName = impressionData.NetworkName;
            string adUnitIdentifier = impressionData.AdUnitIdentifier;
            string adFormat = impressionData.AdFormat;
            double revenue = impressionData.Revenue;
            string currency = "USD"; // All AppLovin revenue is sent in USD

            JuicyAnalyticsManager.Instance?.TrackAdRevenue(adPlatform, networkName, adUnitIdentifier, adFormat, revenue, currency);
        }

        // Banner
        void InitializeBannerAds()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        }

        void FetchBanner()
        {
            MaxSdk.BannerPosition bannerPosition;
            if (JuicySDK.Settings.BannerPosition == BannerPosition.Top)
                bannerPosition = MaxSdkBase.BannerPosition.TopCenter;

            else bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;

            MaxSdk.CreateBanner(BannerID, bannerPosition);

            bannerExist = true;

            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(BannerID, Color.white);

            OnBannerCreated();

            OnBannerRequest();
        }

        // Fired when a banner is loaded
        void OnBannerAdLoadedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnBannerAdLoadedEvent");

            string networkName = adInfo.NetworkName;
            string revenue = adInfo.Revenue.ToString();

            OnBannerStart(new EventProperty("network", networkName), new EventProperty("revenue", revenue));
        }

        // Fired when a banner has failed to load
        void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo)
        {
            string error = errorInfo.AdLoadFailureInfo;
            JuicySDKLog.Verbose("MediationManager : OnBannerAdLoadFailedEvent : errorCode = " + error);
            OnBannerLoadingFailure(error);
        }

        // Fired when a banner ad is clicked
        void OnBannerAdClickedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnBannerAdClickedEvent");
            OnBannerClick();
        }

        // Fired when a banner ad expands to encompass a greater portion of the screen
        void OnBannerAdExpandedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnBannerAdExpandedEvent");
        }

        // Fired when a banner ad collapses back to its initial size
        void OnBannerAdCollapsedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnBannerAdCollapsedEvent");
        }

        // Interstitial
        void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;

        }

        void FetchInterstitial()
        {
            JuicySDKLog.Verbose("MediationManager : FetchInterstitial : interstitial_unitID = " + InterstitialID);
            OnInterstitialRequest();
            MaxSdk.LoadInterstitial(InterstitialID);
        }

        void RefetchInterstitial()
        {
            if (adsRemoved)
                return;

            JuicySDKLog.Verbose("MediationManager : RefetchInterstitial");
            FetchInterstitial();
        }

        void ResetInterstitialRefetchTimer(float refetchPeriod)
        {
            JuicySDKLog.Verbose("MediationManager : ResetInterstitialRefetchTimer : refetchPeriod = " + refetchPeriod);

            CancelInvoke("RefetchInterstitial");
            Invoke("RefetchInterstitial", refetchPeriod);
        }

        void OnInterstitialLoadedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            JuicySDKLog.Verbose("MediationManager : Interstitial_OnAdLoaded");
            InterstitialAdInfo = adInfo;
            OnInterstitialLoaded();
        }

        void OnInterstitialFailedEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
            string error = errorInfo.AdLoadFailureInfo;
            JuicySDKLog.Verbose("MediationManager : Interstitial_OnAdFailedToLoad : error = " + error);
            OnInterstitialLoadingFailure(error);

            ResetInterstitialRefetchTimer(refetchPeriodAfterError);
        }

        void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo, MaxSdk.AdInfo adInfo)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            JuicyAdsManager.OnAdClosed?.Invoke();
            RefetchInterstitial();
        }

        void OnInterstitialDisplayedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : Interstitial_OnAdOpened");
        }

        void OnInterstitialDismissedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            JuicySDKLog.Verbose("MediationManager : Interstitial_OnAdClosed");
            OnInterstitialEnd();
            RefetchInterstitial();
            JuicyAdsManager.OnAdClosed?.Invoke();
        }

        void OnInterstitialClickedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnInterstitialClickedEvent");
            OnInterstitialClick();
        }

        // Rewarded
        void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        }

        void FetchRewarded()
        {
            JuicySDKLog.Verbose("MediationManager : FetchRewarded : rewardedAd_unitID = " + RewardedID);
            OnRewardedRequest();
            MaxSdk.LoadRewardedAd(RewardedID);
        }

        void RefetchRewarded()
        {
            JuicySDKLog.Verbose("MediationManager : RefetchRewarded");
            FetchRewarded();
        }

        void ResetRewardedRefetchTimer(float refetchPeriod)
        {
            JuicySDKLog.Verbose("MediationManager : ResetRewardedRefetchTimer : refetchPeriod = " + refetchPeriod);

            CancelInvoke("RefetchRewarded");
            Invoke("RefetchRewarded", refetchPeriod);
        }

        void OnRewardedAdLoadedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdLoadedEvent");
            RewardedAdInfo = adInfo;
            OnRewardedLoaded();
            OnRewardedAvailable(true);
        }

        void OnRewardedAdFailedEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
            string error = errorInfo.AdLoadFailureInfo;
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdFailedEvent : error = " + error);
            OnRewardedLoadingFailure(error);
            ResetRewardedRefetchTimer(refetchPeriodAfterError);
        }

        void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo, MaxSdk.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            JuicyAdsManager.OnAdClosed?.Invoke();
            JuicyAdsManager.Instance.NotifyRewardedAvailable(false);
            RefetchRewarded();
        }

        void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdDisplayedEvent");
        }

        void OnRewardedAdClickedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdClickedEvent");
            OnRewardedClick();
        }

        void OnRewardedAdDismissedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdDismissedEvent");
            JuicyAdsManager.OnAdClosed?.Invoke();
            if (rewardedAdSuccessBeforeClose)
            {
                OnRewardedEnd(true);
                RefetchRewarded();
            }
            else
            {
                StartWaitToReceiveReward();
            }
        }

        void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdk.AdInfo adInfo)
        {
            // Rewarded ad was displayed and user should receive the reward

            string label = reward.Label;
            double amount = reward.Amount;
            JuicySDKLog.Verbose("MediationManager : OnRewardedAdReceivedRewardEvent : amount = " + amount.ToString() + " | label = " + label);

            if (waitToReceiveReward)
                StopWaitToReceiveReward(true);

            else
                rewardedAdSuccessBeforeClose = true;
        }

        void StartWaitToReceiveReward()
        {
            JuicySDKLog.Verbose("MediationManager : StartWaitToReceiveReward");
            waitToReceiveReward = true;
            Invoke("WaitToReceiveRewardTimeout", waitToReceiveRewardDuration);
        }

        void StopWaitToReceiveReward(bool receivedReward)
        {
            JuicySDKLog.Verbose("MediationManager : StopWaitToReceiveReward : receivedReward = " + receivedReward);
            CancelInvoke("WaitToReceiveRewardTimeout");
            waitToReceiveReward = false;
            OnRewardedEnd(receivedReward);
            RefetchRewarded();
        }

        void WaitToReceiveRewardTimeout()
        {
            JuicySDKLog.Verbose("MediationManager : WaitToReceiveRewardTimeout");
            StopWaitToReceiveReward(false);
        }
    }
}
#endif
