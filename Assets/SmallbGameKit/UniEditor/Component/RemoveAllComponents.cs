#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UniEditor
{
	public static class RemoveAllComponents
	{
		[MenuItem("UniEditor/Remove All Non Visual Components")]
		static void DoRemoveAllNonVisualComponents()
		{
			GameObject[] selectedGameObjects = Selection.gameObjects;
			if(selectedGameObjects.Length > 0)
			{
				foreach(GameObject selectedGameObject in selectedGameObjects)
				{
					DoRemoveAllNonVisualComponents(selectedGameObject);
				}
			}
		}

		static void DoRemoveAllNonVisualComponents(GameObject gameObject)
		{
			Undo.RegisterFullObjectHierarchyUndo(gameObject, "Remove All Non Visual Components");

			foreach(Component component in gameObject.GetComponents<Component>())
			{
				if(IsVisualComponents(component))
					continue;
				
				GameObject.DestroyImmediate(component);
			}
		}

		static bool IsVisualComponents(Component component)
		{
			if(component is Transform)
				return true;

			if(component is Renderer)
				return true;

			if(component is MeshFilter)
				return true;

			if(component is Camera)
				return true;

			if(component is ParticleSystem)
				return true;

			if(component is Terrain)
				return true;

			if(component is Light)
				return true;

			return false;
		}
	}
}
#endif