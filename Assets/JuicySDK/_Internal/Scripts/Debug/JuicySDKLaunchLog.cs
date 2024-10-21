using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using AppsFlyerSDK;

namespace JuicyInternal {
    public class JuicySDKLaunchLog
    {
        #pragma warning disable CS0414
        static string resourcesFolderPath = "Assets/JuicySDK/_Internal/Resources/";
        static string logFileName = "JuicySDKLaunchLog";
        static string logFileExtension = ".txt";
        public static string NL = Environment.NewLine;
        #pragma warning restore CS0414

#if UNITY_ANDROID
        static int androidCharacterLogLimit = 500;
#endif

        public static void Log()
        {
            if (Application.isEditor)
                return;

            TextAsset text = Resources.Load(logFileName) as TextAsset;
            if (text == null)
                return;

            string message = text.ToString();

#if UNITY_IOS
            JuicySDKLog.Log(message);
            return;
#elif UNITY_ANDROID
            //Split the string on Android because size of log limits with logcat
            string[] splits = message.Split('\n');

            string currentString = "";

            for (int i=0; i< splits.Length; i++)
            {
                if(currentString.Length + splits[i].Length > androidCharacterLogLimit)
                {
                    JuicySDKLog.Log(currentString);
                    currentString = "";
                }
                currentString += splits[i] + '\n';
            }

            if(currentString.Length>0)
                JuicySDKLog.Log(currentString);
#endif
        }


#if UNITY_EDITOR
        public static void SaveLog()
        {
            if (!Directory.Exists(resourcesFolderPath))
            {
                JuicySDKLog.LogError("JuicySDKLaunchLog : SaveLog : Can't find resources path at : " + resourcesFolderPath);
                return;
            }

            string log = "";
            log += "===================== JUICYSDK LOG START ====================" + NL;
            log += GetBuildInfoLog();
            log += GetJuicyInfoLog();
            log += GetSettingsInfoLog();
            log += GetAppConfigFileLog();
            log += GetDependenciesLog();
            log += "===================== JUICYSDK LOG END ====================";

            File.WriteAllText(resourcesFolderPath + logFileName + logFileExtension, log);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static string GetBuildInfoLog()
        {
            string log = "";
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            log += "--Build--" + NL; 
            log += "Platform: " + buildTarget.ToString() + NL;
            log += "Unity version: " + Application.unityVersion + NL;
            log += "Date: " + DateTime.Now.ToString() + NL;
            //Player Settings
#if UNITY_2018
            log += "Scripting Runtime version: " + PlayerSettings.scriptingRuntimeVersion.ToString() + NL;
#endif
            log += "Scripting Backend: " + PlayerSettings.GetScriptingBackend(buildTargetGroup) + NL;
            log += "Api compatibility level: " + PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup).ToString() + NL;
#if UNITY_IOS
            //iOS
            log += "Target minimum iOS version: " + PlayerSettings.iOS.targetOSVersionString + NL;
#endif
#if UNITY_ANDROID
            //Android
            log += "Minimum API level: " + PlayerSettings.Android.minSdkVersion.ToString() + Environment.NewLine;
#endif
            return log;
        }

        static string GetJuicyInfoLog()
        {
            string log = "";
            log += "--JuicySDK--" + NL;
            log += "JuicySDK: " + Juicy.JuicySDK.version + NL;
            log += "Facebook: " + Facebook.Unity.FacebookSdkVersion.Build.ToString() + NL;
            log += "Firebase: " + GetFirebaseUnityVersion() + NL;
            log += "AppsFlyer: " + AppsFlyer.getSdkVersion() + NL;
#if Adjust
            log += "Adjust: " + com.adjust.sdk.Adjust.SDKVERSION + NL;
#endif
#pragma warning disable CS0618 //Using Instance() adds an empty object IAPUtil in the scene each build
            log += "UnityIAP: " + UnityEngine.Purchasing.StandardPurchasingModule.k_PackageVersion + NL;
#pragma warning restore CS0618

#if !noJuicyCompilation
            log += JuicySDKMediationLaunchLog.GetMediationLog() + NL;
#endif
            return log;
        }

