#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

namespace UniEditor
{
	public static class AssetDatabaseUtility 
	{	
		public static List<AssetType> LoadAssetsAtLocalFolderPath<AssetType>(string a_oAssetDirectoryPathLocal) where AssetType : UnityEngine.Object
		{
			string oAssetDirectoryGlobalFolderPath = EditorAssetPathUtility.LocalToGlobalAssetPath(a_oAssetDirectoryPathLocal);
			return LoadAssetsAtGlobalFolderPath<AssetType>(oAssetDirectoryGlobalFolderPath);
		}
		
		public static List<AssetType> LoadAssetsAtGlobalFolderPath<AssetType>(string a_oAssetDirectoryPathGlobal) where AssetType : UnityEngine.Object
		{
			List<AssetType> oLoadedAssets = new List<AssetType>();
			
			string[] oAssetGlobalPaths = Directory.GetFiles(a_oAssetDirectoryPathGlobal);
			foreach(string oAssetGlobalPath in oAssetGlobalPaths)
			{
				string oAssetLocalPath = EditorAssetPathUtility.GlobalToLocalAssetPath(oAssetGlobalPath);
				AssetType oLoadedAsset = AssetDatabase.LoadAssetAtPath(oAssetLocalPath, typeof(AssetType)) as AssetType;
				if(oLoadedAsset != null)
				{
					oLoadedAssets.Add(oLoadedAsset);
				}
			}
			
			return oLoadedAssets;
		}
		
		public static AssetType LoadAssetAtLocalFolderPath<AssetType>(string a_oAssetDirectoryPathLocal) where AssetType : UnityEngine.Object
		{
			string oAssetDirectoryGlobalFolderPath = EditorAssetPathUtility.LocalToGlobalAssetPath(a_oAssetDirectoryPathLocal);
			return LoadAssetAtGlobalFolderPath<AssetType>(oAssetDirectoryGlobalFolderPath);
		}
		
		public static AssetType LoadAssetAtGlobalFolderPath<AssetType>(string a_oAssetDirectoryPathGlobal) where AssetType : UnityEngine.Object
		{
			string[] oAssetGlobalPaths = Directory.GetFiles(a_oAssetDirectoryPathGlobal);
			foreach(string oAssetGlobalPath in oAssetGlobalPaths)
			{
				string oAssetLocalPath = EditorAssetPathUtility.GlobalToLocalAssetPath(oAssetGlobalPath);
				AssetType oLoadedAsset = AssetDatabase.LoadAssetAtPath(oAssetLocalPath, typeof(AssetType)) as AssetType;
				if(oLoadedAsset != null)
				{
					return oLoadedAsset;
				}
			}
			
			return null;
		}
	}
}
#endif