using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UniSkin
{
	[CustomEditor(typeof(SkinList))]
	[CanEditMultipleObjects()]
	public class SkinList_Inspector : Editor 
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if(GUILayout.Button("Fill From Seeds"))
			{
				foreach(SkinList skinList in targets)
				{
					skinList.FillFromSeeds();

					EditorUtility.SetDirty(skinList);
					EditorSceneManager.MarkSceneDirty(skinList.gameObject.scene);
				}
			}
		}
	}
}