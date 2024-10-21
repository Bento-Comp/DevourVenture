using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputManager")]
	public class InputManager : MonoBehaviour
	{	
		public List<InputMappingScheme> inputMappingSchemes;
		
		Dictionary<string, InputMappingScheme> inputMappingSchemesByName = new Dictionary<string, InputMappingScheme>();
		
		HashSet<InputMappingScheme> currentInputMappingSchemes = new HashSet<InputMappingScheme>();
		
		static HashSet<string> ms_oActiveInputMappingSchemeNames = new HashSet<string>();
		
		static InputManager ms_oInstance;

		
		static public InputManager Instance
		{
			get
			{
				return ms_oInstance;
			}
		}
		
		public static void ActivateScheme(string a_oSchemeName, bool a_bActivate = true)
		{
			if(a_bActivate)
			{
				if(ms_oActiveInputMappingSchemeNames.Add(a_oSchemeName))
				{
					if(ms_oInstance != null)
					{
						ms_oInstance.AddToActiveScheme(a_oSchemeName);
					}
				}
			}
			else
			{
				if(ms_oActiveInputMappingSchemeNames.Remove(a_oSchemeName))
				{
					if(ms_oInstance != null)
					{
						ms_oInstance.RemoveFromActiveScheme(a_oSchemeName);
					}
				}
			}
		}
		
		public static bool GetButtonDown(string a_oButtonName)
		{
			foreach(InputMappingScheme rInputMappingScheme in ms_oInstance.currentInputMappingSchemes)
			{
				if(rInputMappingScheme.GetButtonDown(a_oButtonName))
				{
					return true;
				}
			}
			return false;
		}
		
		public static bool GetButton(string a_oButtonName)
		{
			foreach(InputMappingScheme rInputMappingScheme in ms_oInstance.currentInputMappingSchemes)
			{
				if(rInputMappingScheme.GetButton(a_oButtonName))
				{
					return true;
				}
			}
			return false;
		}

		public static float GetAxisValue(string axisName)
		{
			float selectedAxisValue = 0.0f;
			float maxAxisMagnitude = 0.0f;
			foreach(InputMappingScheme inputMappingScheme in ms_oInstance.currentInputMappingSchemes)
			{
				float axisValue = inputMappingScheme.GetAxisValue(axisName);
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
			if(inputMappingSchemes == null)
			{
				inputMappingSchemes = new List<InputMappingScheme>();
			}
			else
			{
				inputMappingSchemes.Clear();
			}
			
			inputMappingSchemes.AddRange(GetComponentsInChildren<InputMappingScheme>());
			foreach(InputMappingScheme rInputMappingScheme in inputMappingSchemes)
			{
				rInputMappingScheme.EditorOnly_AutoFill();
			}
		}
		#endif
		
		void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				UnityEngine.Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
			
			GetInputMappings();
			SetActiveSchemes(ms_oActiveInputMappingSchemeNames);
		}
		
		void GetInputMappings()
		{
			foreach(InputMappingScheme rInputMappingScheme in inputMappingSchemes)
			{
				inputMappingSchemesByName.Add(rInputMappingScheme.name, rInputMappingScheme);
			}
		}
		
		void SetActiveSchemes(IEnumerable<string> a_rSchemeNames)
		{
			currentInputMappingSchemes.Clear();
			
			foreach(string oSchemeName in a_rSchemeNames)
			{
				AddToActiveScheme(oSchemeName);
			}
		}
		
		void AddToActiveScheme(string a_oSchemeName)
		{
			InputMappingScheme rInputMappingScheme;
			if(inputMappingSchemesByName.TryGetValue(a_oSchemeName, out rInputMappingScheme))
			{
				currentInputMappingSchemes.Add(rInputMappingScheme);
			}
		}
		
		void RemoveFromActiveScheme(string a_oSchemeName)
		{
			InputMappingScheme rInputMappingScheme;
			if(inputMappingSchemesByName.TryGetValue(a_oSchemeName, out rInputMappingScheme))
			{
				currentInputMappingSchemes.Remove(rInputMappingScheme);
			}
		}
	}
}