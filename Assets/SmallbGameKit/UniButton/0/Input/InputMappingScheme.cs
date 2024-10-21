using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputMappingScheme")]
	public class InputMappingScheme : MonoBehaviour
	{	
		public List<InputButtonMappingList> inputButtonMappingLists;
		
		Dictionary<string, InputButtonMappingList> inputButtonMappingListsByName = new Dictionary<string, InputButtonMappingList>();

		public bool GetButtonDown(string a_oButtonName)
		{
			InputButtonMappingList rInputButtonMappingList;
			if(inputButtonMappingListsByName.TryGetValue(a_oButtonName, out rInputButtonMappingList))
			{
				return rInputButtonMappingList.GetButtonDown();
			}
			else
			{
				return false;
			}
		}
		
		public bool GetButton(string a_oButtonName)
		{
			InputButtonMappingList rInputButtonMappingList;
			if(inputButtonMappingListsByName.TryGetValue(a_oButtonName, out rInputButtonMappingList))
			{
				return rInputButtonMappingList.GetButton();
			}
			else
			{
				return false;
			}
		}

		public float GetAxisValue(string axisName)
		{
			InputButtonMappingList rInputButtonMappingList;
			if(inputButtonMappingListsByName.TryGetValue(axisName, out rInputButtonMappingList))
			{
				return rInputButtonMappingList.GetAxisValue();
			}
			else
			{
				return 0.0f;
			}
		}
		
		#if UNITY_EDITOR
		public void EditorOnly_AutoFill()
		{
			if(inputButtonMappingLists == null)
			{
				inputButtonMappingLists = new List<InputButtonMappingList>();
			}
			else
			{
				inputButtonMappingLists.Clear();
			}
			
			inputButtonMappingLists.AddRange(GetComponentsInChildren<InputButtonMappingList>());
			foreach(InputButtonMappingList rInputButtonMappingList in inputButtonMappingLists)
			{
				rInputButtonMappingList.EditorOnly_AutoFill();
			}
		}
		#endif
		
		void Awake()
		{
			GetButtons();
		}
		
		void GetButtons()
		{
			foreach(InputButtonMappingList rInputButtonMappingList in inputButtonMappingLists)
			{
				inputButtonMappingListsByName.Add(rInputButtonMappingList.name, rInputButtonMappingList);
			}
		}
	}
}