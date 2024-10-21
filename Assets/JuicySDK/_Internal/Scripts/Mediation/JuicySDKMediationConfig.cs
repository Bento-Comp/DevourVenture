#if !noJuicyCompilation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace JuicyInternal
{
    [System.Serializable]
    public class JuicySDKMediationConfig
    {
        public const string version = "Applovin_3.3";

        public string BannerID { get; private set; } = "";
        public string InterstitialID { get; private set; } = "";
        public string RewardedID { get; private set; } = "";
        public string AdmobAppID { get; private set; } = "";

        public JuicySDKMediationConfig(string bannerID, string interstitialID, string rewardedID, string admobAppID)
        {
            this.BannerID = bannerID;
            this.InterstitialID = interstitialID;
            this.RewardedID = rewardedID;
            this.AdmobAppID = admobAppID;
        }

        public string GetLog()
        {
            string nl = System.Environment.NewLine;
            string configLog = "";
            configLog += "Mediation" + nl;
            configLog += "Banner ID: " + BannerID+ nl;
            configLog += "Interstiatial ID: " + InterstitialID + nl;
            configLog += "Rewarded ID: " + RewardedID + nl;
            configLog += "Admob App ID: " + AdmobAppID + nl;

            return configLog;
        }

        public JuicySDKMediationConfig(string fileName)
        {
            XmlDocument appConfig = new XmlDocument();
            if (!XmlUtility.LoadXmlFromResources(fileName, out appConfig))
                JuicySDKLog.LogError("JuicySDKMediationConfig : No config file with name " + fileName + " found in resources");

            XmlNode settingsNode = XmlUtility.GetNode(appConfig, "Settings");
            XmlNode mediationNode = XmlUtility.GetNode(settingsNode, "Mediation");

            XmlNode platformNode;

            #if UNITY_IOS
            platformNode = XmlUtility.GetNode(mediationNode, "iOS");
            #else
            platformNode = platformNode = XmlUtility.GetNode(mediationNode, "Android");
            #endif

            BannerID = XmlUtility.GetNode(platformNode, "ApplovinBannerID").InnerText;
            InterstitialID = XmlUtility.GetNode(platformNode, "ApplovinInterstitialID").InnerText;
            RewardedID = XmlUtility.GetNode(platformNode, "ApplovinRewardedID").InnerText;
            AdmobAppID = XmlUtility.GetNode(platformNode, "AdMobAppID").InnerText;
        }
    }
}
#endif