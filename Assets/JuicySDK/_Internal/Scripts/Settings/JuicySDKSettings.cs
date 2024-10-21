using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Juicy;

namespace JuicyInternal
{

    public enum BannerPosition
    {
        Top,
        Bottom,
    }



    [DefaultExecutionOrder(-32000)]
    public class JuicySDKSettings : ScriptableObject
    {
        public const string version = "4.0";
        public const string documentation = "https://docs.google.com/document/d/15ZirbaxkJYc2xewoh1IGc2wcO9Lio1BXmDW1m5i2u0k/edit?usp=sharing";
        static string[] supportedUnityVersion = new string[3] { "2019", "2020", "2021" };
        public static string[] SupportedUnityVersion { get { return supportedUnityVersion; } }

        static JuicySDKSettings instance;
        public static JuicySDKSettings Instance
        {
            get
            {
                if (instance == null)
                    CreateSettings();
                return instance;
            }
        }

        const string SettingsResourceFolderPath = "Assets/JuicySDKSettings/Resources";
        const string SettingsFilePath = "Assets/JuicySDKSettings/Resources/JuicySDKSettings.asset";

        public static bool IsDebugMode
        {
            get
            {
#if debugJuicySDK
                return true;
#else
                return false;
#endif
            }
        }
        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        //

        #region AppConfig
        [SerializeField]
        string appConfigFileName = "JuicySDKSampleAppConfig";
        public const string dflt_AppConfigFileName = "JuicySDKSampleAppConfig";
        public string AppConfigFileName { get { return appConfigFileName; } }

        public JuicySDKConfig BaseConfig;

#if !noJuicyCompilation
        public JuicySDKMediationConfig MediationConfig;
#endif

        #endregion

        #region Settings
        #region Mediation
        [SerializeField][Tooltip("Override default mediation parameters: don't do this unless it was requested by the Juicy team")]
        bool overrideDefaultMediationParameters = false;
        public bool OverrideDefaultMediationParameters
        {
            get
            {
                return overrideDefaultMediationParameters;
            }
        }


        [SerializeField][Tooltip("Set the banner position")]
        BannerPosition bannerPosition = BannerPosition.Bottom;
        public const BannerPosition dflt_BannerPosition = BannerPosition.Bottom;
        public BannerPosition BannerPosition
        {
            get
            {
                return bannerPosition;
            }
        }
        
        [SerializeField][Tooltip("Whether or not a sucessfull rewarded reset the interstitial timer")]
        bool noInterstitialAfterRewarded = true;
        public const bool dflt_NoInterstitialAfterRewarded = true;
        public bool NoInterstitialAfterRewarded
        {
            get
            {
                if (OverrideDefaultMediationParameters)
                    return noInterstitialAfterRewarded;
                else
                    return dflt_NoInterstitialAfterRewarded;
            }
        }

        [SerializeField]
        [Tooltip("Minimal delay between 2 interstitials")]
        int delayBetweenInterstitial = 30;
        public const int dflt_DelayBetweenInterstitial = 30;
        public int DelayBetweenInterstitial
        {
            get
            {
                if (enableAbTest)
                {
                    if (abTestType == JuicyABTestType.Mediation)
                    {
                        switch (JuicyABTestManager.Instance.CohortIndex)
                        {
                            case 1:
                                return 5;
                            case 2:
                                return 15;
                        }
                    }
                }

                if (OverrideDefaultMediationParameters)
                    return delayBetweenInterstitial;
                else
                    return dflt_DelayBetweenInterstitial;
            }
        }
        #endregion
        #region Privacy
        [SerializeField]
        [Tooltip("Override default privacy parameters: don't do this unless it was requested by the Juicy team")]
        bool overrideDefaultPrivacyParameters = false;
        public bool OverrideDefaultPrivacyParameters
        {
            get
            {
                return overrideDefaultPrivacyParameters;
            }
        }

