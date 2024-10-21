using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputButtonMapping_KeyCode")]
	public class InputButtonMapping_KeyCode : InputButtonMapping
	{	
		public KeyCode keyCode;
		
		protected override bool _GetButtonDown()
		{
			return Input.GetKeyDown(keyCode);
		}
		
		public override bool GetButton()
		{
			return Input.GetKey(keyCode);
		}
	}
}