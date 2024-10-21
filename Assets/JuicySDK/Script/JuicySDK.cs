using UnityEngine;
using System.Collections.Generic;
using JuicyInternal;
using System;

namespace Juicy
{
    public static class JuicySDK
    {
        #region Variables
        /// <summary>
        /// Current version installed of the Juicy SDK
        /// </summary>
        public static string version
        {
            get
            {
#if !noJuicyCompilation
                return JuicySDKSettings.version + "_" + JuicySDKMediationConfig.version;
#else
                return JuicySDKSettings.version;
#endif
            }
        }

        public static bool IsInterstitialAvailable => (AdsManager != null) ? AdsManager.IsInterstitialAvailable : false;
        public static bool IsInterstitialAllowedToBeDisplayed => (AdsManager != null) ? AdsManager.IsInterstitialAllowedToBeDisplayed : false;
        public static bool HasPurchaseFailedPatch => true;

        /// <summary>
        /// Whether or not the user bought the remove ads option
        /// </summary>
        public static bool AdsRemoved { get { return JuicyRemoveAdsManager.AdsRemoved; } }

        /// <summary>
        /// Whether or not the restore purchases is supported
        /// </summary>
        public static bool RestorePurchasesSupported { get { return JuicyPurchaseManager.Instance.RestorePurchasesSupported; } }

        /// <summary>
        /// Whether or not a rewarded is curently available
        /// </summary>
        public static bool IsRewardedAvailable { get { return JuicyAdsManager.Instance.IsRewardedAvailable; } }

        /// <summary>
        /// Whether ABTesting is enabled or not
        /// </summary>
        public static bool IsABTestEnabled { get { return Settings.EnableAbTest; } }

        /// <summary>
        /// Get the ab test current cohort index (-1 == not cohorted, 0 == control, 1...x == variant
        /// </summary>
        public static int ABTestCohortVariantIndex { get { return JuicyABTestManager.Instance.CohortIndex; } }

        /// <summary>
        /// Whether or not the user has allowed to track its personnal data
        /// </summary>
        public static bool IsAllowedToTrackData { get { return JuicyPrivacyManager.IsAllowedToTrackData; } }

        /// <summary>
        /// Whether or not the user has allowed to personalized ads
        /// </summary>
        public static bool IsAllowedToPersonalizedAd { get { return JuicyPrivacyManager.IsAllowedToPersonalizedAds; } }

        /// <summary>
        /// Action invoked on game start
        /// parameter:
        /// index of the current level
        /// </summary>
        public static Action<int> onGameStart;

        /// <summary>
        /// Action invoke on game end
        /// parameter:
        /// score reached during the level
        /// whether or not the level has been completed successfully
        /// index of the current level
        /// </summary>
        public static Action<int, bool, int> onGameEnd;


        //Settings
        public static JuicySDKSettings Settings { get { return JuicySDKSettings.Instance; } }
        //Juicy Manager Access
        static JuicySDKManager JuicySDKManager { get { return JuicySDKManager.Instance; } }
        static JuicyPrivacyManager JuicyPrivacyManager { get { return JuicyPrivacyManager.Instance; } }
        static JuicyAdsManager AdsManager { get { return JuicyAdsManager.Instance; } }
        static JuicyAnalyticsManager AnalyticsManager { get { return JuicyAnalyticsManager.Instance; } }
        static JuicyABTestManager ABTestManager { get { return JuicyABTestManager.Instance; } }
        static JuicyPurchaseManager PurchaseManager { get { return JuicyPurchaseManager.Instance; } }
        static JuicyPurchaseRemoveAdsManager PurchaseRemoveAdsManager { get { return JuicyPurchaseRemoveAdsManager.Instance; } }
        static JuicyRemoveAdsManager RemoveAdsManager { get { return JuicyRemoveAdsManager.Instance; } }
        static JuicyRatingManager RatingManager { get { return JuicyRatingManager.Instance; } }

