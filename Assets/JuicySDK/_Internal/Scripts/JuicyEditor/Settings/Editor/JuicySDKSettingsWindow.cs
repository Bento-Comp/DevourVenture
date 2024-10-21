using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Juicy;
using System.IO;

namespace JuicyInternal
{
    public enum SettingsCategories
    {
        None,
        Configuration,
        Mediation,
        Privacy,
        InAppPurchase,
        Editor,
        ABTest,
#if debugJuicySDK
        Debug,
#endif
    }

    public class JuicySDKSettingsWindow : EditorWindow
    {
        #pragma warning disable 0649
        static SerializedObject settings;
        #pragma warning restore 0649
        Vector2 settingsScrollPos;
        SettingsCategories settingsCategory = SettingsCategories.None;

        [MenuItem("JuicySDK/Juicy SDK Settings %#j")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(JuicySDKSettingsWindow), true, "Juicy SDK Settings");
        }

        #region Unity
        void OnGUI()
        {
            minSize = JuicyEditorSettings.Instance.IsIntegrationMode ? new Vector2(600, 440) : new Vector2(600, 645);
            maxSize = minSize;

            DrawHeader();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (JuicyEditorSettings.Instance.IsIntegrationMode)
                DrawIntegrationWindow();
            else
                DrawProductionWindow();
        }

        private void OnFocus()
        {
            settings = new SerializedObject(JuicySDK.Settings);
        }