        static string GetSettingsInfoLog()
        {
            string log = "";
            log += "--Settings--" + NL;
            log += "Ads" + NL;
            log += "Default delay between interstitial: " + JuicySDKSettings.dflt_DelayBetweenInterstitial + NL;
            log += "No interstitial after rewarded: " + JuicySDKSettings.Instance.NoInterstitialAfterRewarded + NL;
            log += "AB Test" + NL;
            log += "Enabled: " + JuicySDKSettings.Instance.EnableAbTest + NL;
            log += "Keep Variant: " + JuicySDKSettings.Instance.AbTestKeepVariantIndex + NL;
            log += "Type: " + JuicySDKSettings.Instance.AbTestType.ToString() + NL;
            log += "Name: " + JuicySDKSettings.Instance.AbTestName + NL;
            log += "Variant amount: " + JuicySDKSettings.Instance.AbTestVariantAmount + NL;
            log += "Variant population: " + JuicySDKSettings.Instance.AbTestVariantPopulation + NL;
            return log;
        }

        static string GetAppConfigFileLog()
        {
            string log = "";
            log += "--Config--" + NL;
            log += JuicySDKSettings.Instance.GetConfigFileLog();
            return log;
        }

        static string GetDependenciesLog()
        {
            string log = "";
            log += "--Dependencies--" + NL;

            //Dependencies
            foreach (string assetFolder in Directory.GetDirectories("Assets"))
            {
                string[] filePaths = Directory.GetFiles(assetFolder + "/", "*.xml", SearchOption.AllDirectories);
                string[] dependenciesPaths = filePaths.Where(p => p.Contains("dependencies") || p.Contains("Dependencies")).ToArray();

                if (dependenciesPaths.Length < 1)
                    continue;

                char folderSeparatorChar = System.IO.Path.DirectorySeparatorChar;
                log += assetFolder.Split(folderSeparatorChar)[1] + NL;

                foreach (string path in dependenciesPaths)
                {
                    XmlDocument document;
                    if (!XmlUtility.TryGetXmlDocument(path, out document))
                        continue;
#if UNITY_IOS
                    //iOS
                    XmlNode podsNode = XmlUtility.GetNode(document, "iosPods");
                    if (podsNode == null)
                        continue;
                    foreach (XmlNode node in podsNode.ChildNodes)
                    {
                        if (node.Name != "iosPod")
                            continue;
                        string str = "";
                        foreach (XmlAttribute attribute in node.Attributes)
                            str += attribute.Name + ": " + attribute.Value + " ";
                        log += str + NL;
                    }
#endif
#if UNITY_ANDROID
                //Android
                XmlNode packagesNode = XmlUtility.GetNode(document, "androidPackages");
                if (packagesNode == null)
                    continue;
                foreach (XmlNode node in packagesNode.ChildNodes)
                {
                    if (node.Name != "androidPackage")
                        continue;
                    string str = "";
                    foreach (XmlAttribute attribute in node.Attributes)
                        str += attribute.Name + ": " + attribute.Value + " ";
                    log += str + NL; 
                }
#endif
                }
            }
            return log;
        }

        static string GetFirebaseUnityVersion()
        {
            string[] pathsToManifest = Directory.GetFiles("Assets/", "*FirebaseAnalytics_version*", SearchOption.AllDirectories);
            if (pathsToManifest.Length == 0)
                return "?";
            string pathToAnalyticsManifest = pathsToManifest[0];
            string[] firstSplit = pathToAnalyticsManifest.Split('-');
            if (firstSplit.Length < 2)
                return "?";
            string[] secondSplit = firstSplit[1].Split('_');
            if (secondSplit.Length < 2)
                return "?";
            return secondSplit[0];
        }
#endif
        }
    }