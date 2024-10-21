using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ReplaceBy
{
	public static class ReplaceByUtility
	{
		public static void ReplaceBy(List<GameObject> gameObjectToReplaces, GameObject gameObjectModelReplacing, ref List<GameObject> createdGameObjects,
		                             bool scaleOverride = false, bool replaceName = false)
		{
			if(gameObjectToReplaces == null || gameObjectToReplaces.Count <= 0 || gameObjectModelReplacing == null)
				return;

			foreach(GameObject gameObjectToReplace in gameObjectToReplaces)
			{
				GameObject createdGameObject = ReplaceBy(gameObjectToReplace, gameObjectModelReplacing, scaleOverride, replaceName);
				if(createdGameObject != null && createdGameObjects != null)
				{
					createdGameObjects.Add(createdGameObject);
				}
			}
		}

		public static GameObject ReplaceBy(GameObject gameObjectToReplace, GameObject gameObjectModelReplacing, bool scaleOverride, bool replaceName)
		{
			if(gameObjectToReplace == null || gameObjectModelReplacing == null)
				return null;

			GameObject createdGameObjectReplacing = null;
			#if UNITY_EDITOR
			if(EditorUtility.IsPersistent(gameObjectModelReplacing))
			{
				createdGameObjectReplacing = PrefabUtility.InstantiatePrefab(gameObjectModelReplacing) as GameObject;
			}

			if(createdGameObjectReplacing == null)
			{
				createdGameObjectReplacing = GameObject.Instantiate(gameObjectModelReplacing) as GameObject;
			}
			#else
			createdGameObjectReplacing = GameObject.Instantiate(gameObjectModelReplacing) as GameObject;
			#endif

			if(replaceName == false)
				createdGameObjectReplacing.name = gameObjectToReplace.name;

			createdGameObjectReplacing.SetActive(gameObjectToReplace.activeSelf);
			createdGameObjectReplacing.transform.parent = gameObjectToReplace.transform.parent;
			createdGameObjectReplacing.transform.SetSiblingIndex(gameObjectToReplace.transform.GetSiblingIndex());
			createdGameObjectReplacing.transform.localPosition = gameObjectToReplace.transform.localPosition;
			createdGameObjectReplacing.transform.localRotation = gameObjectToReplace.transform.localRotation;

			if(scaleOverride)
			{
				createdGameObjectReplacing.transform.localScale = gameObjectModelReplacing.transform.localScale;
			}
			else
			{
				createdGameObjectReplacing.transform.localScale = gameObjectToReplace.transform.localScale;
			}

			#if UNITY_EDITOR
			string undoName = "Replace " + gameObjectToReplace + "By " + createdGameObjectReplacing;
			Undo.RegisterCreatedObjectUndo(createdGameObjectReplacing, undoName);
			Undo.RegisterFullObjectHierarchyUndo(gameObjectToReplace, undoName);
			#endif

			GameObject.DestroyImmediate(gameObjectToReplace);

			return createdGameObjectReplacing;
		}
	}
}