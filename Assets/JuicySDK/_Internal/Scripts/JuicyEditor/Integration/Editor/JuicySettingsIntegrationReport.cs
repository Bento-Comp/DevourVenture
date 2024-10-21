using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
    public class JuicySettingsIntegrationReport : JuicyIntegrationReport
    {
        public JuicySettingsIntegrationReport()
        {
            JuicyIntegrationReportCategory settingsCat = new JuicyIntegrationReportCategory("SETTINGS");

            //Settings
            if (JuicyConfigFileManager.IsConfigFileDefault)
                settingsCat.Add(new JuicyIntegrationReportItem("Using default config file", false,
                    "You're using the Juicy default configuration file. If you are going to release your app load the config file given to you by the Juicy team"));
            if(JuicySDKSettings.Instance.OverrideDefaultMediationParameters)
                settingsCat.Add(new JuicyIntegrationReportItem("Mediation default parameters overrided", false,
                    "Mediation default parameters are being overrided. Do this only if a member of the Juicy team told you to"));
            if(JuicySDKSettings.Instance.OverrideDefaultPrivacyParameters)
                settingsCat.Add(new JuicyIntegrationReportItem("Privacy default parameters overrided", false,
                    "Privacy default parameters are being overrided. Do this only if a member of the Juicy team told you to"));
            if (JuicySDKSettings.Instance.OverrideDefaultAbTestParameters && JuicySDKSettings.Instance.EnableAbTest)
                settingsCat.Add(new JuicyIntegrationReportItem("ABTest default parameters overrided", false,
                    "ABTest default parameters are being overrided. Do this only if a member of the Juicy team told you to"));
            if (JuicySDKSettings.IsDebugMode)
                settingsCat.Add(new JuicyIntegrationReportItem("Debug mode enabled", false,
                    "Debug mode should not be enabled in your release, remove juicySDKDebug from your scripting define symbol"));

            if (!settingsCat.isEmpty)
                categories.Add(settingsCat);
        }
    }
}
