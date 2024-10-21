using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace JuicyInternal
{
    [System.Serializable]
    public class JuicySDKConfig
    {
        public string AppBundleID { get; private set; } = "";
        public string FacebookAppName { get; private set; } = "";
        public string FacebookAppID { get; private set; } = "";
        public string FacebookClientToken { get; private set; } = "";
        public string PrivacyPolicyLink { get; private set; } = "";
        public string AppStoreId { get; private set; } = "";

        public JuicySDKConfig (string fileName)
        {
            XmlDocument appConfig = new XmlDocument();
            if (!XmlUtility.LoadXmlFromResources(fileName, out appConfig))
                JuicySDKLog.LogError("JuicySDKConfig : No config file with name " + fileName+ " found in resources");

            XmlNode settingsNode = XmlUtility.GetNode(appConfig, "Settings");
            //--Infos--
            XmlNode infoNode = XmlUtility.GetNode(settingsNode, "Infos");
            AppBundleID = XmlUtility.GetNode(infoNode, "BundleID").InnerText;
            AppStoreId = XmlUtility.GetNode(infoNode, "AppStoreID").InnerText;
            PrivacyPolicyLink = XmlUtility.GetNode(infoNode, "PrivacyPolicyLink").InnerText;
            //--Ananlytics--
            XmlNode analyticsNode = XmlUtility.GetNode(settingsNode, "Analytics");
            //Facebook
            XmlNode facebookNode = XmlUtility.GetNode(analyticsNode, "Facebook");
            FacebookAppID = XmlUtility.GetNode(facebookNode, "AppID").InnerText;
            FacebookAppName = XmlUtility.GetNode(facebookNode, "AppName").InnerText;
            FacebookClientToken = XmlUtility.GetNode(facebookNode, "ClientToken").InnerText;
        }

        public string GetLog()
        {
            string nl = System.Environment.NewLine;
            string configLog = "";
            configLog += "Infos" + nl;
            configLog += "Bundle ID: " + AppBundleID + nl;
            configLog += "Privacy Policy Link: " + PrivacyPolicyLink + nl;
            configLog += "AppStore ID: " + AppStoreId + nl;
            configLog += "Analytics" + nl;
            configLog += "Facebook App Name: " + FacebookAppName + nl;
            configLog += "Facebook App ID: " + FacebookAppID + nl;
            configLog += "Facebook Client Token: " + FacebookClientToken + nl;
            return configLog;
        }


    }
}
