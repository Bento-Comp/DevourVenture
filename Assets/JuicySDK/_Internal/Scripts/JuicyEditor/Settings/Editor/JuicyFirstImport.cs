using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Juicy;

namespace JuicyInternal
{
    [InitializeOnLoad]
    public class JuicyFirstImport : MonoBehaviour
    {
        static JuicyFirstImport()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        static void OnEditorUpdate()
        {
            if (JuicyEditorSettings.Instance.IsFirstImport)
                OnFirstImport();
            else if (JuicyEditorSettings.Instance.IsUpgrade)
                OnUpgrade();

            EditorApplication.update -= OnEditorUpdate;
        }

        static void OnFirstImport()
        {
            JuicySDKSettingsWindow.ShowWindow();
            Debug.Log("Juicy SDK_" + JuicySDK.version + " Import. Edit your Settings in JuicySDK > JuicySDKSettings");
            JuicyEditorSettings.Instance.IsFirstImport = false;
        }

        static void OnUpgrade()
        {
            JuicyEditorSettings.Instance.IsUpgrade = false;
            Debug.Log("Juicy SDK has been upgraded to version : " + JuicySDK.version);
            JuicyConfigFileManager.SetUpFacebookConfig();
        }
    }
}

