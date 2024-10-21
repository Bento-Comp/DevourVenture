using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;
#if UNITY_IOS
using Juicy.IOS;
#endif
#if UNITY_ANDROID
using Juicy.Android;
#endif


namespace JuicyInternal
{
    public enum ATTStatus
    {
        Error = -1,
        Authorized = 0,
        NotDetermined= 1,
        Restricted = 2,
        Denied = 3
    }

    [DefaultExecutionOrder(-31600)]
    [AddComponentMenu("JuicySDKInternal/JuicyPrivacyManager")]
    public class JuicyPrivacyManager : MonoBehaviour
    {
        #region Variables
        const string PRIVACY_MAIL_ADDRESS = "gdpr@juicy-publishing.com";
        const string PRIVACY_MAIL_SUBJECT = "GDPR data management request";
        //Rereferences
        [SerializeField] PrivacySettingsPopUp firstOpenPopUpPrefab = null;
        [SerializeField] PrivacySettingsPopUp settingsPopUpPrefab = null;
        #pragma warning disable CS0414
        [SerializeField] PreIOSIDFATrackingPopUp preIOSPopUpPrefab = null;
        #pragma warning restore CS0414
        [SerializeField] GameObject inputMaskPrefab = null;
        PrivacySettingsPopUp firstOpenPopUp = null;
        PrivacySettingsPopUp settingsPopUp = null;
#if UNITY_IOS
        PreIOSIDFATrackingPopUp preIOSPopUp = null;
#endif
        GameObject inputMask = null;

        //Analitycs
        static bool _AnalyticsEnabled { get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.ANALYTICS_ENABLED);}
                                        set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.ANALYTICS_ENABLED,value); }}

        public static bool AnalyticsEnabled { get { return _AnalyticsEnabled; } }

        public static bool IsAllowedToTrackData
        {
            get
            {
                if (JuicySDK.Settings.DisableGDPRManagement)
                    return true;
                else
                    return AnalyticsEnabled && AgeEnabled;
            }
        }

        //Ads
        static bool _AdsEnabled { get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.ADS_ENABLED); }
                                  set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.ADS_ENABLED, value); } }

        public static bool AdsEnabled { get { return _AdsEnabled; } }

        public static bool IsAllowedToPersonalizedAds
        {
            get
            {
                if (JuicySDK.Settings.DisableGDPRManagement)
                    return true;
                else
                    return AdsEnabled && AgeEnabled;
            }
        }

        //Age
        static bool _AgeEnabled { get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.AGE_ENABLED); }
                                  set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.AGE_ENABLED, value); } }

        public static bool AgeEnabled { get { return _AgeEnabled; } }

        public static bool IsAgeAllowed
        {
            get
            {
                if (JuicySDK.Settings.DisableGDPRManagement)
                    return true;
                else
                    return AgeEnabled;
            }
        }

        //Others
        static JuicyPrivacyManager instance;
        public static JuicyPrivacyManager Instance { get { return instance; } }

        static bool _HasBeenWelcomed { get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.HAS_BEEN_WELCOMED); }
                                       set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.HAS_BEEN_WELCOMED,value); } }

        public Action<bool,bool> onPrivacySettingsChange;

        #endregion
        #region Methods
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                JuicySDKLog.LogWarning("A singleton can only be instantiated once!");
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            Initialize();
        }

        void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        #region Public

        public static bool GetLATStatus()
        {
            if (Application.isEditor)
                return true;

#if UNITY_IOS
            ATTStatus status = (ATTStatus)JuicyIOSPlugin.GetATTStatus();
            if (JuicyIOSPlugin.IsIOS14Dot5OrAbove())
            {
                return status != ATTStatus.Authorized;
            }

            else
            {
                return (status == ATTStatus.Denied || status == ATTStatus.Restricted);
            }
#elif UNITY_ANDROID
            return JuicyAndroidPlugin.GetLATStatus();
#else
            return true;
#endif
        }

        public static string GetAdIdentifiant()
        {
            if (Application.isEditor)
                return "0000-0000-0000-0000";

#if UNITY_IOS
            return JuicyIOSPlugin.GetIDFA();
#elif UNITY_ANDROID
            return JuicyAndroidPlugin.GetGAID();
#else
            return "";
#endif
        }
        public void ShowPrivacySettings()
        {
            if(!JuicySDK.Settings.DisableGDPRManagement)
                settingsPopUp.Open();
        }

        public void OnWelcomePopUpCompleted(bool ads, bool analytics, bool age)
        {
            _AdsEnabled = ads;
            _AnalyticsEnabled = analytics;
            _AgeEnabled = age;

            _HasBeenWelcomed = true;
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("privacy_settings", new EventProperty("analyticsEnabled", AnalyticsEnabled),
                                                                new EventProperty("adsEnabled", AdsEnabled),
                                                                new EventProperty("ageEnabled", AgeEnabled),
                                                                new EventProperty("firstIntention", false));

            GDPROver();
        }

        public void UpdatePrivacySettings(bool ads, bool analytics, bool age)
        {
            _AdsEnabled = ads;
            _AnalyticsEnabled = analytics;
            _AgeEnabled = age;

            JuicyAdsManager.Instance.UpdatePrivacySettings();
            JuicyAnalyticsManager.Instance.UpdatePrivacySettings();
            JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("privacy_settings",   new EventProperty("analyticsEnabled", AnalyticsEnabled),
                                                                            new EventProperty("adsEnabled", AdsEnabled),
                                                                            new EventProperty("ageEnabled", AgeEnabled),
                                                                            new EventProperty("firstIntention", false));

            onPrivacySettingsChange?.Invoke(IsAllowedToTrackData, IsAllowedToPersonalizedAds);
        }

        public void SendForgetMeMail()
        {
            string nl = System.Environment.NewLine;
            string mailBody = "Hello Juicy!" + nl + "I want you to remove my personal data from your database." + nl + nl;
            mailBody += "My IDFA/GAID: " + GetAdIdentifiant() + nl + nl;
            mailBody += "Have a nice day!";
            JuicyMailTo.SendMail(PRIVACY_MAIL_ADDRESS, PRIVACY_MAIL_SUBJECT, mailBody);
        }

        public void SendAccessDatasMail()
        {
            string nl = System.Environment.NewLine;
            string mailBody = "Hello Juicy!" + nl + "I want to access the data you have about me." + nl + nl;
            mailBody += "My IDFA/GAID: " + GetAdIdentifiant() + nl + nl;
            mailBody += "Have a nice day!";
            JuicyMailTo.SendMail(PRIVACY_MAIL_ADDRESS, PRIVACY_MAIL_SUBJECT, mailBody);
        }

