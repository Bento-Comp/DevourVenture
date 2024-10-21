#if UNITY_IOS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.iOS.Xcode;

namespace JuicyInternal
{
    public class PBXProjectUtility
    {
        public static bool TryGetPBXProject(string projectPath, out PBXProject project)
        {
            project = new PBXProject();

            try
            {
                project.ReadFromFile(projectPath);
            }

            catch
            {
                project = null;
                return false;
            }

            return true;
        }

        public static void AddFramework(PBXProject project, string framework, bool optionnal = false)
        {
            string guid;
#if UNITY_2019_3_OR_NEWER
            guid = project.GetUnityMainTargetGuid();
#else
            string targetName = PBXProject.GetUnityTargetName();
            guid = project.TargetGuidByName(targetName);
#endif

            project.AddFrameworkToProject(guid, framework + ".framework", optionnal);
        }
    }
}
#endif
