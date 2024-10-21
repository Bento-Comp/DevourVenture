using System;
using System.Collections;
using System.Collections.Generic;
using Juicy;
using UnityEngine;
using AppsFlyerSDK;

namespace JuicyInternal
{
    public enum JuicyMonetizationEventType
    {
        InterstitialTrigger,
        InterstitialStart,
        RewardedTrigger,
        RewardedStart,
    }

    [DefaultExecutionOrder(-31999)]
    [AddComponentMenu("JuicySDKInternal/JuicyAnalyticsManager")]
    public class JuicyAnalyticsManager : MonoBehaviour
    {
        #region Variables
        //Static
        static JuicyAnalyticsManager instance;
        public static JuicyAnalyticsManager Instance { get { return instance; } }

        //Analytics Managers
        AppsFlyerManager appsFlyerManager;
        FirebaseAnalyticsManager firebaseAnalyticsManager;
        FacebookSDKManager facebookSDKManager;
        JuicyAnalyticsEmulationManager emulationManager;

        //Other analytics Stuff
        JuicySnapshot startSnapShot;
        float gameStartTime = 0;
        int gameStartFrameCount = 0;
        public bool IsGameStarted { get; private set; } = false;
        public int CurrentLevel { get; private set; } = -1;
        int eventIndex { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.EVENT_INDEX, 0); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.EVENT_INDEX, value); } }

        //Install Date
        static string installDateSave { get { return JuicyPlayerPrefs.GetString(JuicyPlayerPrefs.INSTALL_DATE, DateTime.MinValue.Ticks.ToString()); } set { JuicyPlayerPrefs.SetString(JuicyPlayerPrefs.INSTALL_DATE, value); } }
        DateTime installDate { get { return new DateTime(Convert.ToInt64(installDateSave)); } set { installDateSave = value.Ticks.ToString(); } }

        /*------ IOS App Tracking Transparency conversion value stuff ------*/
#if UNITY_IOS
        bool isConversionEventSend { get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.IS_CONVERSION_VALUE_SEND, false); } set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.IS_CONVERSION_VALUE_SEND, value); } }
        int conversionValue  { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.CONVERSION_VALUE); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.CONVERSION_VALUE, value); } }
        public Action OnConversionValueUpdate;

        //Conversion value map (in cents)
        float[] conversionValuesMap = new float[64] {
            0, 0.234f, 0.468f, 0.702f, 0.936f, 1.170f, 1.403f, 1.637f, 1.871f, 2.105f, 2.339f,
            2.573f, 2.807f, 3.041f, 3.275f, 3.509f, 3.743f, 3.977f, 4.210f, 4.444f, 4.678f,
            4.912f, 5.146f, 5.38f, 5.921f, 6.463f, 7.004f, 7.546f, 8.087f, 8.629f, 9.170f, 9.712f,
            10.253f, 10.795f, 11.336f, 11.878f, 12.419f, 12.961f, 13.502f, 14.044f, 14.586f, 15.127f,
            15.668f, 16.210f, 16.751f, 17.293f, 17.834f, 18.376f, 18.917f, 19.459f, 20.0f, 21.862f, 23.724f, 25.586f,
            27.448f, 29.310f, 31.172f, 33.034f, 34.896f, 36.758f, 38.620f, 179.080f, 319.540f, 460.0f };
#endif
        /*-----------------------------------------------------------------*/



        #endregion //Variables
        #region Methods
        #region Public
        public void Initialize()
        {
            CreateAnalyticsManagers();
            JuicyPurchaseManager.onProductDelivery += OnProductDelivery;

            //hack : added condition if (startSnapShot == null) because it can be initialized when automatic start script is executed. So a new startsnapshop should not be instantiated.
            if (startSnapShot == null)
                startSnapShot = new JuicySnapshot(false);

#if UNITY_IOS
            if (!isConversionEventSend)
                OnConversionValueUpdate += UpdateIOSConversionValue;
#endif
            OnSessionStart();
        }

        public void UpdatePrivacySettings()
        {
            firebaseAnalyticsManager?.UpdatePrivacySettings();
            facebookSDKManager?.UpdatePrivacySettings();
            appsFlyerManager?.UpdatePrivacySettings();
            emulationManager?.UpdatePrivacySettings();
            UnityAnalyticsManager.UpdatePrivacySettings();
        }

