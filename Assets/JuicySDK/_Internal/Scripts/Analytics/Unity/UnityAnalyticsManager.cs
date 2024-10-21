using UnityEngine;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

namespace JuicyInternal
{
    //We don't use it but it is automatically added in the app with Unity IAP
    public class UnityAnalyticsManager
    {
        //Disable Unity analytics on load
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnLoad()
        {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            Analytics.initializeOnStartup = false;
#endif
        }

        public static void UpdatePrivacySettings()
        {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            if (JuicyPrivacyManager.IsAllowedToTrackData)
            {
                Analytics.ResumeInitialization();
            }

            //Not sure it is usefull since analytics shouldn't be initialized but we nver know
            else
            {
                Analytics.enabled = false;
                Analytics.deviceStatsEnabled = false;
                PerformanceReporting.enabled = false;
            }
#endif
        }    
    }
}