        [SerializeField][Tooltip("Disable GDPR management, ie there will be no GDPR popup at first install and the SDK will act as if the user as consent to everything related to the use of his datas")]
        bool disableGDPRManagement = false;
        public const bool dflt_DisableGDPRManagement = false;
        public bool DisableGDPRManagement
        {
            get
            {
                return disableGDPRManagement;
            }
        }

        [SerializeField]
        [Tooltip("Display the iOS tracking authorization pop up")]
        bool displayIosTrackingAuthorization = true;
        public const bool dflt_DisplayIosTrackingAuthorization = true;
        public bool DisplayIosTrackingAuthorization
        {
            get
            {
                return displayIosTrackingAuthorization;
            }
        }

        [SerializeField]
        [Tooltip("Display the Juicy tracking pop up before the iOS one")]
        bool displayJuicyIosTrackingPopUp = false;
        public const bool dflt_DisplayJuicyIosTrackingPopUp = false;
        public bool DisplayJuicyIosTrackingPopUp
        {
            get
            {
                return displayJuicyIosTrackingPopUp;
            }
        }

        #endregion
        #region In App purchase
        [SerializeField]
        [Tooltip("Should the restore purchases be available in editor")]
        bool restorePurchaseAvailableInEditor = true;
        const bool dflt_RestorePurchaseAvailableInEditor = true;
        public bool RestorePurchaseAvailableInEditor
        {
            get
            {
                return restorePurchaseAvailableInEditor;
            }
        }

        [SerializeField]
        [Tooltip("Force a cancel during a purchase inside the editor, use this to test the behaviour of your app when this occurs")]
        bool forcePurchaseCancelInEditor = false;
        const bool dflt_ForcePurchaseCancelInEditor = false;
        public bool ForcePurchaseCancelInEditor
        {
            get
            {
                return forcePurchaseCancelInEditor;
            }
        }

        [SerializeField]
        [Tooltip("ID for the Remoe Ads Product (the remove ads is considered as a normal buyable product thus it needs an ID)")]
        string removeAdsProductID = "removeads";
        const string dflt_RemoveAdsProductID = "removeads";
        public string RemoveAdsProductID
        {
            get
            {
                return removeAdsProductID;
            }
        }

        [SerializeField][Tooltip("List of all the products that are buyable through your app")]
        List<ProductInfos> otherProducts = new List<ProductInfos>()
        {
            new ProductInfos("consumable01",ProductType.Consumable),
            new ProductInfos("nonconsumable01",ProductType.NonConsumable)
        };
        static List<ProductInfos> dflt_OtherProducts = new List<ProductInfos>()
        {
            new ProductInfos("consumable01",ProductType.Consumable),
            new ProductInfos("nonconsumable01",ProductType.NonConsumable)
        };
        public List<ProductInfos> Products
        {
            get
            {
                ProductInfos removeAdsProduct = new ProductInfos(removeAdsProductID, ProductType.Consumable);
                List<ProductInfos> products = new List<ProductInfos>() { removeAdsProduct };
                products.AddRange(otherProducts);
                return products;
            }
        }
        #endregion
        #region Editor
        [SerializeField][Tooltip("Emulated ads are triggered but not showed (ex: the rewarded are still called but closed immediately with a success response")]
        bool skipAdsInEditor = false;
        const bool dflt_SkipAdsInEditor = false;
        public bool SkipAdsInEditor
        {
            get
            {
                return skipAdsInEditor;
            }
        }
        #pragma warning disable 414
        [SerializeField][Tooltip("Show the JuicySDK related logs (editor only)")]
        bool showJuicySDKLogInEditor = true;
        const bool dflt_ShowJuicySDKDLogInEditor = true;
        #pragma warning restore 414
        public bool ShowLogInEditor
        {
            get
            {
                return showJuicySDKLogInEditor;
            }
        }
        #endregion
        #region AB Test
        public const int ABTESTMAXPOPULATION = 20;
        [SerializeField][Tooltip("Whether this release of your app has an AB test")]
        bool enableAbTest = false;
        public bool EnableAbTest
        {
            get
            {
                return enableAbTest;
            }
        }

