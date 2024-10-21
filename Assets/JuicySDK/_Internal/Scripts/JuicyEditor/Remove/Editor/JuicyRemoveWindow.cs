using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace JuicyInternal
{
    public class JuicyRemoveWindow : EditorWindow
    {
        static JuicyRemoveWindow window;
        static List<JuicyManagedAsset> movedFiles;
        static List<JuicyManagedAsset> files;
        static bool removeSettings;

        GUILayoutOption[]  standardButton = { GUILayout.Width(100), GUILayout.Height(20) };
        Vector2 scrollPos;
        static System.Action<bool> onRemoveDone;

        public static void Init(List<JuicyManagedAsset> _files, bool _removeSettings, System.Action<bool> onRemoveDoneCallback)
        {
            onRemoveDone = onRemoveDoneCallback;
            window = (JuicyRemoveWindow)EditorWindow.GetWindow(typeof(JuicyRemoveWindow), true, "Remove Juicy SDK", true);

            movedFiles = new List<JuicyManagedAsset>();
            files = _files;
            removeSettings = _removeSettings;

            foreach (JuicyManagedAsset asset in _files)
            {
                if (asset.HasBeenMoved && !asset.IsDirectory)
                    movedFiles.Add(asset);
            }

            if (movedFiles.Count == 0)
                Remove();
        }

        void OnGUI()
        {
            minSize = new Vector2(600, 800);
            maxSize = new Vector2(600, 800);

            if (files == null || movedFiles == null)
                return;

            EditorGUILayout.Space();
            DrawHeader();
            EditorGUILayout.Space();
            DrawFileSelection();
            DrawFooter();
        }
        #region Draw
        void DrawHeader()
        {
            GUILayout.BeginHorizontal(EditorGUIUtility.IconContent("console.warnicon"), new GUIStyle());
            EditorGUILayout.LabelField("     The following files has been moved from their original location", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();
        }

        void DrawFileSelection()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);

            EditorGUILayout.BeginVertical();
            foreach (JuicyManagedAsset asset in movedFiles)
            {
                EditorGUILayout.BeginHorizontal();
                string str = "at: " + asset.CurrentPath;
                EditorGUILayout.LabelField(asset.Name, str);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        void DrawFooter()
        {
            EditorGUILayout.LabelField("Are you sure you want to remove the Juicy SDK from your project ?", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", standardButton))
            {
                Remove();
            }

            if (GUILayout.Button("Cancel", standardButton))
            {
                Close();
                onRemoveDone?.Invoke(false);
                return;
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion
        #region Actions
        static void Remove()
        {
            Debug.unityLogger.logEnabled = false;
            window.Close();
            List<string> filesToDelete = new List<string>();

            foreach (JuicyManagedAsset asset in files)
            {
                filesToDelete.Add(asset.CurrentPath);
            }

            if (removeSettings)
            {
                string settingsPath = "Assets/JuicySDKSettings";
                if (Directory.Exists(settingsPath))
                {
                    Directory.Delete(settingsPath, true);
                }
            }

            JuicyRemoveManager.DeleteFiles(filesToDelete.ToArray());
            onRemoveDone?.Invoke(true);
            Debug.unityLogger.logEnabled = true;
        }
        #endregion
    }
}

