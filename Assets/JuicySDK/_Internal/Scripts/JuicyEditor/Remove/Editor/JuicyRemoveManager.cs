using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

namespace JuicyInternal
{
    [System.Serializable]
    public class JuicyManagedAsset
    {
        [SerializeField] string originPath;
        [SerializeField] string guid;

        public string OriginPath
        {
            get { return originPath; }
            set { originPath = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string Name { get { return Path.GetFileName(OriginPath); } }
        public string CurrentPath { get { return AssetDatabase.GUIDToAssetPath(Guid); } }
        public bool Exist { get { return File.Exists(CurrentPath) || Directory.Exists(CurrentPath); } }
        public bool IsDirectory { get { return Exist && Directory.Exists(CurrentPath); } }
        public bool HasBeenMoved { get { return Exist && CurrentPath != OriginPath; } }

        public JuicyManagedAsset(string path)
        {
            OriginPath = path;
        }

        public void SetGuid()
        {
            Guid = AssetDatabase.AssetPathToGUID(OriginPath);
        }

        public override string ToString()
        {
            return Name + ": origin -> " + originPath + " current -> " + CurrentPath;
        }
    }

    public class JuicyRemoveManager : ScriptableObject
    {
        static JuicyRemoveManager instance;
        public static JuicyRemoveManager Instance
        {
            get
            {
                if (instance == null)
                    CreateSettings();

                return instance;
            }
        }

        const string FolderPath = "Assets/JuicySDK/_Internal/Scripts/FileManagement/Resources";
        const string FilePath = "Assets/JuicySDK/_Internal/Scripts/FileManagement/Resources/JuicyRemoveManager.asset";

        public List<JuicyManagedAsset> JuicyAssets = new List<JuicyManagedAsset>();

        static void CreateSettings()
        {
            instance = Resources.Load<JuicyRemoveManager>("JuicyRemoveManager");

            if (instance == null)
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                JuicyRemoveManager asset = ScriptableObject.CreateInstance<JuicyRemoveManager>();
                AssetDatabase.CreateAsset(asset, FilePath);
                AssetDatabase.Refresh();

                AssetDatabase.SaveAssets();
                instance = asset;
            }
        }

        public void SetJuicyFiles(string[] pathes)
        {
            JuicyAssets.Clear();
            foreach (string path in pathes)
            {
                JuicyManagedAsset asset = new JuicyManagedAsset(path);
                asset.SetGuid();
                JuicyAssets.Add(asset);
            }
            SaveSettings();
        }

        public void RemoveJuicy(bool removeSettings, System.Action<bool> onRemoveDoneCallback)
        {
            JuicyRemoveWindow.Init(JuicyAssets,removeSettings, onRemoveDoneCallback);
        }

        public static void DeleteFiles(string[] paths)
        {
            List<string> pathsToDelete = new List<string>(paths);

            /* ---Delete Files ---*/

            //Reverse loop to be able to remove from list while enumerating
            for (int i = pathsToDelete.Count - 1; i >= 0; i--)
            {
                if (File.Exists(pathsToDelete[i]))
                {
                    File.Delete(pathsToDelete[i]);
                    //Delete the .meta
                    if (File.Exists(pathsToDelete[i] + ".meta"))
                        File.Delete(pathsToDelete[i] + ".meta");

                    pathsToDelete.RemoveAt(i);
                }
            }
            AssetDatabase.Refresh();

            /*--- Delete Directories ---*/

            //Orders path by depth from deepest to shallowest -> Delete the deepest path first
            pathsToDelete = pathsToDelete.OrderByDescending(GetPathDepth).ToList();

            for (int i = 0; i < pathsToDelete.Count; i++)
            {
                if (Directory.Exists(pathsToDelete[i]))
                {
                    //Only delete the empty ones
                    if (Directory.GetFiles(pathsToDelete[i]).Length == 0)
                    {
                        Directory.Delete(pathsToDelete[i]);
                        //Delete the .meta
                        if (File.Exists(pathsToDelete[i] + ".meta"))
                            File.Delete(pathsToDelete[i] + ".meta");
                        //Need to refresh each time in case of nested directories
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        static int GetPathDepth(string path)
        {
            return path.Split(Path.DirectorySeparatorChar).Length;
        }

        internal void SaveSettings()
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
        }
    }
}
