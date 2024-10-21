using System;

namespace Hack
{
    public static class JuicySDKHack
    {
        /*
        
        // HACK_SEV JuicySDK.cs line 9
        public static bool IsInterstitialAvailable => (AdsManager != null) ? AdsManager.IsInterstitialAvailable : false;
        public static bool IsInterstitialAllowedToBeDisplayed => (AdsManager != null) ? AdsManager.IsInterstitialAllowedToBeDisplayed : false;

        // HACK_SEV JuicyAdsManager.cs line 15
        public bool IsInterstitialAvailable => (adsManager == null) ? false
            : isInterstitialAvailable;

        public bool IsInterstitialAllowedToBeDisplayed => (adsManager == null) ? false
            : (ElapsedTimeSinceLastInterstitial >= JuicySDK.Settings.DelayBetweenInterstitial);

        float ElapsedTimeSinceLastInterstitial => (float)DateTime.Now.Subtract(lastInterstitialTime).TotalSeconds;
        
         */
        public static bool IsInterstitialAvailable => Juicy.JuicySDK.IsInterstitialAvailable;
        public static bool IsInterstitialAllowedToBeDisplayed => Juicy.JuicySDK.IsInterstitialAllowedToBeDisplayed;


        /* Hack JuicySDK.cs line 27
        public static bool HasPurchaseFailedPatch => true;

        */
        private static bool HasPurchaseFailedPatch => Juicy.JuicySDK.HasPurchaseFailedPatch;
    }
}
