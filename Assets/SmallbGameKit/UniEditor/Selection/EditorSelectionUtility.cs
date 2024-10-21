#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

namespace UniEditor
{
	public static class EditorSelectionUtility
	{
		public static bool IsSelected(GameObject a_rGameObject)
		{
			foreach(GameObject rGameObjectSelected in Selection.gameObjects)
			{
				if(rGameObjectSelected == a_rGameObject)
				{
					return true;
				}
			}
			
			return false;
		}
		
		public static TObjectType GetSelected<TObjectType>() where TObjectType : Object
		{
			foreach(Object rSelectedObject in Selection.objects)
			{
				if(rSelectedObject is TObjectType)
				{
					return rSelectedObject as TObjectType;
				}
			}
			
			return null;
		}
		
		public static TComponentType GetComponentOnSelected<TComponentType>() where TComponentType : Component
		{
			foreach(Object rSelectedObject in Selection.objects)
			{
				GameObject rSelectedGameObject = rSelectedObject as GameObject; 
				if(rSelectedGameObject != null)
				{
					TComponentType rSelectedComponent = rSelectedGameObject.GetComponent<TComponentType>();
					if(rSelectedComponent != null)
					{
						return rSelectedComponent;
					}
				}
			}
			
			return null;
		}
		
		public static void UnselectAll()
		{
			Selection.objects = new Object[0];
		}
		
		public static void Select(Component a_rComponent)
		{
			Select(a_rComponent.gameObject);
		}
		
		public static void Select(Object a_rObject)
		{
			Selection.objects = new UnityEngine.Object[]{a_rObject};
		}
		
		public static void AddToSelection(Object a_rObject)
		{
			List<Object> oSelectedObjects = new List<Object>();
			oSelectedObjects.InsertRange(0, Selection.objects);
			
			if(oSelectedObjects.Contains(a_rObject) == false)
			{
				oSelectedObjects.Add(a_rObject);
			}
		
			Selection.objects = oSelectedObjects.ToArray();
		}
	
		public static void RemoveFromSelection(GameObject a_rObject)
		{
			List<Object> oSelectedObjects = new List<Object>();
			oSelectedObjects.InsertRange(0, Selection.objects);
			
			oSelectedObjects.Remove(a_rObject);
		
			Selection.objects = oSelectedObjects.ToArray();
		}
	}
}
#endif