#if UNITY_IOS
        public void TryUpdateIOSConversionValue()
        {
            OnConversionValueUpdate?.Invoke();
        }
#endif

        #region Event Tracking
        /*---------- Analytics Event -------------*/
        /* Analytics event are send only to Firebase (and Facebook if the line is uncommented, right know we don't send event to facebook
         * Because it create huge lag spike on Android devices for a yet unknown reason*/

        public void TrackAnalyticsEvent(string eventName, JuicySnapshotFlag snapshotFlag, params EventProperty[] eventProperties)
        {
            if (!JuicyPrivacyManager.IsAllowedToTrackData)
            {
                JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackEvent : eventName = " + eventName + " can't be send because of privacy settings");
                return;
            }

            List<EventProperty> properties = new List<EventProperty>();
            //Add CohortIndexProperty
            properties.Add(new EventProperty("ab_test_variant_index", JuicySDK.ABTestCohortVariantIndex));
            properties.Add(new EventProperty("ab_test_name", JuicySDKSettings.Instance.AbTestName));
            //Add event index property
            properties.Add(new EventProperty("event_index", eventIndex));
            //Add snapshot properties
            EventProperty[] snapShotProperties = new JuicySnapshot().GetProperties(snapshotFlag);
            if (snapShotProperties.Length > 0)
                properties.AddRange(snapShotProperties);
            //Add users properties
            if (eventProperties?.Length > 0)
                properties.AddRange(eventProperties);

            string logString = "";
            foreach (EventProperty property in properties)
                logString += property.ToString() + " | ";
            JuicySDKLog.Verbose($"JuicyAnalyticsManager : TrackAnalyticsEvent : eventName = {eventName} with {properties.Count} : Properties : {logString}");

            firebaseAnalyticsManager?.SendEvent(eventName, properties);
            //facebookSDKManager?.SendEvent(eventName, properties);
            emulationManager?.SendEvent(eventName, properties);


            eventIndex++;
        }

        public void TrackAnalyticsEvent(string eventName, params EventProperty[] properties)
        {
            TrackAnalyticsEvent(eventName, JuicySnapshotFlag.Complete, properties);
        }

        public void TrackUserEvent(string eventName, params EventProperty[] properties)
        {
            TrackAnalyticsEvent("x_" + eventName, JuicySnapshotFlag.None, properties);
        }

        public void TrackGameStart(int level)
        {
            JuicySnapshot.GameCount++;
            IsGameStarted = true;
            CurrentLevel = level;
            gameStartTime = Time.fixedUnscaledTime;
            gameStartFrameCount = Time.frameCount;

#if UNITY_IOS
            TryUpdateIOSConversionValue();
#endif

            appsFlyerManager?.SendGameStart(level);

            //HACK : startSnapShot can be null because of automatic start script executed before startsnapshot initialization
            if (startSnapShot == null)
            {
                startSnapShot = new JuicySnapshot(false);
            }
            //Hack : end

            startSnapShot.TakeSnaphshot();

            if (!JuicyPrivacyManager.IsAllowedToTrackData)
            {
                JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackEvent : eventName = game_start can't be send because of privacy settings");
                return;
            }

            JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackGameStart : level = " + level);
            TrackAnalyticsEvent("game_start");
        }

        public void TrackGameEnd(int score, bool success, int level)
        {
            IsGameStarted = false;
            float gameTime = Time.fixedUnscaledTime - gameStartTime;
            float fps = Time.frameCount - gameStartFrameCount;
            JuicySnapshot.TotalGameTime += gameTime;

            appsFlyerManager?.SendGameEnd(level, success, gameTime);

            if (!JuicyPrivacyManager.IsAllowedToTrackData)
            {
                JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackEvent : eventName = game_end can't be send because of privacy settings");
                return;
            }

            JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackGameCompleted : score = " + score + " | success = " + success + " | level = " + level);

            List<EventProperty> properties = new List<EventProperty>(new JuicySnapshot().GetDifferentialProperties(startSnapShot));
            properties.Add(new EventProperty("game_time", gameTime));
            properties.Add(new EventProperty("game_fps", fps));
            properties.Add(new EventProperty("score", score));
            properties.Add(new EventProperty("success", success));
            TrackAnalyticsEvent("game_end", properties.ToArray());
        }

        /*---------- Monetization Event -------------*/
        /* Monetization event are send to every analytics we have (except facebook right now (see Analytics event). Since Adjust used a per app / per event token
         * for the event we cannot use eventName like for the analytics event. That's why we use an enum for each monetization event. An event is then
         * send to adjust with the right token and the enum is then converted to a string before being send to our other analytics*/

        public void TrackMonetizationEvent(JuicyMonetizationEventType eventType, JuicySnapshotFlag snapshotFlag, params EventProperty[] eventProperties)
        {
            string eventName = GetEventNameForType(eventType);
            List<EventProperty> properties = new List<EventProperty>();

            //Add CohortIndexProperty
            properties.Add(new EventProperty("ab_test_variant_index", JuicySDK.ABTestCohortVariantIndex));
            properties.Add(new EventProperty("ab_test_name", JuicySDKSettings.Instance.AbTestName));

            //Add event index property
            properties.Add(new EventProperty("event_index", eventIndex));
            //Add snapshot properties
            EventProperty[] snapShotProperties = new JuicySnapshot().GetProperties(snapshotFlag);
            if (snapShotProperties.Length > 0)
                properties.AddRange(snapShotProperties);
            //Add users properties
            if (eventProperties?.Length > 0)
                properties.AddRange(eventProperties);

            string logString = "";
            foreach (EventProperty property in properties)
                logString += property.ToString() + " | ";

            JuicySDKLog.Verbose($"JuicyAnalyticsManager : TrackEvent : eventName = {eventName} with {properties.Count} : Properties : {logString}");

            //Send the event to AppsFlyer
            appsFlyerManager?.SendMonetizationEvent(eventName, properties);

            //Also send the events to other platform if the users allows to shares analytics data
            if (JuicyPrivacyManager.IsAllowedToTrackData)
            {
                firebaseAnalyticsManager?.SendEvent(eventName, properties);
                //facebookSDKManager?.SendEvent(eventName, properties);
                emulationManager?.SendEvent(eventName, properties);
            }

            eventIndex++;
        }

        public void TrackMonetizationEvent(JuicyMonetizationEventType eventType, params EventProperty[] properties)
        {
            TrackMonetizationEvent(eventType, JuicySnapshotFlag.Complete, properties);
        }

        public void TrackAdRevenue(string adPlatform, string networkName, string adUnitIdentifier, string adFormat, double revenue, string currency)
        {
            appsFlyerManager?.SendAdRevenue(networkName, revenue, currency);
            firebaseAnalyticsManager?.SendAdImpressionEvent(adPlatform, networkName, adUnitIdentifier, adFormat, revenue, currency);
        }
        #endregion //Event Tracking
        #endregion // Public
        #region Private
        #region Unity
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

        void OnApplicationQuit()
        {
            OnSessionEnd();
        }

        void OnDestroy()
        {
            JuicyPurchaseManager.onProductDelivery -= OnProductDelivery;
        }
        #endregion //Unity
        #region Initialisation
        void CreateAnalyticsManagers()
        {
            if (Application.isEditor)
                CreateEmulatedAnalytics();
            else
            {
#if !noTenjin
                CreateAppsFlyer();
#endif
#if !noFacebook
                CreateFacebookSDK();
#endif
#if !noFirebase
                CreateFirebaseAnalytics();
#endif
#if Adjust
                CreateAdjust();
#endif
            }
        }

        void CreateAppsFlyer()
        {
            appsFlyerManager = JuicyUtility.CreateSubManager<AppsFlyerManager>(transform);
            appsFlyerManager.gameObject.AddComponent<AppsFlyerSDK.AppsFlyer>();
            appsFlyerManager.Initialize();
        }

        void CreateFacebookSDK()
        {
            facebookSDKManager = JuicyUtility.CreateSubManager<FacebookSDKManager>(transform);
            facebookSDKManager.Initialize();
        }

        void CreateFirebaseAnalytics()
        {
            firebaseAnalyticsManager = JuicyUtility.CreateSubManager<FirebaseAnalyticsManager>(transform);
            firebaseAnalyticsManager.Initialize();
        }

        void CreateEmulatedAnalytics()
        {
            emulationManager = JuicyUtility.CreateSubManager<JuicyAnalyticsEmulationManager>(transform);
            emulationManager.Initialize();
        }
        #endregion //Initiallisation
        void OnSessionStart()
        {
            JuicySnapshot.SessionCount++;

            if (JuicySnapshot.FirstInstallAppVersion == "undefined")
                JuicySnapshot.FirstInstallAppVersion = Application.version;
            if (JuicySnapshot.FirstInstallJuicyVersion == "undefined")
                JuicySnapshot.FirstInstallJuicyVersion = JuicySDK.version;
            if (installDate == DateTime.MinValue)
                installDate = DateTime.Now;

            TrackAnalyticsEvent("juicysdk_session_start", new EventProperty("lat_status", JuicyPrivacyManager.GetLATStatus()));
        }

        void OnSessionEnd()
        {
            TrackAnalyticsEvent("juicysdk_session_end");
        }

        void OnProductDelivery(ProductSummary productSummary)
        {
            if (!JuicyPrivacyManager.IsAllowedToTrackData)
            {
                JuicySDKLog.Verbose("JuicyAnalyticsManager : TrackEvent : eventName = productDelivery can't be send because of privacy settings");
                return;
            }

            JuicySDKLog.Verbose("JuicyAnalyticsManager : OnProductDelivery : productId = " + productSummary.productId);

            appsFlyerManager?.NotifyProductDelivery(productSummary);
        }

        string GetEventNameForType(JuicyMonetizationEventType eventType)
        {
            switch (eventType)
            {
                case JuicyMonetizationEventType.InterstitialTrigger:
                    return "interstitial_trigger";
                case JuicyMonetizationEventType.InterstitialStart:
                    return "interstitial_start";
                case JuicyMonetizationEventType.RewardedTrigger:
                    return "rewarded_trigger";
                case JuicyMonetizationEventType.RewardedStart:
                    return "rewarded_start";
                default:
                    return "unknow";
            }
        }

        public bool FirstInstallLessThan24hAgo()
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = now - installDate;
            return diff.TotalHours < 24;
        }

#if UNITY_IOS
        void UpdateIOSConversionValue()
        {
            if (DateTime.Now.Subtract(installDate).TotalDays >= 1)
            {
                isConversionEventSend = true;
                OnConversionValueUpdate -= UpdateIOSConversionValue;
                return;
            }

            float revenue = JuicySnapshot.CurrentRevenue;
            int newConversionValue = conversionValue;

            //Get the corresponding conversionValue
            for(int i=conversionValue; i < conversionValuesMap.Length; i++)
            {
                if (conversionValuesMap[i]/100f < revenue)
                    newConversionValue = i;
                else
                    break;
            }

            if (newConversionValue > conversionValue)
                conversionValue = newConversionValue;
            else
                return;
            
            appsFlyerManager?.UpdateIOSConversionValue(conversionValue);
            JuicySDKLog.Verbose("JuicyAnalyticsManager : UpdateConversionValue : Value updated to " + conversionValue);

            if (conversionValue >= 63)
            {
                isConversionEventSend = true;
                OnConversionValueUpdate -= UpdateIOSConversionValue;
            }
        }
#endif
        #endregion
        #endregion
    }
}