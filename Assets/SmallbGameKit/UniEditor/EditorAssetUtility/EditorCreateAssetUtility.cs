#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace UniEditor
{
	public static class EditorCreateAssetUtility 
	{
		public static ScriptableObjectType GetOrCreateScriptableObject<ScriptableObjectType>(string assetName, string assetFolderPath) where ScriptableObjectType : ScriptableObject
		{
			string assetPath = GetAssetPath(assetName, assetFolderPath);
			ScriptableObjectType asset = AssetDatabase.LoadAssetAtPath<ScriptableObjectType>(assetPath);

			if(asset != null)
				return asset;

			return CreateScriptableObject<ScriptableObjectType>(assetName, assetFolderPath);
		}

		public static ScriptableObjectType CreateScriptableObject<ScriptableObjectType>(string a_oAssetName, string a_oAssetFolderPath) where ScriptableObjectType : ScriptableObject
		{
			ScriptableObjectType oScriptableObject = ScriptableObject.CreateInstance<ScriptableObjectType>();
			oScriptableObject.name = a_oAssetName;
			
			SaveAsset(oScriptableObject, a_oAssetFolderPath);
			
			return oScriptableObject;
		}
			
		public static ScriptableObjectType CreateScriptableObjectInSelectedProjectFolder<ScriptableObjectType>(string assetName)
			where ScriptableObjectType : ScriptableObject
		{
			ScriptableObjectType scriptableObject = ScriptableObject.CreateInstance<ScriptableObjectType>();
			scriptableObject.name = assetName;
					
			SaveAssetInSelectedProjectFolder(scriptableObject);

			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();

			Selection.activeObject = scriptableObject;

			return scriptableObject;
		}

		public static string GetAssetPath(string a_oAssetName, string a_oAssetFolderPath)
		{
			return a_oAssetFolderPath + "/" + a_oAssetName + ".asset";
		}
		
		public static string SaveAsset(UnityEngine.Object a_oObject, string a_oAssetFolderPath)
		{
			string oAssetPath = GetAssetPath(a_oObject.name, a_oAssetFolderPath);
			AssetDatabase.CreateAsset(a_oObject, oAssetPath);
			
			return oAssetPath;
		}
		
		public static string SaveAssetInSelectedProjectFolder(UnityEngine.Object a_rObjectToSave)
		{
			string oAssetPath = GetCreationPathForNewAssetInSelectedProjectFolder(a_rObjectToSave.name);
			
			AssetDatabase.CreateAsset(a_rObjectToSave, oAssetPath);
			
			return oAssetPath;
		}
		
		public static string GetCreationPathForNewAssetInSelectedProjectFolder(string a_oAssetName)
		{
			string oAssetPath = EditorAssetPathUtility.GetLocalSelectedAssetFolderPath() + a_oAssetName + ".asset";
			oAssetPath = AssetDatabase.GenerateUniqueAssetPath(oAssetPath);
			
			return oAssetPath;
		}
	}
}
#endif