        //Score
        public static int BestScore { get { return _BestScore; } }
        static int _BestScore { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.BEST_SCORE); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.BEST_SCORE, value); } }
        //Level
        static int currentLevel;
        #endregion

        public static List<ProductSummary> GetProductSummaryList()
        {
            return PurchaseManager.GetProductSummaryList();
        }

        #region General
        /// <summary>
        /// Add a function to trigger once the JuicySDK is initialized (on first launch this will be trigger only once the user
        /// sets its privacy settings)
        /// </summary>
        /// <param name="onInitialized">Action to invoke when the Juicy SDK is initialized</param>
        public static void AddJuicySDKInitializedListener(Action onInitialized)
        {
            JuicySDKManager.OnJuicySDKInitialized += onInitialized;
        }
        /// <summary>
        /// Remove a function to trigger once the JuicySDK is initialized (on first launch this will be trigger only once the user
        /// sets its privacy settings)
        /// </summary>
        /// <param name="onInitialized">Action to invoke when the Juicy SDK is initialized</param>
        public static void RemoveJuicySDKInitializedListener(Action onInitialized)
        {
            JuicySDKManager.OnJuicySDKInitialized -= onInitialized;
        }
        #endregion
        #region Ads

        //--------- Banner ----------//

        /// <summary>
        /// Display the banner at the bottom of the screen
        /// The banner is automatically shows once the SDK as initialized, you'll have to use this only if you use HideBanner first
        /// </summary>
        public static void ShowBanner()
        {
            JuicySDKLog.Verbose("JuicySDK : ShowBanner");
            AdsManager.ShowBanner();
        }

        /// <summary>
        /// Hide the banner
        /// </summary>
        public static void HideBanner()
        {
            JuicySDKLog.Verbose("JuicySDK : HideBanner");
            AdsManager.HideBanner();
        }

        //---------- Interstitial ----------//

        /// <summary>
        /// Display an interstitial if there's one available
        /// </summary>
        public static void NotifyInterstitialOpportunity()
        {
            JuicySDKLog.Verbose("JuicySDK : NotifyInterstitialOpportunity");
            AdsManager.NotifyInterstitialOpportunity();
        }

        //---------- Rewarded ----------//

        /// <summary>
        /// Add a listener to the availability state of the rewarded ad
        /// </summary>
        /// <param name="onRewardedAvailable">Action to invoke when the rewarded availability state changes</param>
        public static void AddRewardedAvailableListener(Action<bool> onRewardedAvailable)
        {
            AdsManager.onRewardedAvailable += onRewardedAvailable;
        }

        /// <summary>
        /// Remove a listener from the rewarded availability state
        /// </summary>
        /// <param name="onRewardedAvailable">Listener to remove</param>
        public static void RemoveRewardedAvailableListener(Action<bool> onRewardedAvailable)
        {
            AdsManager.onRewardedAvailable -= onRewardedAvailable;
        }

        /// <summary>
        /// Notify the SDK of a rewarded opportunity start (a button that can display a rewarded video has appeared)
        /// </summary>
        /// <param name="id">Id of the opportunity</param>
        public static void NotifyRewardedOpportunityStart(string id)
        {
            JuicySDKLog.Verbose("JuicySDK : NotifyRewardedOpportunityStart : Rewarded Opportunity Start for id: " + id);
            AdsManager.NotifyRewardedOpportunityStart(id);
        }

        /// <summary>
        /// Notify the SDK of a rewarded opportunity end (a button that can display a rewarded video has disappeared)
        /// </summary>
        /// <param name="id">Id of the opportunity</param>
        public static void NotifyRewardedOpportunityEnd(string id)
        {
            JuicySDKLog.Verbose("JuicySDK : NotifyRewardedOpportunityEnd : Rewarded Opportunity End for id: " + id);
            AdsManager.NotifyRewardedOpportunityEnd(id);
        }

        /// <summary>
        /// Display a rewarded for a certain ID
        /// </summary>
        /// <param name="id">Id of the button who called this function</param>
        /// <param name="onRewardedEnd">Action to invoke when the rewarded ends, the boolean paramter of the action is true if the rewarded
        /// video has been fully watched, false otherwise </param>
        public static void ShowRewarded(string id, Action<bool> onRewardedEnd)
        {
            JuicySDKLog.Verbose("JuicySDK : ShowRewarded for ID: " + id);
            AdsManager.ShowRewarded(id, onRewardedEnd);
        }


        //---------- Ad callbacks ----------//

        /// <summary>
        /// Add a listener to be notified when a full screen ad is opened
        /// </summary>
        /// <param name="onAdDisplayed"><Action to invoke when an ad is opened/param>
        public static void AddAdDisplayedListener(Action onAdDisplayed)
        {
            JuicyAdsManager.OnAdDisplayed += onAdDisplayed;
        }

        /// <summary>
        /// Add a listener to be notified when a full screen ad is opened
        /// </summary>
        /// <param name="onAdDisplayed"><Action to invoke when an ad is opened/param>
        public static void RemoveAdDisplayedListener(Action onAdDisplayed)
        {
            JuicyAdsManager.OnAdDisplayed -= onAdDisplayed;
        }

        /// <summary>
        /// Add a listener to be notified when a full screen ad is closed
        /// </summary>
        /// <param name="onAdClosed">Action to invoke when an ad is closed</param>
        public static void AddAdClosedListener(Action onAdClosed)
        {
            JuicyAdsManager.OnAdClosed += onAdClosed;
        }

        /// <summary>
        /// Add a listener to be notified when a full screen ad is closed
        /// </summary>
        /// <param name="onAdClosed">Action to invoke when an ad is closed</param>
        public static void RemoveAdClosedListener(Action onAdClosed)
        {
            JuicyAdsManager.OnAdClosed -= onAdClosed;
        }

        #endregion
        #region Analytics

        //---------- Events ----------//

        // There are some limitations with the data base in which
        // the events are gathered. To avoid issues before logging your
        // own events talk about them with the Juicy team to make sure
        // everything is ok.

        /// <summary>
        /// Send an event to the different analytics connected to the SDK
        /// </summary>
        /// <param name="eventName">Name of the event to send</param>
        [Obsolete("Discuss with the Juicy team before logging your own events")]
        public static void TrackEvent(string eventName)
        {
            TrackEvent(eventName, null);
        }

        /// <summary>
        /// Send an event to the different analytics connected to the SDK
        /// </summary>
        /// <param name="eventName">Name of the event to send</param>
        /// <param name="eventProperties">Array of properties to include within the event/param>
        [Obsolete("Discuss with the Juicy team before logging your own events")]
        public static void TrackEvent(string eventName, params EventProperty[] eventProperties)
        {
            JuicySDKLog.Verbose(EventProperty.AddToString("JuicySDK : TrackEvent : eventName = " + eventName, eventProperties));
            AnalyticsManager.TrackUserEvent(eventName, eventProperties);
        }

        //---------- Game notification ----------//

        /// <summary>
        /// Notify the start of a game to the SDK
        /// </summary>
        /// <param name="level">Current level, use -1 if your game doesn't have a notion of level</param>
        public static void NotifyGameStart(int level)
        {
            JuicySDKLog.Verbose("JuicySDK : NotifyGameStart : level = " + level);

            AnalyticsManager.TrackGameStart(level);
            currentLevel = level;

            onGameStart?.Invoke(level);
        }

        /// <summary>
        /// Notify the end of a game to the SDk
        /// </summary>
        /// <param name="score">Score reached at the end of the game</param>
        /// <param name="success">Whether or not the level has been completed successfully</param>
        public static void NotifyGameEnd(int score, bool success = true)
        {
            if (AnalyticsManager.IsGameStarted == false)
                return;

            bool bestScoreBeaten = false;
            if (score > _BestScore)
            {
                bestScoreBeaten = true;
                _BestScore = score;
            }

            JuicySDKLog.Verbose("JuicySDK : NotifyGameEnd : score = " + score + " | success = " + success + " | level = " + currentLevel);
            AnalyticsManager.TrackGameEnd(score, success, currentLevel);
            RatingManager.NotifyShowRateBoxOpportunity(bestScoreBeaten, success, currentLevel);

            onGameEnd?.Invoke(score, success, currentLevel);
        }
        #endregion
        #region Privacy
        /// <summary>
        /// Add a function to trigger when the user changes its privacy settings.
        /// </summary>
        /// <param name="onInitialized">Action to invoke when the user changes its privacy settings (the first boolean is whether or not he allowed to track its data
        /// and the second is whether or not he allowed personalized ads)</param>
        public static void AddPrivacySettingsChangeListener(Action<bool, bool> onChange)
        {
            JuicyPrivacyManager.onPrivacySettingsChange += onChange;
        }
        /// <summary>
        /// Remove a function to trigger when the user changes its privacy settings.
        /// </summary>
        /// <param name="onInitialized">Action to invoke when the user changes its privacy settings (the first boolean is whether or not he allowed to track its data
        /// and the second is whether or not he allowed personalized ads)</param>
        public static void RemovePrivacySettingsChangeListener(Action<bool, bool> onChange)
        {
            JuicyPrivacyManager.onPrivacySettingsChange -= onChange;
        }
        /// <summary>
        /// Display the privacy settings window
        /// </summary>
        public static void ShowPrivacySettings()
        {
            JuicySDKLog.Verbose("JuicySDK : ShowPrivacySettings");
            JuicyPrivacyManager.ShowPrivacySettings();
        }
        #endregion
        #region Purchase
        /// <summary>
        /// Trigger the RemoveAds process
        /// </summary>
        public static void BuyRemoveAds()
        {
            JuicySDKLog.Verbose("JuicySDK : BuyRemoveAds");
            PurchaseRemoveAdsManager.BuyRemoveAds();
        }

        /// <summary>
        /// Add a listener to the RemoveAds process completion
        /// </summary>
        /// <param name="removeAdsListener">Listener to add</param>
        public static void AddRemoveAdsListener(System.Action removeAdsListener)
        {
            JuicyRemoveAdsManager.onRemoveAds += removeAdsListener;

            if (AdsRemoved == false)
                return;

            if (removeAdsListener != null)
            {
                removeAdsListener();
            }
        }

        /// <summary>
        /// Remove a listener from the RemoveAds process completion
        /// </summary>
        /// <param name="removeAdsListener">Listener to remove</param>
        public static void RemoveRemoveAdsListener(System.Action removeAdsListener)
        {
            JuicyRemoveAdsManager.onRemoveAds -= removeAdsListener;
        }

        /// <summary>
        /// Trigger the Buying of an product process
        /// </summary>
        /// <param name="productId">ID of the product to buy</param>
        public static void BuyProduct(string productId)
        {
            JuicySDKLog.Verbose("JuicySDK : BuyProduct : " + productId);
            PurchaseManager.BuyProduct(productId);
        }

        /// <summary>
        /// Add a listener to the product delivery event (once it the product has been bought successfully)
        /// </summary>
        /// <param name="productDeliveryListener">Listener to add</param>
        public static void AddProductDeliveryListener(System.Action<ProductSummary> productDeliveryListener)
        {
            JuicyPurchaseManager.onProductDelivery += productDeliveryListener;
        }

        /// <summary>
        /// Remove a listener from the product delivery event (once it the product has been bought successfully)
        /// </summary>
        /// <param name="productDeliveryListener">Listener to add</param>
        public static void RemoveProductDeliveryListener(System.Action<ProductSummary> productDeliveryListener)
        {
            JuicyPurchaseManager.onProductDelivery -= productDeliveryListener;
        }

        /// <summary>
        /// Add a listener to the purchase failed event
        /// </summary>
        /// <param name="onPurchaseFailed">Listener to add</param>
        public static void AddPurchaseFailedCallback(Action onPurchaseFailed)
        {
            JuicyPurchaseManager.onPurchaseFail += onPurchaseFailed;
        }

        /// <summary>
        /// Remove listener from the purchase failed event
        /// </summary>
        /// <param name="onPurchaseFailed">Listener to remove</param>
        public static void RemovePurchaseFailedCallback(Action onPurchaseFailed)
        {
            JuicyPurchaseManager.onPurchaseFail -= onPurchaseFailed;
        }

        /// <summary>
        /// Restore the purchases already made in the app   
        /// </summary>
        public static void RestorePurchases()
        {
            JuicyPurchaseManager.Instance.RestorePurchases();
        }
        #endregion //Purchase
    }
}