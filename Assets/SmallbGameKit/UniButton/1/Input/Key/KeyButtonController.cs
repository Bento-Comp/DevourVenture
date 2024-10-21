 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Key/KeyButtonController")]
	public class KeyButtonController : MonoBehaviour
	{
		public Button controlledButton;
		
		public List<string> keys;
		
		bool buttonEnabled;
		
		void OnEnable()
		{
			OnButtonEnable();
		}
		
		void OnDisable()
		{
			OnButtonDisable();
		}
		
		void Update()
		{
			bool bButtonEnabled;
			if(controlledButton == null)
			{
				bButtonEnabled = true;
			}
			else
			{
				bButtonEnabled = controlledButton.enabled;
			}
			
			if(bButtonEnabled != buttonEnabled)
			{
				if(bButtonEnabled)
				{
					OnButtonEnable();
				}
				else
				{
					OnButtonDisable();
				}
			}
			if(buttonEnabled)
			{
				UpdateKey();
			}
		}
		
		void UpdateKey()
		{		
			if(controlledButton.Pressed)
			{
				if(IsAtLeastAKeyPressed() == false)
				{
					controlledButton.Release();
				}
			}
			else
			{
				if(IsAtLeastAKeyDown())
				{
					controlledButton.Press();
				}
			}
		}
		
		bool IsAtLeastAKeyPressed()
		{
			foreach(string oKey in keys)
			{
				if(InputManager.GetButton(oKey))
				{
					return true;
				}
			}
			
			return false;
		}
		
		bool IsAtLeastAKeyDown()
		{
			foreach(string oKey in keys)
			{
				if(InputManager.GetButtonDown(oKey))
				{
					return true;
				}
			}
			
			return false;
		}
		
		void OnButtonEnable()
		{
			buttonEnabled = true;
		}
		
		void OnButtonDisable()
		{
			Cancel();
		}
		
		public void Cancel()
		{
			CancelButton();
			buttonEnabled = false;
		}
		
		void CancelButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Cancel();
			}
		}
	}
}