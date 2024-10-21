 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/ButtonHub/ButtonHubController")]
	public class ButtonHubController : MonoBehaviour
	{
		public Button controlledButton;
		
		public List<Button> inputButtons;
		
		void Awake()
		{
			controlledButton.onEnable += OnControlledButtonEnable;
			UpdateControlledButtonState(false);
			foreach(Button rButton in inputButtons)
			{
				if(rButton != null)
				{
					rButton.onStateChange += OnButtonStateChange;
				}
			}
		}
		
		void OnDestroy()
		{
			foreach(Button rButton in inputButtons)
			{
				if(rButton != null)
				{
					rButton.onStateChange -= OnButtonStateChange;
				}
			}
			controlledButton.onEnable -= OnControlledButtonEnable;
		}

		void OnControlledButtonEnable()
		{
			UpdateControlledButtonState(false);
		}
		
		void OnButtonStateChange(Button a_rButton, bool a_bCanceled)
		{
			if(IsDeactivated())
			{
				return;
			}
			UpdateControlledButtonState(a_bCanceled);
		}
		
		void UpdateControlledButtonState(bool a_bCanceled)
		{
			bool bButtonPressed = IsAtLeastAButtonPressed();
			if(controlledButton.Pressed)
			{
				if(bButtonPressed == false)
				{
					if(a_bCanceled)
					{
						controlledButton.Cancel();
					}
					else
					{
						controlledButton.Release();	
					}
				}
			}
			else
			{
				if(bButtonPressed)
				{
					controlledButton.Press();
				}
			}
		}
		
		bool IsAtLeastAButtonPressed()
		{
			foreach(Button rButton in inputButtons)
			{
				if(rButton != null && rButton.Pressed)
				{
					return true;
				}
			}
			
			return false;
		}

		bool IsDeactivated()
		{
			return controlledButton.enabled == false || controlledButton.gameObject.activeInHierarchy == false || enabled == false || gameObject.activeInHierarchy == false;
		}
	}
}