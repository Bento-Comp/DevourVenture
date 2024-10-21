using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JuicyInternal
{
    [CustomEditor(typeof(JuicyEditorSettings))]
    public class JuicyEditorSettings_Inspector : Editor
    {
        bool toggle = false;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            JuicyEditorSettings settings = (JuicyEditorSettings)target;
            EditorGUILayout.LabelField("Juicy Editor Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("This file is used by the Juicy SDK to save datas. Do not delete/rename or move it");

            toggle = EditorGUILayout.Foldout(toggle, "Debug");
            if (toggle)
            {
                EditorGUILayout.LabelField("First Import: ", settings.IsFirstImport.ToString());
                EditorGUILayout.LabelField("Integration Mode: ", settings.IsIntegrationMode.ToString());
            }
        }
    }
}
