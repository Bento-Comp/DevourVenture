using UnityEngine;
using System.Xml;
using Juicy;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using System.IO;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace JuicyInternal
{
    public class JuicyBaseProcessBuildManager : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
#if UNITY_ANDROID
        private string androidNamespace = "http://schemas.android.com/apk/res/android";

        private string manifestPath { get { return Application.dataPath + "/Plugins/Android/AndroidManifest.xml"; } }
        private string gradlePath { get { return Application.dataPath + "/Plugins/Android/mainTemplate.gradle"; } }
        private string launcherGradlePath { get { return Application.dataPath + "/Plugins/Android/launcherTemplate.gradle"; } }
        private string baseTemplateGradlePath { get { return Application.dataPath + "/Plugins/Android/baseProjectTemplate.gradle"; } }
        private string propertiesGradlePath { get { return Application.dataPath + "/Plugins/Android/gradleTemplate.properties"; } }

        private string backUpExtension = ".backup";
#endif
        // Make sure the plist or manifest edition is done after everything else
        // 32000 is reserved for the mediation process build manager
        public int callbackOrder { get { return 31999; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            //Load App config settings from config file
            JuicySDKSettings.Instance.LoadAppConfig();
            //Setup LaunchLog
            JuicySDKLaunchLog.SaveLog();

#if UNITY_ANDROID
            ManifestMod.GenerateManifest();
            EditAppGradle();
            EditAppManifest();
#endif
        }

        public void OnPostprocessBuild(BuildReport report)
        {
#if UNITY_IOS
            EditAppPlist(report.summary.outputPath + "/Info.plist");
            EditPBXPrject(PBXProject.GetPBXProjectPath(report.summary.outputPath));
#endif
            if (!JuicyEditorSettings.Instance.IsIntegrationMode)
                JuicyIntegrationWindow.ShowWindow(true);
        }

#if UNITY_ANDROID

        void EditAppGradle()
        {
            //Delete all gradle backups
            //main gradle
            if (File.Exists(gradlePath + backUpExtension))
                File.Delete(gradlePath + backUpExtension);
            if (File.Exists(gradlePath + backUpExtension + ".meta"))
                File.Delete(gradlePath + backUpExtension + ".meta");
            //launcher
            if (File.Exists(launcherGradlePath + backUpExtension))
                File.Delete(launcherGradlePath + backUpExtension);
            if (File.Exists(launcherGradlePath + backUpExtension + ".meta"))
                File.Delete(launcherGradlePath + backUpExtension + ".meta");

            //base template
            if (File.Exists(baseTemplateGradlePath + backUpExtension))
                File.Delete(baseTemplateGradlePath + backUpExtension);
            if (File.Exists(baseTemplateGradlePath + backUpExtension + ".meta"))
                File.Delete(baseTemplateGradlePath + backUpExtension + ".meta");

            //properties
            if (File.Exists(propertiesGradlePath + backUpExtension))
                File.Delete(propertiesGradlePath + backUpExtension);
            if (File.Exists(propertiesGradlePath + backUpExtension + ".meta"))
                File.Delete(propertiesGradlePath + backUpExtension + ".meta");



#if UNITY_2020_1_OR_NEWER
            //Changes in aapt options since Unity 2020
            string original = "noCompress = ['.unity3d', '.ress', '.resource', '.obb'**STREAMING_ASSETS**]";
            string replace = "noCompress = ['.ress', '.resource', '.obb'] + unityStreamingAssets.tokenize(', ')\r\n        ignoreAssetsPattern = \"!.svn:!.git:!.ds_store:!*.scc:.*:!CVS:!thumbs.db:!picasa.ini:!*~\"";

            string mainGradleFile = File.ReadAllText(gradlePath);
            string launcherGradleFile = File.ReadAllText(launcherGradlePath);
            mainGradleFile = mainGradleFile.Replace(original, replace);
            launcherGradleFile = launcherGradleFile.Replace(original, replace);
            File.WriteAllText(gradlePath, mainGradleFile);
            File.WriteAllText(launcherGradlePath, launcherGradleFile);

            //Change gradle plugin to match unity gradle version cf https://developer.android.com/studio/releases/gradle-plugin?buildsystem=ndk-build#updating-gradle
            original = "classpath 'com.android.tools.build:gradle:3.4.3'";
            replace = "classpath 'com.android.tools.build:gradle:4.0.1'";
            string baseGradleFile = File.ReadAllText(baseTemplateGradlePath);
            baseGradleFile = baseGradleFile.Replace(original, replace);
            File.WriteAllText(baseTemplateGradlePath, baseGradleFile);
#endif

        }

        void EditAppManifest()
        {
            XmlDocument manifest = new XmlDocument();
            if (!XmlUtility.TryGetXmlDocument(manifestPath, out manifest))
            {
                Debug.LogError("ProcessBuildManager : AddAdMobIDToManifest : Can't find manifest at : " + manifestPath);
                return;
            }

            XmlNode appNode = XmlUtility.GetNode(manifest, "application");

            //Allow HTTP traffic (useful for geolocalisation)
            XmlUtility.SetNodeAttribute(manifest, appNode, "android", "usesCleartextTraffic", androidNamespace, "true");

            //FireBase
            //Anlytics
            XmlNode fireBaseNode = XmlUtility.GetNode(appNode, "meta-data", "android:name", "firebase_analytics_collection_enabled");
            if (fireBaseNode == null)
            {
                fireBaseNode = XmlUtility.AddNode(manifest, appNode, "meta-data");
                XmlUtility.SetNodeAttribute(manifest, fireBaseNode, "android", "name", androidNamespace, "firebase_analytics_collection_enabled");
            }
            XmlUtility.SetNodeAttribute(manifest, fireBaseNode, "android", "value", androidNamespace, "false");
            //Ads
            XmlNode fireBaseAdNode = XmlUtility.GetNode(appNode, "meta-data", "android:name", "google_analytics_default_allow_ad_personalization_signals");
            if (fireBaseAdNode == null)
            {
                fireBaseAdNode = XmlUtility.AddNode(manifest, appNode, "meta-data");
                XmlUtility.SetNodeAttribute(manifest, fireBaseAdNode, "android", "name", androidNamespace, "google_analytics_default_allow_ad_personalization_signals");
            }
            XmlUtility.SetNodeAttribute(manifest, fireBaseAdNode, "android", "value", androidNamespace, "true");

            //Facebook
            //Events
            XmlNode facebookEventNode = XmlUtility.GetNode(appNode, "meta-data", "android:name", "com.facebook.sdk.AutoLogAppEventsEnabled");
            if (facebookEventNode == null)
            {
                facebookEventNode = XmlUtility.AddNode(manifest, appNode, "meta-data");
                XmlUtility.SetNodeAttribute(manifest, facebookEventNode, "android", "name", androidNamespace, "com.facebook.sdk.AutoLogAppEventsEnabled");
            }
            XmlUtility.SetNodeAttribute(manifest, facebookEventNode, "android", "value", androidNamespace, "false");
            //ID
            XmlNode facebookIDNode = XmlUtility.GetNode(appNode, "meta-data", "android:name", "com.facebook.sdk.AdvertiserIDCollectionEnabled");
            if (facebookIDNode == null)
            {
                facebookIDNode = XmlUtility.AddNode(manifest, appNode, "meta-data");
                XmlUtility.SetNodeAttribute(manifest, facebookIDNode, "android", "name", androidNamespace, "com.facebook.sdk.AdvertiserIDCollectionEnabled");
            }
            XmlUtility.SetNodeAttribute(manifest, facebookIDNode, "android", "value", androidNamespace, "false");


            manifest.Save(manifestPath);
        }
#endif
#if UNITY_IOS
        void EditAppPlist(string plistPath)
        {
            PlistDocument plist = new PlistDocument();
            if (!PlistUtility.TryGetPlist(plistPath, out plist))
            {
                Debug.LogError("ProcessBuildManager : EditAppPlist : Can't find plist at : " + plistPath);
                return;
            }

            PlistElementDict rootDict = plist.root;

            PlistElementDict ATSDict = PlistUtility.GetDict(rootDict, "NSAppTransportSecurity");

            //Used for geolocalisation stuff
            /* PlistElementDict ATSExceptionDict = PlistUtility.GetDict(ATSDict, "NSExceptionDomains");
            PlistElementDict ATSDomainDict = PlistUtility.GetDict(ATSExceptionDict, "ip-api.com");
            PlistUtility.SetKey(ATSDomainDict, "NSExceptionAllowsInsecureHTTPLoads", true);
            PlistUtility.SetKey(ATSDomainDict, "NSIncludesSubdomains", true);*/

            //iOS 15+ Advertising postback send to Tenjin
            PlistUtility.SetKey(rootDict, "NSAdvertisingAttributionReportEndpoint", "https://tenjin-skan.com");

            //Key added by Firebase, but some mediation acquires work properly only if there's only NSAllowsArbitraryLoad key in ATS Dict
            PlistUtility.DeleteKey(ATSDict, "NSAllowsArbitraryLoadsInWebContent");
            //Depracted key preventing upload on appstore
            PlistUtility.DeleteKey(rootDict, "UIApplicationExitsOnSuspend");

            //Firebase privacy
            PlistUtility.SetKey(rootDict, "FIREBASE_ANALYTICS_COLLECTION_ENABLED", true);
            PlistUtility.SetKey(rootDict, "GOOGLE_ANALYTICS_DEFAULT_ALLOW_AD_PERSONALIZATION_SIGNALS", false);

            //Facebook privacy
            PlistUtility.SetKey(rootDict, "FacebookAutoLogAppEventsEnabled", false);
            PlistUtility.SetKey(rootDict, "FacebookAdvertiserIDCollectionEnabled", false);

            PlistUtility.SetKey(rootDict, "NSUserTrackingUsageDescription", "Your data will be used for personalized advertising and analytics purposes");

            PlistUtility.SetKey(rootDict, "NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");

            plist.WriteToFile(plistPath);
        }

        void EditPBXPrject(string projectPath)
        {
            PBXProject project = new PBXProject();
            if (!PBXProjectUtility.TryGetPBXProject(projectPath, out project))
            {
                Debug.LogError("ProcessBuildManager : EditPBXProject : Can't find project at : " + projectPath);
                return;
            }

            //Do this because some version of Unity don't do it automatically (TODO check which one)
            project.AddFrameworkToProject(project.GetUnityMainTargetGuid(), "UnityFramework.framework", false);
            //Make sure Swift library are only embeded for main target and not framework target, otherwise the upload to the appstore may fail
            //with error 'contains disallowed file Frameworks'
            project.SetBuildProperty(project.GetUnityFrameworkTargetGuid(), "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            project.SetBuildProperty(project.GetUnityMainTargetGuid(), "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            //Force "ENABLE_BITCODE" to "NO"
            project.SetBuildProperty(project.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(project.GetUnityFrameworkTargetGuid(), "ENABLE_BITCODE", "NO");


            PBXProjectUtility.AddFramework(project, "AppTrackingTransparency", true);
            project.WriteToFile(projectPath);


        }
#endif
    }
}