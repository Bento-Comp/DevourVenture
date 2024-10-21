#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace UniEditor
{
	public static class EditorAssetPrefabUtility 
	{
		public static ComponentType CreatePrefab<ComponentType>(string a_oPrefabName) where ComponentType : Component
		{
			return CreatePrefab<ComponentType>(a_oPrefabName, EditorAssetPathUtility.GetLocalSelectedAssetFolderPath());
		}
		
		public static ComponentType CreatePrefab<ComponentType>(string a_oPrefabName, string a_oAssetPath) where ComponentType : Component
		{
			a_oAssetPath = AssetDatabase.GenerateUniqueAssetPath(a_oAssetPath + "/" + a_oPrefabName + ".prefab");

			ComponentType oComponent = ComponentBuilderUtility.BuildComponent<ComponentType>();
			
			ComponentType oComponentPrefab = PrefabUtility.SaveAsPrefabAsset(oComponent.gameObject, a_oAssetPath).GetComponent<ComponentType>();
			
			GameObject.DestroyImmediate(oComponent.gameObject);
			
			return oComponentPrefab;
		}
		
		public static string GetCreationPathForNewPrefabInSelectedProjectFolder(string a_rPrefabName)
		{
			string oAssetPath = EditorAssetPathUtility.GetLocalSelectedAssetFolderPath() + a_rPrefabName + ".prefab";
			oAssetPath = AssetDatabase.GenerateUniqueAssetPath(oAssetPath);
			
			return oAssetPath;
		}
		
		public static void PingPrefabInProjectView(GameObject a_rGameObject)
		{
			GameObject rGameObjectToPing = a_rGameObject;
			if(a_rGameObject.transform.parent != null && a_rGameObject.transform.parent.parent != null)
			{
				rGameObjectToPing = PrefabUtility.GetOutermostPrefabInstanceRoot(a_rGameObject);
			}
			
			EditorGUIUtility.PingObject(rGameObjectToPing);
		}
	}
}
#endif