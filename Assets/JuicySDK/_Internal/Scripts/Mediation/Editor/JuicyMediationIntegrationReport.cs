#if !noJuicyCompilation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
    public class JuicyMediationIntegrationReport : JuicyIntegrationReport
    {
        public JuicyMediationIntegrationReport()
        {
#if !noJuicyCompilation
            //Default config == no check of the config values
            if (JuicyConfigFileManager.IsConfigFileDefault)
                return;

            JuicySDKMediationConfig config = JuicySDKSettings.Instance.MediationConfig;
            JuicySDKMediationConfig defaultConfig = new JuicySDKMediationConfig(JuicySDKSettings.dflt_AppConfigFileName);
            JuicyIntegrationReportCategory mediationCategory = new JuicyIntegrationReportCategory("MEDIATION");

            if (config.AdmobAppID == defaultConfig.AdmobAppID)
                mediationCategory.Add(new JuicyIntegrationReportItem("Default Admob App ID", true, "It seems that there's an error with your config file, please contact the Juicy team"));
            if (config.BannerID== defaultConfig.BannerID)
                mediationCategory.Add(new JuicyIntegrationReportItem("Default Banner ID", true, "It seems that there's an error with your config file, please contact the Juicy team"));
            if (config.InterstitialID == defaultConfig.InterstitialID)
                mediationCategory.Add(new JuicyIntegrationReportItem("Default Interstiatial ID", true, "It seems that there's an error with your config file, please contact the Juicy team"));
            if (config.RewardedID == defaultConfig.RewardedID)
                mediationCategory.Add(new JuicyIntegrationReportItem("Default Rewarded ID", true, "It seems that there's an error with your config file, please contact the Juicy team"));

            if (!mediationCategory.isEmpty)
                categories.Add(mediationCategory);
#endif
        }
    }
}
#endif
