using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinList")]
	public class SkinList : MonoBehaviour
	{
		public List<Skin> skins = new List<Skin>();

		public List<Skin> skinSeeds = new List<Skin>();

		#if UNITY_EDITOR
		public void FillFromSeeds()
		{
			skins.Clear();
			foreach(Skin skinSelectorSeed in skinSeeds)
			{
				string assetPath = UniEditor.EditorAssetPathUtility.GetAssetDirectoryPath(skinSelectorSeed);
				//Debug.Log(assetPath);
				string[] assetGUIDs = AssetDatabase.FindAssets("", new string[]{assetPath});
				foreach(string assetGUID in assetGUIDs)
				{
					string path = AssetDatabase.GUIDToAssetPath(assetGUID);
					//Debug.Log(path);

					Skin asset = AssetDatabase.LoadAssetAtPath<Skin>(path);

					if(asset == null)
						continue;

					Skin levelAsset = AssetDatabase.LoadAssetAtPath<Skin>(path);
					skins.Add(levelAsset);

					EditorUtility.SetDirty(levelAsset);
				}
			}
			EditorSceneManager.MarkSceneDirty(gameObject.scene);
		}
		#endif
	}
}
