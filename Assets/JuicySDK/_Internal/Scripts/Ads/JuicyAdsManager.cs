using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using Juicy;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31900)]
    [AddComponentMenu("JuicySDKInternal/JuicyAdsManager")]
    public class JuicyAdsManager : MonoBehaviour
    {
        #region Variables

        public bool IsInterstitialAvailable => (adsManager == null) ? false : isInterstitialAvailable;

        public bool IsInterstitialAllowedToBeDisplayed => (adsManager == null) ? false
            : (ElapsedTimeSinceLastInterstitial >= JuicySDK.Settings.DelayBetweenInterstitial);

        float ElapsedTimeSinceLastInterstitial => (float)DateTime.Now.Subtract(lastInterstitialTime).TotalSeconds;


        //---------- Manager ----------//
        static JuicyAdsManager instance;
        public static JuicyAdsManager Instance { get { return instance; } }

        public static Action OnAdDisplayed;
        public static Action OnAdClosed;

        AdsManagerBase adsManager;

        bool AdsRemoved { get { return JuicyRemoveAdsManager.AdsRemoved; } }

        //---------- Banner ----------//

        bool isBannerFailed = false;
        bool autoShowBannerAtStart = true;

        //---------- Interstitial ----------//

        bool isInterstitialFailed = false;

        DateTime lastInterstitialTime;
        DateTime lastIdealInterstitialTime;
        DateTime lastPotentialInterstitialTime;

        DateTime lastInterstitialRequestTime;

        bool isInterstitialAvailable { get { return adsManager.InterstitialAvailable; } }

        //---------- Rewarded ----------//

        bool isRewardedFailed = false;

        DateTime lastRewardedRequestTime;

        Action<bool> currentRewardedAction;
        public Action<bool> onRewardedAvailable;
        public bool IsRewardedAvailable { get; private set; } = false;
        #endregion

        #region Manager

        //---------- Unity ----------//

        void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                    Destroy(gameObject);

                return;
            }

            instance = this;
        }

        void OnDestroy()
        {
            JuicyRemoveAdsManager.onRemoveAds -= OnRemoveAds;
        }

        //----------- Public ----------//

        public void Initialize()
        {
            lastInterstitialTime = DateTime.Now;
            lastPotentialInterstitialTime = DateTime.Now;
            lastIdealInterstitialTime = DateTime.Now;

            CreateAdManager();
            InitializeAdsManagers(AdsRemoved);

            JuicyRemoveAdsManager.onRemoveAds += OnRemoveAds;

            if(autoShowBannerAtStart)
                ShowBanner();
        }

        public void UpdatePrivacySettings()
        {
            adsManager.UpdatePrivacySettings();
        }

        public void ShowMediationTestSuite()
        {
            if (adsManager == null)
            {
                JuicySDKLog.LogWarning("JuicyAdsManager : ShowMediationTestSuite : No ads manager set up");
                return;
            }

            adsManager.ShowMediationTestSuite();
        }
        //----------- Private ----------//

        void CreateAdManager()
        {
#if UNITY_EDITOR || noJuicyCompilation
                adsManager = JuicyUtility.CreateSubManager<JuicyAdsEmulationManager>(transform);
#else
                adsManager = JuicyUtility.CreateSubManager<MediationManager>(transform);
#endif
        }

        void InitializeAdsManagers(bool adsRemoved)
        {
            adsManager.InitializeAdsManager(adsRemoved);
        }

        void OnRemoveAds()
        {
            adsManager.OnRemoveAds();
        }

        float GetRevenueFromProperties(EventProperty[] properties)
        {
            return EventProperty.ExtractFloatValueFromArray("revenue", 0, properties);
        }
        #endregion
        #region Banner

        //------------ Public ----------//

        public void ShowBanner()
        {
            autoShowBannerAtStart = true;

            if (adsManager == null)
            {
                JuicySDKLog.LogWarning("JuicyAdsManager : ShowBanner : No ads manager set up");
                return;
            }

            if (AdsRemoved)
                return;

            BannerInsert.Instance?.DisplayBannerInsert(true);

            if (adsManager.BannerAvailable)
                adsManager.ShowBanner();
        }

        public void HideBanner()
        {
            autoShowBannerAtStart = false;

            if (AdsRemoved)
                return;

            if (adsManager != null)
            {
                adsManager.HideBanner();
                BannerInsert.Instance?.DisplayBannerInsert(false);
            }
        }

        //---------- Notifications ----------//

        public void NotifyBannerRequest()
        {
            if (!isBannerFailed)
            {
                JuicySDKLog.Verbose("JuicyAdsManager : NotifyBannerRequest" );
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_request");
            }
        }

        public void NotifyBannerCreated()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyBannerCreated");
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_created");

            if (autoShowBannerAtStart)
                ShowBanner();
        }

        public void NotifyBannerStart(params EventProperty[] properties)
        {
            isBannerFailed = false;
            JuicySnapshot.TotalBanner++;
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyBannerStart");

            //Get revenue
            JuicySnapshot.CurrentRevenue += GetRevenueFromProperties(properties);

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_fetch", new EventProperty("success", true), new EventProperty("error", "none"));
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_start", properties);
        }

        public void NotifyBannerLoadingFailure(string error)
        {
            if (!isBannerFailed)
            {
                isBannerFailed = true;
                JuicySDKLog.Verbose("JuicyAdsManager : NotifyBannerLoadingFailure : " + error);
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_fetch", new EventProperty("success", false), new EventProperty("error",error));
            }
        }

        public void NotifyBannerClick()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyBannerClick");
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("banner_click");
        }

        #endregion
        #region Interstitial

        //---------- Public -----------//

        public void NotifyInterstitialOpportunity()
        {
            if (AdsRemoved)
                return;

            if (adsManager == null)
            {
                JuicySDKLog.LogWarning("JuicyAdsManager : NotifyInterstitialOpportunity : No ads manager set up");
                return;
            }

            float elapsedTimeSinceLastIdealInterstitial = (float)DateTime.Now.Subtract(lastIdealInterstitialTime).TotalSeconds;
            float elapsedTimeSinceLastPotentialInterstitial = (float)DateTime.Now.Subtract(lastPotentialInterstitialTime).TotalSeconds;
            float elapsedTimeSinceLastInterstitial = (float)DateTime.Now.Subtract(lastInterstitialTime).TotalSeconds;

            float delayBetweenInterstial = JuicySDK.Settings.DelayBetweenInterstitial;
            bool interstitialAvailable = isInterstitialAvailable;

            JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialOpportunity : elapsedTimeSinceLastInterstitial = "
                                + elapsedTimeSinceLastInterstitial + "s/" + delayBetweenInterstial+ "s");

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_opportunity", new EventProperty("available", interstitialAvailable), new EventProperty("network", Application.internetReachability.ToString()));

            //Check delay between interstitials
            //Ideal
            if (elapsedTimeSinceLastIdealInterstitial >= delayBetweenInterstial)
            {
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_ideal");
                lastIdealInterstitialTime = DateTime.Now;
            }
            //Potential
            if (elapsedTimeSinceLastPotentialInterstitial >= delayBetweenInterstial)
            {
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_potential");
                lastPotentialInterstitialTime = DateTime.Now;
                elapsedTimeSinceLastPotentialInterstitial = 0;
            }
            //Real
            if (elapsedTimeSinceLastInterstitial >= delayBetweenInterstial)
            {
                JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialOpportunity : Interstitial Trigger");
                if (interstitialAvailable)
                {
                    JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialOpportunity : Interstitial Available");
                    ShowInterstitial();

                    //Potential call + readjustment with real timer. Do the call only if there's at least half the delay passed (to avoid too close calls).
                    if (elapsedTimeSinceLastPotentialInterstitial >= delayBetweenInterstial / 2)
                        JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_potential");

                    lastPotentialInterstitialTime = DateTime.Now;
                }

                else //interstitial not available
                {
                    JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialOpportunity : Interstitial Not Available");
                }

                if(JuicyAnalyticsManager.Instance.FirstInstallLessThan24hAgo()) //If the first install was in the past 24h hours : send event, else not
                    JuicyAnalyticsManager.Instance.TrackMonetizationEvent(JuicyMonetizationEventType.InterstitialTrigger, new EventProperty("available", interstitialAvailable), new EventProperty("network", Application.internetReachability.ToString()));
            }

            else //elapsed time since last interstitial too small
            {
                JuicySDKLog.Log("JuicyAdsManager : NotifyInterstitialOpportunity : Interstitial can only be displayed every " + delayBetweenInterstial + "s. " +
                    "Time since last interstitial : " + elapsedTimeSinceLastInterstitial + "s.");
            }
        }

        //--------- Notifications ----------//
        public void NotifyInterstitialRequest()
        {
            if (!isInterstitialFailed)
            {
                JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialRequest : " + this);
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_request");
                lastInterstitialRequestTime = DateTime.Now;
            }
        }

        public void NotifyInterstitialLoaded()
        {
            isInterstitialFailed = false;
            float loadDurationInSeconds = (float)DateTime.Now.Subtract(lastInterstitialRequestTime).TotalSeconds;

            JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialLoaded : loadDurationInSeconds = " + loadDurationInSeconds);

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_fetch", new EventProperty("success", true),
                                                                            new EventProperty("loadDurationInSeconds", loadDurationInSeconds),
                                                                            new EventProperty("error", "none"));

        }

        public void NotifyInterstitialLoadingFailure(string error)
        {
            if (!isInterstitialFailed)
            {
                isInterstitialFailed = true;
                float loadDurationInSeconds = (float)DateTime.Now.Subtract(lastInterstitialRequestTime).TotalSeconds;

                JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialLoadingFailure : loadDurationInSeconds = " + loadDurationInSeconds);

                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_fetch", new EventProperty("success", false),
                                                                                new EventProperty("loadDurationInSeconds", loadDurationInSeconds),
                                                                                new EventProperty("error", error));
            }
        }

        public void NotifyInterstitialStart(params EventProperty[] properties)
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialStart");
            JuicySnapshot.TotalInterstitial++;

            //Get revenue
            JuicySnapshot.CurrentRevenue += GetRevenueFromProperties(properties);

            JuicyAnalyticsManager.Instance.TrackMonetizationEvent(JuicyMonetizationEventType.InterstitialStart, properties);
        }

        public void NotifyInterstitialEnd()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialEnd");
            lastInterstitialTime = DateTime.Now;
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_end");
        }

        public void NotifyInterstitialClick()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyInterstitialClick");
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("interstitial_click");
        }

        //---------- Private ----------//

        void ShowInterstitial()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : ShowInterstitial");

            if (adsManager.InterstitialAvailable)
            {
                adsManager.ShowInterstitial();
                lastInterstitialTime = DateTime.Now;
            }
        }

