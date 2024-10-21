using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[CustomEditor(typeof(InputManager))]
	[CanEditMultipleObjects]
	public class InputManagerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			if(GUILayout.Button("Auto Fill"))
			{
				foreach(Object rTarget in targets)
				{
					InputManager rComponent = rTarget as InputManager;
					if(rComponent != null)
					{
						rComponent.EditorOnly_AutoFill();
					}
				}
			}
		}
	}
}