        [SerializeField][Tooltip("")]
        JuicyABTestType abTestType = JuicyABTestType.Custom;
        const JuicyABTestType dflt_abTestType = JuicyABTestType.Custom;
        public JuicyABTestType AbTestType
        {
            get
            {
                return abTestType;
            }
        }

        
        [SerializeField][Tooltip("")]
        bool abTestKeepVariantIndex = false;
        public const bool dflt_keepVariantIndex = false;
        public bool AbTestKeepVariantIndex
        {
            get
            {
                return abTestKeepVariantIndex;
            }
        }

        [SerializeField][Tooltip("Amount of variant for the current AB test")]
        int abTestVariantAmount = 1;
        public const int dflt_AbTestVariantAmount = 1;
        public int AbTestVariantAmount
        {
            get
            {
                return abTestVariantAmount;
            }
        }
        public string AbTestName
        {
            get
            {
                if (!enableAbTest)
                    return "None";

                return abTestType.ToString() + ": " +  Application.version;
            }
        }
        [SerializeField][Tooltip("Whether or not to force an AB Test variant in editor")]
        bool forceAbTestVariantInEditor = false;
        public const bool dflt_ForceAbTestVariantInEditor = false;
        public bool ForceABTestVariantInEditor
        {
            get
            {
                return IsEditor && forceAbTestVariantInEditor;
            }
        }
        [SerializeField][Tooltip("AB test variant to force in editor")]
        int abTestVariantToForce = 1;
        public const int dflt_AbTestVariantToForce = 1;
        public int AbTestVariantToForce
        {
            get
            {
                return abTestVariantToForce;
            }
        }
        [SerializeField][Tooltip("Override default AB test parameters: don't do this unless it was requested by the Juicy team")]
        bool overrideDefaultAbTestParameters = false;
        public bool OverrideDefaultAbTestParameters
        {
            get
            {
                return overrideDefaultAbTestParameters;
            }
        }
        [SerializeField][Tooltip("Percentage of the population in each variant")]
        [Range(5, 20)]
        int abTestVariantPopulation = 5;
        public const int dflt_AbTestVariantPopulation = 5;
        public int AbTestVariantPopulation
        {
            get
            {
                if (overrideDefaultAbTestParameters)
                    return abTestVariantPopulation;
                else
                    return dflt_AbTestVariantPopulation;
            }
        }
        #endregion
        #region Debug
        [SerializeField][Tooltip("Use the Verbose logs (only if show logs in editor is check). Use this for tests only, a lot a stuff will be logged and it might slow down your app")]
        bool verboseLogs = false;
        const bool dflt_VerboseLogs = false;
        public bool UseVerboseLogs
        {
            get
            {
                 return verboseLogs && IsDebugMode;
            }
        }
        [SerializeField][Tooltip("Make the SDK act as if the Remove Ads has been bought")]
        bool debugForceRemoveAds = false;
        const bool dflt_DebugForceRemoveAds = false;
        public bool DebugForceRemoveAds
        {
            get
            {
                return IsDebugMode && debugForceRemoveAds;
            }
        }
        #pragma warning disable 414
        [SerializeField][Tooltip("Trigger the rating popup opportunity every game without any delay or postpone time (if you already have rated the app you'll have to delete the juicy player prefs to test it again in editor or reinstall the app on device)")]
        bool debugRating = false;
        const bool dflt_DebugRating = false;
        #pragma warning restore 414
        public bool DebugRating
        {
            get
            {
                return IsDebugMode && debugRating;
            }
        }
        [SerializeField][Tooltip("Force the AbTest cohort variant index in build")]
        bool forceAbTestVariantInBuild = false;
        public bool ForceAbTestVariantInBuild
        {
            get
            {
                return IsDebugMode && forceAbTestVariantInBuild;
            }
        }
        #endregion
        #endregion

