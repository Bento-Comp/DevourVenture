using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace JuicyInternal {

    public class JuicyProjectIntegrationReport  : JuicyIntegrationReport
    {
        const string GRADLE_PATH = "Assets/Plugins/Android/mainTemplate.gradle";
        const string EXCLUDE_ARMV7 = "exclude ('/lib/armeabi-v7a/*' + '*')";
        const string EXCLUDE_ARM64 = "exclude ('/lib/arm64-v8a/*' + '*')";

        public JuicyProjectIntegrationReport()
        {
            JuicyIntegrationReportCategory unityCategory = new JuicyIntegrationReportCategory("UNITY");
            JuicyIntegrationReportCategory projectCategory = new JuicyIntegrationReportCategory("PROJECT");

            //Unity
            bool isUnityVersionSupported = false;
            foreach (string version in JuicySDKSettings.SupportedUnityVersion)
            {
                if (Application.unityVersion.Contains(version))
                {
                    isUnityVersionSupported = true;
                    break;
                }
            }

            if (!isUnityVersionSupported)
                unityCategory.Add(new JuicyIntegrationReportItem("Untested Unity version", true,
                    "The version of Unity you're using hasn't have its compatibility with the Juicy SDK tested."));

            //Project
#if UNITY_ANDROID
            string file = File.ReadAllText(GRADLE_PATH);

            bool targetContainsARM64 = (PlayerSettings.Android.targetArchitectures & AndroidArchitecture.ARM64) != 0;
            bool targetContainsARMv7 = (PlayerSettings.Android.targetArchitectures & AndroidArchitecture.ARMv7) != 0;

            if (targetContainsARM64)
            {
                if (file.Contains(EXCLUDE_ARM64))
                    projectCategory.Add(new JuicyIntegrationReportItem("Target Architecture & Gradle Mismatch (ARM64)", true,
                        "You have added Arm64 to your target architectures but your gradle file excludes it. If you haven't built yet with those settings building should fix the gradle file automatically." +
                        "If it doesn't go to Assets/External Dependencies Manager/Android Resolver/Force Resolve."));
            }

            if (targetContainsARMv7)
            {
                if (file.Contains(EXCLUDE_ARMV7))
                    projectCategory.Add(new JuicyIntegrationReportItem("Target Architecture & Gradle Mismatch (ARMv7)", true,
                        "You have added Arm64 to your target architectures but your gradle file excludes it. If you haven't built yet with those settings building should fix the gradle file automatically." +
                        "If it doesn't go to Assets/External Dependencies Manager/Android Resolver/Force Resolve."));
            }
#endif

            if (!unityCategory.isEmpty)
                categories.Add(unityCategory);
            if (!projectCategory.isEmpty)
                categories.Add(projectCategory);
        }
    }
}


