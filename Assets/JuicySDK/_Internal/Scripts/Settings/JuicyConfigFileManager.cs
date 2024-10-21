#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Xml;
using Facebook.Unity.Settings;
using System.Linq;

namespace JuicyInternal {
    public class JuicyConfigFileManager
    {
        const string SETTINGS_FOLDER_PATH = "Assets/JuicySDKSettings";
        const string SETTINGS_RESOURCES_PATH = "Assets/JuicySDKSettings/Resources";
        const string FIREBASE_PLIST_PATH = "Assets/JuicySDKSettings/GoogleService-Info.plist";
        const string FIREBASE_JSON_PATH = "Assets/JuicySDKSettings/google-services.json";

        public static bool IsConfigFileDefault { get { return JuicySDKSettings.Instance.AppConfigFileName == JuicySDKSettings.dflt_AppConfigFileName; } }

        static string currentConfigPath { get { return Path.Combine(SETTINGS_RESOURCES_PATH, JuicySDKSettings.Instance.AppConfigFileName + ".xml"); } }

        public static void ImportConfigFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Can't find file: " + path);
                return;
            }

            if (path != currentConfigPath)
            {
                DeleteCurrentConfig();
                File.Copy(path, Path.Combine(SETTINGS_RESOURCES_PATH, Path.GetFileName(path)), true);
            }

            AssetDatabase.Refresh();
            SerializedObject settings = new SerializedObject(JuicySDKSettings.Instance);

            settings.Update();
            settings.FindProperty("appConfigFileName").stringValue = Path.GetFileNameWithoutExtension(path);
            settings.ApplyModifiedProperties();
            JuicySDKSettings.Instance.LoadAppConfig();

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, JuicySDKSettings.Instance.BaseConfig.AppBundleID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, JuicySDKSettings.Instance.BaseConfig.AppBundleID);

            SetUpFacebookConfig();
            GenerateFirebaseFile();
            AssetDatabase.Refresh();
        }

        public static void ReloadCurrentConfig()
        {
            if (IsConfigFileDefault)
                LoadDefaultConfig();
            else
                ImportConfigFile(currentConfigPath);
        }

        public static void LoadDefaultConfig()
        {
            DeleteCurrentConfig();

            SerializedObject settings = new SerializedObject(JuicySDKSettings.Instance);
            settings.Update();
            settings.FindProperty("appConfigFileName").stringValue = JuicySDKSettings.dflt_AppConfigFileName;
            settings.ApplyModifiedProperties();
            JuicySDKSettings.Instance.LoadAppConfig();

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, JuicySDKSettings.Instance.BaseConfig.AppBundleID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, JuicySDKSettings.Instance.BaseConfig.AppBundleID);

            SetUpFacebookConfig();
            GenerateFirebaseFile();
            AssetDatabase.Refresh();
        }

        public static bool IsFirebaseFileDefault()
        {
            XmlDocument config = new XmlDocument();
            if (!XmlUtility.LoadXmlFromResources(JuicySDKSettings.dflt_AppConfigFileName, out config))
            {
                Debug.LogError("JuicyConfigFileManager : IsFirebaseFileDefault : Can't find default xml file ");
                return true;
            }

            XmlNode settingsNode = XmlUtility.GetNode(config, "Settings");
            XmlNode analyticsNode = XmlUtility.GetNode(settingsNode, "Analytics");
            XmlNode firebaseNode = XmlUtility.GetNode(analyticsNode, "Firebase");

#if UNITY_IOS
            string dfltRawFirebase = XmlUtility.GetNode(firebaseNode, "iOS").InnerText;
            string currentRawFirebase = File.ReadAllText(FIREBASE_PLIST_PATH);
#else
            string dfltRawFirebase = XmlUtility.GetNode(firebaseNode, "Android").InnerText;
            string currentRawFirebase = File.ReadAllText(FIREBASE_JSON_PATH);
#endif
            return dfltRawFirebase == currentRawFirebase;
        }

        static void DeleteCurrentConfig()
        {
            string previousConfigFilePath = Path.Combine(SETTINGS_RESOURCES_PATH, JuicySDKSettings.Instance.AppConfigFileName + ".xml");
            if (File.Exists(previousConfigFilePath))
                File.Delete(previousConfigFilePath);
            string metaFile = previousConfigFilePath + ".meta";
            if (File.Exists(metaFile))
                File.Delete(metaFile);
        }

        static void GenerateFirebaseFile()
        {
            if (File.Exists(FIREBASE_PLIST_PATH))
                File.Delete(FIREBASE_PLIST_PATH);
            if (File.Exists(FIREBASE_JSON_PATH))
                File.Delete(FIREBASE_JSON_PATH);

            XmlDocument config = new XmlDocument();
            if (!XmlUtility.LoadXmlFromResources(JuicySDKSettings.Instance.AppConfigFileName, out config))
            {
                Debug.LogError("JuicyConfigFileManager : GenerateFirebaseFile : Can't find xml file " + JuicySDKSettings.Instance.AppConfigFileName + " in Resources");
                return;
            }

            XmlNode settingsNode = XmlUtility.GetNode(config, "Settings");
            XmlNode analyticsNode = XmlUtility.GetNode(settingsNode, "Analytics");
            XmlNode firebaseNode = XmlUtility.GetNode(analyticsNode, "Firebase");
            string rawPlist = XmlUtility.GetNode(firebaseNode, "iOS").InnerText;
            string rawJson = XmlUtility.GetNode(firebaseNode, "Android").InnerText;

            if (rawPlist != "")
                File.WriteAllText(FIREBASE_PLIST_PATH, rawPlist);
            if (rawJson != "")
                File.WriteAllText(FIREBASE_JSON_PATH, rawJson);
        }

        public static void SetUpFacebookConfig()
        {
            JuicySDKSettings.Instance.LoadAppConfig();

            //Access Facebook settings through serialized property 
            SerializedObject fb = new SerializedObject(FacebookSettings.Instance);
            fb.Update();
            SerializedProperty appLabels = fb.FindProperty("appLabels");
            SerializedProperty appIds = fb.FindProperty("appIds");
            SerializedProperty clientTokens = fb.FindProperty("clientTokens");
            SerializedProperty idCollectionEnabled = fb.FindProperty("advertiserIDCollectionEnabled");
            SerializedProperty autoLogEvents = fb.FindProperty("autoLogAppEventsEnabled");

            appLabels.arraySize = 1;
            appLabels.GetArrayElementAtIndex(0).stringValue = JuicySDKSettings.Instance.BaseConfig.FacebookAppName;
            appIds.arraySize = 1;
            appIds.GetArrayElementAtIndex(0).stringValue = JuicySDKSettings.Instance.BaseConfig.FacebookAppID;
            clientTokens.arraySize = 1;
            clientTokens.GetArrayElementAtIndex(0).stringValue = JuicySDKSettings.Instance.BaseConfig.FacebookClientToken;
            idCollectionEnabled.boolValue = false;
            autoLogEvents.boolValue = false;
            fb.ApplyModifiedProperties();
        }
    }
}
#endif