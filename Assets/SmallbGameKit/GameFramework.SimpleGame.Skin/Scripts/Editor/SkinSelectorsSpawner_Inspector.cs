using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

using UnityEngine.UI;

using UniSkin;

using UnityEditor;

using UnityEditor.SceneManagement;

namespace GameFramework.SimpleGame.Skin
{
	[CustomEditor(typeof(SkinSelectorsSpawner))]
	[CanEditMultipleObjects()]
	public class SkinSelectorsSpawner_Inspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if(GUILayout.Button("Force Rebuild"))
			{
				foreach(SkinSelectorsSpawner skinSelectorSpawner in targets)
				{
					skinSelectorSpawner.ForceRebuild();
					
					EditorUtility.SetDirty(skinSelectorSpawner);
					EditorSceneManager.MarkSceneDirty(skinSelectorSpawner.gameObject.scene);
				}
			}
		}
	}
}