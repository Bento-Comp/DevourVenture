using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity.Settings;

namespace JuicyInternal
{
    public class JuicyConfigIntegrationReport : JuicyIntegrationReport
    {
        public JuicyConfigIntegrationReport()
        {
            //Default config == no check of the config values
            if (JuicyConfigFileManager.IsConfigFileDefault)
                return;

            JuicyIntegrationReportCategory firebaseCategory = new JuicyIntegrationReportCategory("FIREBASE");
            JuicyIntegrationReportCategory facebookCategory = new JuicyIntegrationReportCategory("FACEBOOK");
            JuicyIntegrationReportCategory defaultCatgory = new JuicyIntegrationReportCategory("DEFAULT");

            JuicySDKConfig currentConfig = JuicySDKSettings.Instance.BaseConfig;
            JuicySDKConfig defaultConfig = new JuicySDKConfig(JuicySDKSettings.dflt_AppConfigFileName);

            if (JuicyConfigFileManager.IsFirebaseFileDefault())
                firebaseCategory.Add(new JuicyIntegrationReportItem("Sample Firebase file", true, "It seems that your config file is not setup for the current target platform, ask the juicy team to give you a new one. " +
                    "If you just want to test your app on the current platform without publishing it reset your config file to default."));

            if (FacebookSettings.AppIds[0] != currentConfig.FacebookAppID)
                facebookCategory.Add(new JuicyIntegrationReportItem("Incorrect Facebook AppID", true,
                    "It seems that the Facebook AppID of your project doesn't match the one in your config file, please contact the juicy team"));

            if (FacebookSettings.ClientTokens[0] != currentConfig.FacebookClientToken)
                facebookCategory.Add(new JuicyIntegrationReportItem("Incorrect Facebook Client Token", true,
                    "It seems that the Facebook Client Token of your project doesn't match the one in your config file, please contact the juicy team"));

            //Default config
            if (currentConfig.AppBundleID == defaultConfig.AppBundleID)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default App Bundle ID", true,
                    "It seems that there's an error with your config file, please contact the Juicy team"));
            if (currentConfig.FacebookAppID == defaultConfig.FacebookAppID)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default Facebook ID", true,
                    "It seems that there's an error with your config file, please contact the Juicy team"));
            if (currentConfig.FacebookAppName == defaultConfig.FacebookAppName)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default Facebook App Name",
                    true, "It seems that there's an error with your config file, please contact the Juicy team"));
            if (currentConfig.FacebookClientToken == defaultConfig.FacebookClientToken)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default Facebook Client Token", true,
                    "It seems that there's an error with your config file, please contact the Juicy team"));
#if UNITY_IOS
            if (currentConfig.AppStoreId == defaultConfig.AppStoreId)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default AppStore ID", true, "It seems that there's an error with your config file, please contact the Juicy team"));
#endif
            if (currentConfig.PrivacyPolicyLink == defaultConfig.PrivacyPolicyLink)
                defaultCatgory.Add(new JuicyIntegrationReportItem("Default Privay Policy Link", true,
                    "It seems that there's an error with your config file, please contact the Juicy team"));

            if (!firebaseCategory.isEmpty)
                categories.Add(firebaseCategory);
            if (!facebookCategory.isEmpty)
                categories.Add(facebookCategory);
            if (!defaultCatgory.isEmpty)
                categories.Add(defaultCatgory);

        }
    }
}
