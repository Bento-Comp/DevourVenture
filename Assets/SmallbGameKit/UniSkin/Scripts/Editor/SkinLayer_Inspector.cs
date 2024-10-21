using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UniSkin
{
	[CustomEditor(typeof(SkinLayer))]
	[CanEditMultipleObjects()]
	public class SkinLayer_Inspector : Editor 
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("<-"))
				{
					foreach(SkinLayer skinLayer in targets)
					{
						skinLayer.SelectedSkinIndex--;
					}
				}

				if(GUILayout.Button("->"))
				{
					foreach(SkinLayer skinLayer in targets)
					{
						skinLayer.SelectedSkinIndex++;
					}
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}