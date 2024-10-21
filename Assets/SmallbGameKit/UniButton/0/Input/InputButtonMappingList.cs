using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputButtonMappingLists")]
	public class InputButtonMappingList : MonoBehaviour
	{	
		public List<InputButtonMapping> inputButtonMappings;
		
		public bool GetButtonDown()
		{
			foreach(InputButtonMapping rButtonMapping in inputButtonMappings)
			{
				if(rButtonMapping.GetButtonDown())
				{
					return true;
				}
			}
			
			return false;
		}
		
		public bool GetButton()
		{
			foreach(InputButtonMapping rButtonMapping in inputButtonMappings)
			{
				if(rButtonMapping.GetButton())
				{
					return true;
				}
			}
			
			return false;
		}

		public float GetAxisValue()
		{
			float selectedAxisValue = 0.0f;
			float maxAxisMagnitude = 0.0f;
			foreach(InputButtonMapping rButtonMapping in inputButtonMappings)
			{
				float axisValue = rButtonMapping.GetAxisValue();
				float axisMagnitude = Mathf.Abs(axisValue);
				if(axisMagnitude > maxAxisMagnitude)
				{
					maxAxisMagnitude = axisMagnitude;
					selectedAxisValue = axisValue;
				}
			}
			
			return selectedAxisValue;
		}

		#if UNITY_EDITOR
		public void EditorOnly_AutoFill()
		{
			if(inputButtonMappings == null)
			{
				inputButtonMappings = new List<InputButtonMapping>();
			}
			else
			{
				inputButtonMappings.Clear();
			}
			
			inputButtonMappings.AddRange(GetComponentsInChildren<InputButtonMapping>());
		}
		#endif
	}
}