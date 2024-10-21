using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using Juicy;

namespace JuicyInternal
{
	[CustomEditor(typeof(JuicySDKSettings))]
	public class JuicySDKSettings_Inspector : Editor
	{
		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();

			JuicySDKSettings settings = (JuicySDKSettings)target;
            EditorGUILayout.LabelField("Juicy SDK Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("The file is use to store the Juicy SDK Settings. To edit those settings go to JuicySDk > Juicy SDK Settings");
            if(GUILayout.Button("Open Settings Window"))
            {
                JuicySDKSettingsWindow.ShowWindow();
            }

		}
	}
}