#if UNITY_IOS
        public ATTStatus GetIDFATrackingStatus()
        {
            if (Application.isEditor)
                return ATTStatus.Authorized;

            int intStatus = JuicyIOSPlugin.GetATTStatus();
            if (intStatus < 0 || intStatus > 3)
            {
                JuicySDKLog.LogError("JuicyPrivacyManager : GetIDFATrackingStatus : Cannot read the status of the authorization");
                return ATTStatus.Error;
            }

            ATTStatus status = (ATTStatus)intStatus;
            return status;
        }

        public void CallATTPopUp()
        {
            JuicyIOSPlugin.CallATTPopUp("JuicyPrivacyManager", "OnATTSet");
        }
#endif

#endregion
#region Private
        void Initialize()
        {
            InstantiateInputMask();
#if UNITY_IOS
            if (!DisplayIOSATTPopUp())
                InitializeGDPR();
#else
            InitializeGDPR();
#endif

        }

#if UNITY_IOS
        bool DisplayIOSATTPopUp()
        {
            if (JuicyIOSPlugin.IsIOS14Dot5OrAbove() && JuicySDK.Settings.DisplayIosTrackingAuthorization)
            {
                if (JuicyIOSPlugin.GetATTStatus() == (int)ATTStatus.NotDetermined)
                {
                    if (JuicySDK.Settings.DisplayJuicyIosTrackingPopUp)
                        DisplayPreIOSPopUp();
                    else
                        CallATTPopUp();

                    return true;
                }
            }

            return false;
        }

        void DisplayPreIOSPopUp()
        {
            preIOSPopUp = Instantiate(preIOSPopUpPrefab);
            preIOSPopUp.transform.SetParent(transform.GetChild(0));
            preIOSPopUp.transform.localScale = Vector3.one;
            preIOSPopUp.GetComponent<RectTransform>().FillParent();
        }

        void OnATTSet(string message)
        {
            int intStatus = 0;
            if (!int.TryParse(message, out intStatus))
            {
                JuicySDKLog.LogError("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Cannot read the status of the authorization");
                InitializationOver();
                return;
            }

            if (intStatus < 0 || intStatus > 3)
            {
                JuicySDKLog.LogError("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Cannot read the status of the authorization");
                InitializationOver();
                return;
            }

            ATTStatus status = (ATTStatus)intStatus;
            switch (status)
            {
                case ATTStatus.Authorized:
                    JuicySDKLog.Log("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Tracking status = Authorized");
                    break;
                case ATTStatus.Denied:
                    JuicySDKLog.Log("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Tracking status = Denied");
                    break;
                case ATTStatus.NotDetermined:
                    JuicySDKLog.Log("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Tracking status = Not Determined");
                    break;
                case ATTStatus.Restricted:
                    JuicySDKLog.Log("JuicyPrivacyManager : OnIDFATrackingAUthorizationSet : Tracking status = Restricted");
                    break;
            }

            InitializeGDPR();
        }
#endif
        void InstantiateInputMask()
        {
            inputMask = Instantiate(inputMaskPrefab);
            inputMask.transform.SetParent(transform.GetChild(0));
            inputMask.transform.localScale = Vector3.one;
            inputMask.GetComponent<RectTransform>().FillParent();
        }

        void DestroyInputMask()
        {
            if (inputMask != null)
                Destroy(inputMask);
        }

        void InitializeGDPR()
        {
            if (JuicySDK.Settings.DisableGDPRManagement)
            {
                GDPROver();
                return;
            }

            //Set up GDPR settings pop up
            SetUpGDPRUpdatePopUp();
            //If GDPR hasn't been filled yet display the GDPR pop up
            if (_HasBeenWelcomed)
                GDPROver();
            else
                DisplayGDPRWelcomePopUp();
        }

        void DisplayGDPRWelcomePopUp()
        {
            firstOpenPopUp = Instantiate(firstOpenPopUpPrefab);
            firstOpenPopUp.transform.SetParent(transform.GetChild(0));
            firstOpenPopUp.transform.localScale = Vector3.one;
            firstOpenPopUp.GetComponent<RectTransform>().FillParent();
            firstOpenPopUp.Open();
        }

        void SetUpGDPRUpdatePopUp()
        {
            settingsPopUp = Instantiate(settingsPopUpPrefab);
            settingsPopUp.transform.SetParent(transform.GetChild(0));
            settingsPopUp.transform.localScale = Vector3.one;
            settingsPopUp.GetComponent<RectTransform>().FillParent();
            settingsPopUp.Close();
        }

        void GDPROver()
        {
            InitializationOver();
        }

        void InitializationOver()
        {
            JuicySDKLog.Log("JuicyPrivacyManager : InitializationOver");
            DestroyInputMask();
            JuicySDKManager.Instance.OnPrivacySet();
        }
#endregion
#endregion
    }
}