#if debugJuicySDKInternal
        [Obsolete("Do not use this function unless your a member of the Juicy SDK developpement team")]
        public void SkipInterstitialDelay()
        {
            Debug.LogWarning("JuicyAdsManager : SkipInterstitialDelay : If you see this warning please remove all references to the JuicyAdsManager > SkipInterstitialDelay function");
            lastInterstitialTime = lastInterstitialTime.AddSeconds(-JuicySDK.Settings.DelayBetweenInterstitial);
        }
#endif
        #endregion
        #region Rewarded

        //---------- Public -----------//

        public void NotifyRewardedOpportunityStart(string id)
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedOpportunityStart : Notify Rewarded opportunity start for ID: " + id);
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_opportunity", new EventProperty("start", true), new EventProperty("caller_id", id));
        }

        public void NotifyRewardedOpportunityEnd(string id)
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedOpportunityEnd : Notify Rewarded opportunity end for ID: " + id);
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_opportunity", new EventProperty("start", false), new EventProperty("caller_id", id));
        }

        public void ShowRewarded(string id, Action<bool> onRewardedEnd)
        {
            if (adsManager == null)
            {
                JuicySDKLog.LogWarning("JuicyAdsManager : ShowRewarded : No ads manager set up");
                return;
            }

            if (IsRewardedAvailable)
            {
                JuicySDKLog.Verbose("JuicyAdsManager : ShowRewarded : Show rewarded for ID: " + id);
                currentRewardedAction = onRewardedEnd;
                adsManager.ShowRewarded();
            }

            else
                JuicySDKLog.Verbose("JuicyAdsManager : ShowRewarded : Rewarded Not Available for ID + " + id);

            if (JuicyAnalyticsManager.Instance.FirstInstallLessThan24hAgo()) //If the first install was in the past 24h hours : send event, else not
                JuicyAnalyticsManager.Instance.TrackMonetizationEvent(JuicyMonetizationEventType.RewardedTrigger, new EventProperty("available", IsRewardedAvailable), new EventProperty("caller_id", id));
        }

        //---------- Notifications ----------//

        public void NotifyRewardedAvailable(bool available)
        {
            if (adsManager == null)
                return;

            if (IsRewardedAvailable == available)
                return;

            IsRewardedAvailable = available;
            onRewardedAvailable?.Invoke(available);
        }

        public void NotifyRewardedStart(params EventProperty[] properties)
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedStart");
            JuicySnapshot.TotalRewarded++;

            //Get revenue
            JuicySnapshot.CurrentRevenue += GetRevenueFromProperties(properties);

            JuicyAnalyticsManager.Instance.TrackMonetizationEvent(JuicyMonetizationEventType.RewardedStart, properties);
        }

        public void NotifyRewardedEnd(bool success)
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedEnd : success = " + success);

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_end", new EventProperty("success", success));
            
            if (JuicySDK.Settings.NoInterstitialAfterRewarded && success)
            {
                lastInterstitialTime = DateTime.Now;
                lastIdealInterstitialTime = DateTime.Now;
                lastPotentialInterstitialTime = DateTime.Now;
            }

            currentRewardedAction.Invoke(success);
        }

        public void NotifyRewardedClick()
        {
            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedClick");
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_click");
        }

        public void NotifyRewardedRequest()
        {
            if (!isRewardedFailed)
            {
                JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedRequest : " + this);
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_request");
                lastRewardedRequestTime = DateTime.Now;
            }
        }

        public void NotifyRewardedLoaded()
        {
            isRewardedFailed = false;
            float loadDurationInSeconds = (float)DateTime.Now.Subtract(lastRewardedRequestTime).TotalSeconds;

            JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedLoaded : loadDurationInSeconds = " + loadDurationInSeconds);

            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_fetch", new EventProperty("success", true),
                                                                        new EventProperty("loadDurationInSeconds", loadDurationInSeconds),
                                                                        new EventProperty("error", "none"));
        }

        public void NotifyRewardedLoadingFailure(string error)
        {
            if (!isRewardedFailed)
            {
                isRewardedFailed = true;
                float loadDurationInSeconds = (float)DateTime.Now.Subtract(lastRewardedRequestTime).TotalSeconds;

                JuicySDKLog.Verbose("JuicyAdsManager : NotifyRewardedLoadingFailure : loadDurationInSeconds = " + loadDurationInSeconds);

                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rewarded_fetch", new EventProperty("success", false),
                                                                            new EventProperty("loadDurationInSeconds", loadDurationInSeconds),
                                                                            new EventProperty("error", error));
            }
        }
        #endregion
    }
}