        #region Methods
        static void CreateSettings()
        {
            instance = Resources.Load<JuicySDKSettings>("JuicySDKSettings");

#if UNITY_EDITOR
            if (instance == null)
            {
                if (!Directory.Exists(SettingsResourceFolderPath))
                {
                    Directory.CreateDirectory(SettingsResourceFolderPath);
                }

                JuicySDKSettings asset = ScriptableObject.CreateInstance<JuicySDKSettings>();
                AssetDatabase.CreateAsset(asset, SettingsFilePath);
                AssetDatabase.Refresh();

                AssetDatabase.SaveAssets();
                JuicySDKLog.Log("Juicy SDK: Settings file didn't exist and was created");
                instance = asset;

                JuicyConfigFileManager.LoadDefaultConfig();
            }
#endif
        }

        public void LoadAppConfig()
        {
            BaseConfig = new JuicySDKConfig(AppConfigFileName);

            #if !noJuicyCompilation
            MediationConfig = new JuicySDKMediationConfig(AppConfigFileName);
            #endif
        }

#if UNITY_EDITOR
        public string GetConfigFileLog()
        {
            string nl = System.Environment.NewLine;
            LoadAppConfig();
            string configLog = BaseConfig.GetLog();

#if !noJuicyCompilation
            configLog += MediationConfig.GetLog();
#endif
            return configLog;
        }

        public void ResetSettingsToDefault()
        {
            //Mediation
            overrideDefaultMediationParameters = false;
            bannerPosition = dflt_BannerPosition;
            noInterstitialAfterRewarded = dflt_NoInterstitialAfterRewarded;
            delayBetweenInterstitial = dflt_DelayBetweenInterstitial;
            //Privacy
            overrideDefaultPrivacyParameters = false;
            disableGDPRManagement = dflt_DisableGDPRManagement;
            displayIosTrackingAuthorization = dflt_DisplayIosTrackingAuthorization;
            displayJuicyIosTrackingPopUp = dflt_DisplayJuicyIosTrackingPopUp;
            //IAP
            forcePurchaseCancelInEditor = dflt_ForcePurchaseCancelInEditor;
            restorePurchaseAvailableInEditor = dflt_RestorePurchaseAvailableInEditor;
            removeAdsProductID = dflt_RemoveAdsProductID;
            otherProducts = new List<ProductInfos>(dflt_OtherProducts);
            //Editor
            showJuicySDKLogInEditor = dflt_ShowJuicySDKDLogInEditor;
            skipAdsInEditor = dflt_SkipAdsInEditor;
            //ABTest
            enableAbTest = false;
            abTestKeepVariantIndex = dflt_keepVariantIndex;
            abTestType = JuicyABTestType.Custom;
            abTestVariantAmount = dflt_AbTestVariantAmount;
            forceAbTestVariantInEditor = dflt_ForceAbTestVariantInEditor;
            abTestVariantToForce = dflt_AbTestVariantToForce;
            overrideDefaultAbTestParameters = false;
            abTestVariantPopulation = dflt_AbTestVariantPopulation;
            //Debug
            debugForceRemoveAds = dflt_DebugForceRemoveAds;
            debugRating = dflt_DebugRating;
            verboseLogs = dflt_VerboseLogs;
            forceAbTestVariantInBuild = false;

            JuicyConfigFileManager.LoadDefaultConfig();
        }

        public void OnProductionModeSwitch()
        {
            removeAdsProductID = "";
            otherProducts.Clear();
        }

        public void ConfigureSettingsForABTest(JuicyABTestType abTestType)
        {
            this.abTestType = abTestType;
            switch (abTestType)
            {
                case JuicyABTestType.Custom:
                    abTestVariantAmount = 1;
                    break;
                case JuicyABTestType.Mediation:
                    abTestVariantAmount = 2;
                    break;
            }

            overrideDefaultAbTestParameters = false;
            abTestVariantPopulation = dflt_AbTestVariantPopulation;
        }

   
#endif
#endregion
        }

    
}
