using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace JuicyInternal
{
    public class JuicyEditorSettings : ScriptableObject
    {
        [SerializeField]
        bool isFirstImport = true;
        [SerializeField]
        bool isUpgrade = false;
        [SerializeField]
        bool isIntegrationMode = true;

        public bool IsFirstImport { get { return isFirstImport; } set { isFirstImport = value; Save(); } }
        public bool IsIntegrationMode { get { return isIntegrationMode; } set { isIntegrationMode = value; Save(); } }
        public bool IsUpgrade { get { return isUpgrade; } set { isUpgrade = value; Save(); } }

        const string SettingsResourceFolderPath = "Assets/JuicySDKSettings/Resources";
        const string SettingsFilePath = "Assets/JuicySDKSettings/Resources/JuicyEditorSettings.asset";

        static JuicyEditorSettings instance;
        public static JuicyEditorSettings Instance
        {
            get
            {
                if (instance == null)
                    CreateSettings();
                return instance;
            }
        }

        static void CreateSettings()
        {
            instance = Resources.Load<JuicyEditorSettings>("JuicyEditorSettings");

            if (instance == null)
            {
                if (!Directory.Exists(SettingsResourceFolderPath))
                {
                    Directory.CreateDirectory(SettingsResourceFolderPath);
                }

                JuicyEditorSettings asset = ScriptableObject.CreateInstance<JuicyEditorSettings>();
                AssetDatabase.CreateAsset(asset, SettingsFilePath);
                AssetDatabase.Refresh();

                AssetDatabase.SaveAssets();
                JuicySDKLog.LogWarning("Juicy Editor Settings file didn't exist and was created");
                instance = asset;
            }
        }

        void Save()
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
