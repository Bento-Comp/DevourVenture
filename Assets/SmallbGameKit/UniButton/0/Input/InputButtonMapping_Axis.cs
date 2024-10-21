using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputButtonMapping_Axis")]
	public class InputButtonMapping_Axis : InputButtonMapping
	{	
		public enum EAxisDirection
		{
			Positive,
			Negative
		}

		[Serializable]
		public class ConcurentialAxis
		{
			public string name;

			public float GetAxisValue()
			{
				return Input.GetAxis(name);
			}
		}

		public string axisName;
		
		public EAxisDirection axisDirection;

		public float deadZone = 0.0f;

		public List<ConcurentialAxis> concurentialAxes;

		public float concurentialAxisTreshold = 0.1f;
		
		bool buttonWasPressed;
		
		bool buttonDown;
		
		protected override bool _GetButtonDown()
		{
			return buttonDown;
		}
		
		public override bool GetButton()
		{
			float axis = Input.GetAxis(axisName);
			bool buttonPressed = false;
			switch(axisDirection)
			{
				case EAxisDirection.Positive:
				{
					buttonPressed = axis > deadZone;
				}
				break;
				
				case EAxisDirection.Negative:
				default:
				{
					buttonPressed = axis < -deadZone;
				}
				break;
			}

			if(buttonPressed)
			{
				if(IsOneConcurentialAxisGreater(axis))
				{
					buttonPressed = false;
				}
			}
			
			/*if(bButtonPressed)
			{
				//Debug.Log("GetButton? " + axisName + ", " + axisDirection + " : " + bButtonPressed + ", " + fAxis);
			}*/
			
			return buttonPressed;
		}

		public override float GetAxisValue()
		{
			float axis = Input.GetAxis(axisName);
			if(IsOneConcurentialAxisGreater(axis))
			{
				return 0.0f;
			}
			return axis;
		}

		protected override void Update()
		{
			base.Update();

			bool bButtonIsPressed = GetButton();
			
			buttonDown = buttonWasPressed == false && bButtonIsPressed;
			
			buttonWasPressed = bButtonIsPressed;
		}

		bool IsOneConcurentialAxisGreater(float axis)
		{
			foreach(ConcurentialAxis concurentialAxis in concurentialAxes)
			{
				if(Mathf.Abs(axis) - Mathf.Abs(concurentialAxis.GetAxisValue()) < -concurentialAxisTreshold)
				{
					return true;
				}
			}
			return false;
		}
	}
}