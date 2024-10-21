using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
    public class JuicyPlayerPrefs
    {
        const string JUICY_PLAYERPREFS_PREFIX = "JuicySDK_";

        /*--- RemoveAds ---*/
        public const string ADS_REMOVED = "AdsRemoved";

        /*--- SDK ---*/
        public const string BEST_SCORE = "BestScore";

        /*--- Settings ---*/
        public const string USE_VERBOSE_LOGS = "UseVerboseLogs";

        /*--- JuicySnapShot ---*/
        public const string FIRST_INSTALL_APP_VERSION = "FirstInstallAppVersion";
        public const string FIRST_INSTALL_JUICY_VERSION = "FirstInstallJuicyVersion";
        public const string TOTAL_GAME_COUNT = "TotalGameCount";
        public const string TOTAL_SESSION_COUNT = "TotalSessionCount";
        public const string TOTAL_BANNER_COUNT = "TotalBannerCount";
        public const string TOTAL_INTERSTITIAL_COUNT = "TotalInterstitialCount";
        public const string TOTAL_REWARDED_COUNT = "TotalRewardedCount";
        public const string TOTAL_GAME_TIME = "TotalGameTime";
        public const string TOTAL_APPLICATION_TIME = "TotalApplicationTime";
        public const string TOTAL_REAL_TIME = "TotalRealTime";

        /*--- Privacy ---*/
        public const string ANALYTICS_ENABLED = "AnalyticsEnabled";
        public const string ADS_ENABLED = "AdsEnabled";
        public const string AGE_ENABLED = "AgeEnabled";
        public const string HAS_BEEN_WELCOMED = "HasBeenWelcomed_1.0";

        /*--- Analytics ---*/
        public const string EVENT_INDEX = "EventIndex";
        public const string INSTALL_DATE = "InstallDate";
        public const string IS_CONVERSION_VALUE_SEND = "IsConversionValueSend";
        public const string CONVERSION_VALUE = "ConversionValue";

        /*--- ABTest ---*/
        public const string COHORT_INDEX = "CohortIndex";

        /*--- Rating ---*/
        public const string RATING_HASBEENRATED = "HasBeenRated";
        public const string RATING_PREVIOUSSHOW = "RatingPreviousShow";
        public const string RATING_COUNTER = "RatingCounter";

        /*--- Revenue ---*/
        public const string CURRENT_REVENUE = "CurrentRevenue";

        #region Methods
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(JUICY_PLAYERPREFS_PREFIX + key);
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(JUICY_PLAYERPREFS_PREFIX + key);
        }

        public static void DeleteAll()
        {
            //RemoveAds
            DeleteKey(ADS_REMOVED);
            //SDK
            DeleteKey(BEST_SCORE);
            //Settings
            DeleteKey(USE_VERBOSE_LOGS);
            //SnapShot
            DeleteKey(FIRST_INSTALL_APP_VERSION);
            DeleteKey(FIRST_INSTALL_JUICY_VERSION);
            DeleteKey(TOTAL_GAME_COUNT);
            DeleteKey(TOTAL_SESSION_COUNT);
            DeleteKey(TOTAL_BANNER_COUNT);
            DeleteKey(TOTAL_INTERSTITIAL_COUNT);
            DeleteKey(TOTAL_REWARDED_COUNT);
            DeleteKey(TOTAL_GAME_TIME);
            DeleteKey(TOTAL_APPLICATION_TIME);
            DeleteKey(TOTAL_REAL_TIME);
            //Privacy
            DeleteKey(ANALYTICS_ENABLED);
            DeleteKey(ADS_ENABLED);
            DeleteKey(AGE_ENABLED);
            DeleteKey(HAS_BEEN_WELCOMED);
            //Analytics
            DeleteKey(EVENT_INDEX);
            DeleteKey(IS_CONVERSION_VALUE_SEND);
            DeleteKey(CONVERSION_VALUE);
            DeleteKey(INSTALL_DATE);
            //ABTest
            DeleteKey(COHORT_INDEX);
            //Rating
            DeleteKey(RATING_HASBEENRATED);
            DeleteKey(RATING_PREVIOUSSHOW);
            DeleteKey(RATING_COUNTER);
            //Revenue
            DeleteKey(CURRENT_REVENUE);
        }

        #region Get
        public static bool GetBool(string key, bool def = false)
        {
            int defInt = def ? 1 : 0;
            return PlayerPrefs.GetInt(JUICY_PLAYERPREFS_PREFIX + key, defInt) == 1 ? true : false;
        }

        public static int GetInt(string key, int def = 0)
        {
            return PlayerPrefs.GetInt(JUICY_PLAYERPREFS_PREFIX + key, def);
        }

        public static float GetFloat(string key, float def = 0)
        {
            return PlayerPrefs.GetFloat(JUICY_PLAYERPREFS_PREFIX + key, def);
        }

        public static string GetString(string key, string def = "")
        {
            return PlayerPrefs.GetString(JUICY_PLAYERPREFS_PREFIX + key, def);
        }
        #endregion
        #region Set
        public static void SetBool(string key, bool val)
        {
            int valInt = val ? 1 : 0;
            PlayerPrefs.SetInt(JUICY_PLAYERPREFS_PREFIX + key, valInt);
        }

        public static void SetInt(string key, int val)
        {
            PlayerPrefs.SetInt(JUICY_PLAYERPREFS_PREFIX + key, val);
        }

        public static void SetFloat(string key, float val)
        {
            PlayerPrefs.SetFloat(JUICY_PLAYERPREFS_PREFIX + key, val);
        }

        public static void SetString(string key, string val)
        {
            PlayerPrefs.SetString(JUICY_PLAYERPREFS_PREFIX + key, val);
        }
        #endregion
        #endregion
    }
}