        #endregion //Unity
        #region Draw
        #region Window
        void DrawHeader()
        {
            GUIStyle backGroundStyle = new GUIStyle();
            Texture2D texture =  GetTexture(new Color(.4f, .4f, .4f));
            backGroundStyle.normal.background = texture;

            GUIStyle fontStyle = new GUIStyle();
            fontStyle.fontSize = 17;
            fontStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.BeginVertical(backGroundStyle, GUILayout.Height(50));
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("JuicySDK_" + JuicySDK.version, fontStyle);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(JuicyEditorSettings.Instance.IsIntegrationMode ? "Switch to Production Mode" : "Switch to Integration mode", GUILayout.Width(200)))
            {
                SwitchMode();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        void DrawIntegrationWindow()
        {
            EditorGUILayout.BeginVertical();

            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            style.padding = new RectOffset(10, 10, 0, 0);
            style.normal.textColor = EditorGUIUtility.isProSkin ? Color.grey : Color.black;

            //Integration mode explanantion
            EditorGUILayout.LabelField("You are currently using the juicy SDK in integration mode",EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("This mode allows you to test the integration of the SDK. When you're integration of the SDK is done properly, click on the switch to production mode button." +
                "Once in production mode replace the default IDs by the ones given to you by the Juicy team.",style);

            GUILayout.Space(30);

            //Documetation
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Read the Juicy SDK documentation to learn how to set it up", EditorStyles.boldLabel);
            if (GUILayout.Button("Documentation", GUILayout.Width(200)))
                Application.OpenURL(JuicySDKSettings.documentation);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(12);

            EditorGUILayout.LabelField("If you encountered issues not covered by the documentation, feel free to contact a member of the Juicy development team.", style);

            GUILayout.Space(30);

            //PlayerPrefs
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Delete all the Player Prefs", EditorStyles.boldLabel);
            if (GUILayout.Button("Delete Player Prefs", GUILayout.Width(200)))
                DeletePlayerPrefs();
                
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(12);

            EditorGUILayout.LabelField("Use this to test one time features like the Remove Ads, Privacy Pop-Up... again", style);

            GUILayout.Space(30);

            //Remove
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Remove the Juicy SDK from your project",EditorStyles.boldLabel);
            if (GUILayout.Button("Remove SDK", GUILayout.Width(200)))
                RemoveSDK(false);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(12);

            EditorGUILayout.LabelField("This will remove all the files imported by the JuicySDK Package.", style);

            EditorGUILayout.EndVertical();
        }

        void DrawProductionWindow()
        {
            settings.Update();
            DrawProductionSettings();
            settings.ApplyModifiedProperties();
            DrawProductionButtons();
        }

        void DrawProductionSettings()
        {
            EditorGUIUtility.labelWidth = 150;
            settingsCategory = (SettingsCategories)EditorGUILayout.EnumPopup("Settings Filter: ", settingsCategory);
            settingsScrollPos = EditorGUILayout.BeginScrollView(settingsScrollPos, EditorStyles.helpBox, GUILayout.Height(400));

            EditorGUILayout.Space();
            EditorGUIUtility.labelWidth = 250;

            if (settingsCategory != SettingsCategories.None)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(settingsCategory.ToString().ToUpper(), EditorStyles.boldLabel);
                EditorGUILayout.Space();
                DrawSettingsCategory(settingsCategory);
            }

            else
            {
                foreach (SettingsCategories category in System.Enum.GetValues(typeof(SettingsCategories)))
                {
                    if (category == SettingsCategories.None)
                        continue;

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(category.ToString().ToUpper(), EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    DrawSettingsCategory(category);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
            }

            EditorGUILayout.EndScrollView();

        }

        void DrawProductionButtons()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(15);

            //Generate integration report
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Check if you set up all the necessaries IDs in the JuicySDK");
            if (GUILayout.Button("Generate Report", GUILayout.Width(200)))
                GenerateReport();
            EditorGUILayout.EndHorizontal();

            //Docs
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Any question? Reading the documentation might be useful");
            if (GUILayout.Button("Documentation", GUILayout.Width(200)))
                Application.OpenURL(JuicySDKSettings.documentation);
            EditorGUILayout.EndHorizontal();

            //Player Prefs
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Delete Juicy's Player Prefs to test again one time features");
            if (GUILayout.Button("Delete Player Prefs", GUILayout.Width(200)))
                DeletePlayerPrefs();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(25);

            //Upgrade JuicySDK
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Upgrade Juicy SDK to a newer version");
            if (GUILayout.Button("Upgrade SDK", GUILayout.Width(200)))
                UpgradeSDK();
            EditorGUILayout.EndHorizontal();

            //Remove JuicySDK
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Completly remove the Juicy SDK from your project");
            if (GUILayout.Button("Remove SDK", GUILayout.Width(200)))
                RemoveSDK(true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        #endregion //Window
        #region Settings
        void DrawSettingsCategory(SettingsCategories sc)
        {
            switch (sc)
            {
                case SettingsCategories.Configuration:
                    DrawConfigurationSettings();
                    break;
                case SettingsCategories.Mediation:
                    DrawMediationSettings();
                    break;
                case SettingsCategories.Privacy:
                    DrawPrivacySettings();
                    break;
                case SettingsCategories.InAppPurchase:
                    DrawInAppSettings();
                    break;
                case SettingsCategories.Editor:
                    DrawEditorSettings();
                    break;
                case SettingsCategories.ABTest:
                    DrawABTestSettings();
                    break;
#if debugJuicySDK
                case SettingsCategories.Debug:
                    DrawDebugSettings();
                    break;
#endif
                default:
                    break;
            }
        }

        void DrawConfigurationSettings()
        {
            bool isDefault = JuicyConfigFileManager.IsConfigFileDefault;
            GUIStyle richText = new GUIStyle(GUI.skin.label);
            richText.richText = true;

            string configFileName = "";
            configFileName += isDefault ? "<color=orange>": "<color=green>";
            configFileName += "     ";
            configFileName += JuicySDKSettings.Instance.AppConfigFileName + ".xml";
            configFileName += isDefault ? " (default)" : "";
            configFileName += "</color>";

            EditorGUILayout.LabelField("Current juicy config file: " + configFileName,richText);
            EditorGUILayout.Space();
            //Juicy app config file
            DrawSettingsButton("Import the config file for your app", "Import config file", ImportConfigFile);
            DrawSettingsButton("Reimport the current config file", "Reimport config", ReimportConfigFile);
            DrawSettingsButton("Log the config file in the console", "Log config file", LogConfigFile);
            DrawSettingsButton("Reset config file to default", "Reset config", ResetConfigFile);
        }

        void DrawMediationSettings()
        {
            //Default Override 
            SerializedProperty overrideProp = settings.FindProperty("overrideDefaultMediationParameters");
            SerializedProperty noInterAfterRewardedProp = settings.FindProperty("noInterstitialAfterRewarded");
            SerializedProperty delayBetweenInterstitialProp = settings.FindProperty("delayBetweenInterstitial");
            SerializedProperty bannerPosition = settings.FindProperty("bannerPosition");

            bannerPosition.intValue = (int)(BannerPosition)EditorGUILayout.EnumPopup("Banner Position", (BannerPosition)bannerPosition.intValue);

            DrawOverrideProperty(overrideProp, "Override Default Mediation Parameters");
            EditorGUILayout.Space();

            //Check value of the property vs value in settings to detect change in the value
            if (overrideProp.boolValue != JuicySDKSettings.Instance.OverrideDefaultMediationParameters)
            {
                //Warning when set to true
                if (overrideProp.boolValue)
                {
                    if (!EditorUtility.DisplayDialog("Warning", "You're about to override mediation paramters. Do this only if a member of the Juicy team told you to.", "OK", "Cancel"))
                    {
                        overrideProp.boolValue = false;
                        return;
                    }
                }
                //If value is change to false then reset the settings to default
                else
                {
                    noInterAfterRewardedProp.boolValue = JuicySDKSettings.dflt_NoInterstitialAfterRewarded;
                    delayBetweenInterstitialProp.intValue = JuicySDKSettings.dflt_DelayBetweenInterstitial;
                }
            }

            //Value is true: display mediation overridable settings
            if (overrideProp.boolValue)
            {
                GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                EditorGUILayout.PropertyField(noInterAfterRewardedProp);
                EditorGUILayout.PropertyField(delayBetweenInterstitialProp);
                GUILayout.EndVertical();
            }
        }

        void DrawPrivacySettings()
        {
            //Default overrides
            EditorGUILayout.Space();
            SerializedProperty overrideProp = settings.FindProperty("overrideDefaultPrivacyParameters");
            SerializedProperty disableGDPRProp = settings.FindProperty("disableGDPRManagement");
            SerializedProperty iosTrackingAuthorizationProp = settings.FindProperty("displayIosTrackingAuthorization");
            SerializedProperty preiOSPopUpProp = settings.FindProperty("displayJuicyIosTrackingPopUp");

            DrawOverrideProperty(overrideProp, "Override Default Privacy Parameters");
            EditorGUILayout.Space();

            //Check value of the property vs value in settings to detect change in the value
            if (overrideProp.boolValue != JuicySDKSettings.Instance.OverrideDefaultPrivacyParameters)
            {
                //Warning when set to true
                if (overrideProp.boolValue)
                {
                    if (!EditorUtility.DisplayDialog("Warning", "You're about to override privacy paramters. Do this only if a member of the Juicy team told you to.", "OK", "Cancel"))
                    {
                        overrideProp.boolValue = false;
                        return;
                    }
                }
                //If value is change to false then reset the settings to default
                else
                {
                    disableGDPRProp.boolValue = JuicySDKSettings.dflt_DisableGDPRManagement;
                    iosTrackingAuthorizationProp.boolValue = JuicySDKSettings.dflt_DisplayIosTrackingAuthorization;
                    preiOSPopUpProp.boolValue = JuicySDKSettings.dflt_DisplayJuicyIosTrackingPopUp;
                }
            }

            //Value is true: display mediation overridable settings
            if (overrideProp.boolValue)
            {
                GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                EditorGUILayout.PropertyField(disableGDPRProp);
                EditorGUILayout.PropertyField(iosTrackingAuthorizationProp);
                if(iosTrackingAuthorizationProp.boolValue)
                    EditorGUILayout.PropertyField(preiOSPopUpProp);
                GUILayout.EndVertical();
            }
        }

        void DrawInAppSettings()
        {
            DrawSettingsProperties("removeAdsProductID", "restorePurchaseAvailableInEditor","forcePurchaseCancelInEditor","otherProducts");
        }

        void DrawEditorSettings()
        {
            DrawSettingsProperties("skipAdsInEditor","showJuicySDKLogInEditor");
        }

        void DrawABTestSettings()
        {
            SerializedProperty useABTestProp = settings.FindProperty("enableAbTest");
            SerializedProperty keepVariantProp = settings.FindProperty("abTestKeepVariantIndex");
            SerializedProperty variantAmountProp = settings.FindProperty("abTestVariantAmount");
            SerializedProperty forceVariantInEditorProp = settings.FindProperty("forceAbTestVariantInEditor");
            SerializedProperty variantToForceInEditorProp = settings.FindProperty("abTestVariantToForce");
            SerializedProperty variantPopulationProp = settings.FindProperty("abTestVariantPopulation");

            int maxVariantAmount =  JuicySDKSettings.ABTESTMAXPOPULATION / variantPopulationProp.intValue;

            JuicyABTestType abTestType = (JuicyABTestType)settings.FindProperty("abTestType").enumValueIndex;
            EditorGUILayout.PropertyField(useABTestProp);

            if (useABTestProp.boolValue)
            {
                EditorGUILayout.PropertyField(keepVariantProp);
                abTestType = (JuicyABTestType)EditorGUILayout.EnumPopup("AB Test Type",abTestType);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Population tested: " + variantAmountProp.intValue * variantPopulationProp.intValue + "%");

                //On AB Test type changed
                if(abTestType != JuicySDKSettings.Instance.AbTestType)
                {
                    JuicySDKSettings.Instance.ConfigureSettingsForABTest(abTestType);
                }

                switch (abTestType)
                {
                    case JuicyABTestType.Mediation:
                        break;
                    case JuicyABTestType.Custom:
                        variantAmountProp.intValue = EditorGUILayout.IntSlider("Ab Test Variant Amount ", variantAmountProp.intValue, 1, maxVariantAmount);
                        break;
                }

                EditorGUILayout.PropertyField(forceVariantInEditorProp);
                if (forceVariantInEditorProp.boolValue)
                    variantToForceInEditorProp.intValue = EditorGUILayout.IntSlider("Ab Test Variant To Force", variantToForceInEditorProp.intValue, 0, JuicySDKSettings.Instance.AbTestVariantAmount);

                //Override
                EditorGUILayout.Space();
                if (abTestType == JuicyABTestType.Custom)
                    DrawABTestOverrideSettings(variantPopulationProp, variantAmountProp);
            }
        }

        void DrawABTestOverrideSettings(SerializedProperty variantPopulationProp, SerializedProperty variantAmountProp)
        {
            SerializedProperty overrideProp = settings.FindProperty("overrideDefaultAbTestParameters");
            DrawOverrideProperty(overrideProp, "Override Default ABTest Parameters");

            //Check value of the property vs value in settings to detect change in the value
            if (overrideProp.boolValue != JuicySDKSettings.Instance.OverrideDefaultAbTestParameters)
            {
                //Warning when set to true
                if (overrideProp.boolValue)
                {
                    if (!EditorUtility.DisplayDialog("Warning", "You're about to override abtest default paramters. Do this only if a member of the Juicy team told you to.", "OK", "Cancel"))
                    {
                        overrideProp.boolValue = false;
                        return;
                    }
                }
                //If value is change to false then reset the settings to default
                else
                {
                    GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                    variantPopulationProp.intValue = JuicySDKSettings.dflt_AbTestVariantPopulation;
                    GUILayout.EndVertical();
                }
            }

            //Value is true: display mediation overridable settings
            if (overrideProp.boolValue)
            {
                variantPopulationProp.intValue = EditorGUILayout.IntSlider("Ab Test Population per Variant ", variantPopulationProp.intValue, 1, JuicySDKSettings.ABTESTMAXPOPULATION);
                //On value changed: clamp the variant amount to not be over ABTESTMAXPOPULATION tested
                if (variantPopulationProp.intValue != JuicySDKSettings.Instance.AbTestVariantPopulation)
                {
                    while (variantAmountProp.intValue * variantPopulationProp.intValue > JuicySDKSettings.ABTESTMAXPOPULATION)
                    {
                        if (variantAmountProp.intValue == 1)
                            break;
                        variantAmountProp.intValue--;
                    }
                }
            }
        }

        void DrawDebugSettings()
        {
            DrawSettingsProperties("verboseLogs", "useMediationTestSuite", "debugForceRemoveAds", "debugRating");

            SerializedProperty useABTestProp = settings.FindProperty("enableAbTest");
            if (useABTestProp.boolValue)
            {
                SerializedProperty variantPopulationProp = settings.FindProperty("abTestVariantPopulation");
                SerializedProperty variantToForceProp = settings.FindProperty("abTestVariantToForce");
                SerializedProperty forceVariantInBuildProp = settings.FindProperty("forceAbTestVariantInBuild");
                EditorGUILayout.PropertyField(forceVariantInBuildProp);
                if (forceVariantInBuildProp.boolValue)
                    variantToForceProp.intValue = EditorGUILayout.IntSlider("Ab Test Variant To Force", variantToForceProp.intValue, 0, JuicySDKSettings.Instance.AbTestVariantAmount);
            }
        }

        #endregion //Settings
        void DrawOverrideProperty(SerializedProperty property, string label)
        {
            GUILayout.BeginHorizontal();
            GUIStyle style = GUI.skin.label;
            style.richText = true;
            string txt = "";
            txt += property.boolValue ? "<color=orange>" : "";
            txt += label;
            txt += property.boolValue ? "</color>" : "";
            EditorGUILayout.LabelField(txt, style, GUILayout.Width(245));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
        }
        #endregion //Draw
        #region Actions
        void SwitchMode()
        {
            if (JuicyEditorSettings.Instance.IsIntegrationMode)
            {
                SwitchToProductionMode();
            }

            else
            {
                if (!EditorUtility.DisplayDialog("Juicy", "Going back to Inegration mode will reset all the settings to their default values. Are you sure you want to do this ?", "Yes", "Cancel"))
                    return;

                SwitchToIntegrationMode();
            }
        }

        void SwitchToProductionMode()
        {
            EditorUtility.DisplayDialog("Juicy", "The Juicy SDK is now in production mode, please set up you project IDs in the JuicySDSettings window", "Ok");
            JuicyEditorSettings.Instance.IsIntegrationMode = false;
            JuicySDKSettings.Instance.OnProductionModeSwitch();
        }

        void SwitchToIntegrationMode()
        {
            JuicyConfigFileManager.LoadDefaultConfig();

            Undo.RecordObject(JuicySDK.Settings, "Reset SDK Settings");
            JuicySDKSettings.Instance.ResetSettingsToDefault();
            EditorUtility.SetDirty(JuicySDK.Settings);
            JuicyEditorSettings.Instance.IsIntegrationMode = true;
            EditorUtility.DisplayDialog("Juicy", "The Juicy SDK is now back to integration mode", "Ok");
            GUIUtility.ExitGUI();
        }

        void ImportConfigFile()
        {
            AssetDatabase.Refresh();

            if (!JuicyConfigFileManager.IsConfigFileDefault)
            {
                if (!EditorUtility.DisplayDialog("Warning", "A config file is already loaded in your project. Do you want to override it ?", "Override", "Cancel"))
                {
                    return;
                }
            }

            string path = "";
            path = EditorUtility.OpenFilePanel("JuicySDK config file", "", "xml");

            if (path == "")
            {
                GUIUtility.ExitGUI();
                return;
            }

            JuicyConfigFileManager.ImportConfigFile(path);
            EditorUtility.DisplayDialog("Success", "Your JuicySDK config file has been imported", "Ok");
            GUIUtility.ExitGUI();
        }

        void LogConfigFile()
        {
            string log = "--ConfigFile--" + System.Environment.NewLine;
            log += JuicySDKSettings.Instance.GetConfigFileLog();
            Debug.Log(log);
        }

        void ResetConfigFile()
        {
            if (!EditorUtility.DisplayDialog("Reset config file to default", "Reset the config file to default ?", "Reset", "Cancel"))
                return;
            JuicyConfigFileManager.LoadDefaultConfig();
        }

        void ReimportConfigFile()
        {
            if (!EditorUtility.DisplayDialog("Reimport config", "Re import your current config file ?", "Reimport", "Cancel"))
                return;

            JuicyConfigFileManager.ReloadCurrentConfig();
        }

        void RemoveSDK(bool settingsChoice)
        {
            if (EditorUtility.DisplayDialog("Remove JuicySDK", "Remove the Juicy SDK from your project?", "Remove", "Cancel"))
            {
                bool removeSettings = true;
                if (settingsChoice)
                {
                    if (!EditorUtility.DisplayDialog("Remove Settings file", "Do you want to also delete the Juicy SDK Settings (you will loose your Juicy SDk configurations)", "Yes Remove them", "No keep them"))
                        removeSettings = false;
                }
                Close();
                JuicyRemoveManager.Instance.RemoveJuicy(removeSettings,(bool removed)=>
                {
                }
                );
                GUIUtility.ExitGUI();
            }
            else
                return;
        }

        void UpgradeSDK()
        {
            if (!EditorUtility.DisplayDialog("Update JuicySDK", "Do you want to upgrade to a new version of the Juicy SDK", "Upgrade", "Cancel"))
                return;

            string newJuicyPackagePath = EditorUtility.OpenFilePanel("Upgrade Juicy SDK", "", "unitypackage");
            if (newJuicyPackagePath == "")
                return;

            JuicyEditorSettings.Instance.IsUpgrade = true;

            JuicyRemoveManager.Instance.RemoveJuicy(false,(bool removed)=>
            {
                if(removed)
                    AssetDatabase.ImportPackage(newJuicyPackagePath, true);
            }
            );

            Close();
            GUIUtility.ExitGUI();
        }

        void GenerateReport()
        {
            JuicyIntegrationWindow.ShowWindow();
        }

        void DeletePlayerPrefs()
        {
            JuicyPlayerPrefs.DeleteAll();
            EditorUtility.DisplayDialog("Success", "The Juicy Player Prefs have been deleted", "Ok");
        }
        #endregion //Actions
        #region Utils
        void DrawSettingsProperties(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
                DrawProperty(settings, propertyName);
        }

        void DrawSettingsButton(string label, string buttonName, System.Action onClick)
        {
            EditorGUIUtility.labelWidth = 100;
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            if (GUILayout.Button(buttonName, GUILayout.Width(320)))
                onClick?.Invoke();
            GUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = 250;
        }

        SerializedProperty DrawProperty(SerializedObject obj, string propertyName)
        {
            SerializedProperty property = obj.FindProperty(propertyName);
            if (property == null)
                return null;
            EditorGUILayout.PropertyField(property,true);
            return property;
        }

        Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        #endregion //Utils
    }
}
