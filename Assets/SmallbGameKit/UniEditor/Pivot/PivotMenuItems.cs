#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UniEditor
{
	public class PivotMenuItems : Editor
	{
		[MenuItem("UniEditor/Add Pivot")]
		static void DoAddPivot()
		{
			GameObject[] rSelectedGameObjects = Selection.gameObjects;
			if(rSelectedGameObjects.Length > 0)
			{
				List<GameObject> oCreatedGameObjects = new List<GameObject>();
				foreach(GameObject rSelectedGameObject in rSelectedGameObjects)
				{
					oCreatedGameObjects.Add(DoAddPivot(rSelectedGameObject));
				}
				Selection.objects = oCreatedGameObjects.ToArray();
			}
		}
		
		// Add pivot
		static GameObject DoAddPivot(GameObject a_rGameObject)
		{
			GameObject rPivot = new GameObject(a_rGameObject.name + "_Pivot");
			
			Transform rPivotTransform = rPivot.transform;
			Transform rSelectedTransform = a_rGameObject.transform;
			
			rPivotTransform.parent = rSelectedTransform.parent;
			rPivotTransform.localPosition = rSelectedTransform.localPosition;
			rPivotTransform.localRotation = rSelectedTransform.localRotation;
			//rPivotTransform.localScale = rSelectedTransform.localScale;
			rPivotTransform.localScale = Vector3.one;

			//rSelectedTransform.parent = rPivotTransform;
	
			Undo.RegisterCreatedObjectUndo(rPivot, "Add Pivot");
			Undo.SetTransformParent(rSelectedTransform, rPivotTransform, "Add Pivot");
	
			return rPivot;
		}
		
		[MenuItem("UniEditor/Fix Pivot")]
		static void DoFixPivot()
		{
			GameObject rSelected = Selection.activeGameObject;
			if(rSelected != null)
			{
				Transform rSelectedTransform = rSelected.transform;
				Transform rPivotTransform = rSelectedTransform.parent;
				
				if(rPivotTransform != null)
				{
					rSelectedTransform.parent = rPivotTransform.parent;
					
					rPivotTransform.parent = rSelectedTransform.parent;
					rPivotTransform.localPosition = rSelectedTransform.localPosition;
					rPivotTransform.localRotation = rSelectedTransform.localRotation;
					rPivotTransform.localScale = rSelectedTransform.localScale;
					
					rSelectedTransform.parent = rPivotTransform;
					
					Selection.activeGameObject = rSelected;
				}
			}
		}
	}
}
#endif