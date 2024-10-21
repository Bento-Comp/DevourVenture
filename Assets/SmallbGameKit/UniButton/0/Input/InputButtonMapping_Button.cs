using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputButtonMapping_Button")]
	public class InputButtonMapping_Button : InputButtonMapping
	{	
		public string buttonName;
		
		protected override bool _GetButtonDown()
		{
			return Input.GetButtonDown(buttonName);
		}
		
		public override bool GetButton()
		{
			return Input.GetButton(buttonName);
		}
	}
}