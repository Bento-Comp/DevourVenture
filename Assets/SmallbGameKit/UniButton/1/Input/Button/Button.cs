using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Button")]
	public class Button : MonoBehaviour
	{
		public enum EButtonType
		{
			ClickButton,
			ToggleButton
		}
		
		[Serializable]
		public class HoldParameters
		{
			public float holdDelay = 0.5f;
			
			float holdDuration;
			
			bool held;
			
			public bool IsHeld
			{
				get
				{
					return held;
				}
			}
			
			public void OnDown()
			{
				holdDuration = 0.0f;
			}
			
			public void OnUp()
			{
				held = false;
				holdDuration = 0.0f;
			}
			
			public bool Update()
			{
				holdDuration += Time.deltaTime;
				if(held == false)
				{
					if(holdDuration >= holdDelay)
					{
						held = true;
						return true;
					}
				}	
				return false;
			}
		}
		
		[Serializable]
		public class RepeatParameters
		{
			public bool repeatButton;
			
			public float firstRepeatButtonDelay = 0.25f;
			
			public float repeatButtonPeriod = 0.05f;
			
			float pressedTimeRemainingBeforeNextRepeat;

			bool repeating;
			
			public bool Update()
			{
				repeating = false;
				if(repeatButton)
				{
					pressedTimeRemainingBeforeNextRepeat -= Time.deltaTime;
					if(pressedTimeRemainingBeforeNextRepeat <= 0.0f)
					{
						repeating = true;
						return true;
					}
				}
				return false;
			}
			
			public void OnDown()
			{
				if(repeating)
				{
					pressedTimeRemainingBeforeNextRepeat = repeatButtonPeriod;
				}
				else
				{
					pressedTimeRemainingBeforeNextRepeat = firstRepeatButtonDelay;
				}
			}
		}
		
		[Serializable]
		public class CancelParameters
		{
			public bool onHold;
			public bool onDisable;
		}
		
		[Serializable]
		public class ToggleParameters
		{
			public bool onHold;
		}
		
		public Action<Button, bool> onStateChange;

		public Action onEnable;

		public Action<bool> onToggle;
		
		public Action onDown;
		
		public Action onUp;
		
		public Action onClick; 
		
		public Action onHold;

		public EButtonType buttonType = EButtonType.ClickButton;
		
		public HoldParameters hold = new HoldParameters();
		
		public RepeatParameters repeat = new RepeatParameters();
		
		public CancelParameters cancel = new CancelParameters();

		bool pressed;

		bool wasPressedOnDisable;

		float pressedTime;

		public float PressedTime
		{
			get
			{
				return pressedTime;
			}
		}

		public bool Pressed
		{
			get
			{
				return pressed;
			}
		}
		
		public void Toggle()
		{
			Toggle(!pressed);
		}
		
		public void Toggle(bool a_bPressed)
		{
			if(a_bPressed)
			{
				_Press();
			}
			else
			{
				_Release();
			}
		}
		
		public void Press()
		{
			switch(buttonType)
			{
				case EButtonType.ClickButton:
				{
					if(pressed == false)
					{
						_Press();
					}
				}
				break;
				
				case EButtonType.ToggleButton:
				{
					if(pressed)
					{
						_Release();
					}
					else
					{
						_Press();
					}
				}
				break;
			}
		}
		
		public void Release()
		{
			switch(buttonType)
			{
				case EButtonType.ClickButton:
				{
					if(pressed)
					{
						_Release();
					}
				}
				break;
				
				case EButtonType.ToggleButton:
				{
				}
				break;
			}
		}
		
		public void Cancel()
		{
			if(pressed)
			{
				_Cancel();
			}
		}

		void OnEnable()
		{
			if(onEnable != null)
				onEnable();
		}

		void OnDisable()
		{
			if(cancel.onDisable)
				Cancel();
		}

		void Update()
		{
			if(pressed)
			{
				pressedTime += Time.deltaTime;
				if(hold.Update())
				{
					Hold();
				}
				
				if(repeat.Update())
				{
					_Release();
					_Press();
				}
			}
		}
		
		void _Press()
		{	
			Down();
			NotifyStateChange(false);
		}
		
		void _Release()
		{
			Click();
			Up();
			NotifyStateChange(false);
		}
		
		void _Cancel()
		{
			Up();
			NotifyStateChange(true);
		}
		
		void Down()
		{	
			pressedTime = 0.0f;
			pressed = true;
			hold.OnDown();
			repeat.OnDown();
			NotifyDown();
			NotifyToggle(true);
		}
		
		void Up()
		{
			pressed = false;
			hold.OnUp();
			NotifyUp();
			NotifyToggle(false);
		}
		
		void Click()
		{
			if(buttonType != EButtonType.ToggleButton)
			{
				NotifyClick();
			}
		}
		
		void Hold()
		{
			if(cancel.onHold)
			{
				_Cancel();
			}
			NotifyHold();
		}
		
		void NotifyToggle(bool a_bPressed)
		{
			if(buttonType == EButtonType.ToggleButton)
			{
				if(onToggle != null)
				{
					onToggle(a_bPressed);
				}
			}
		}
		
		void NotifyStateChange(bool a_bCancel)
		{
			if(onStateChange != null)
			{
				onStateChange(this, a_bCancel);
			}
		}
		
		void NotifyDown()
		{
			if(onDown != null)
			{
				onDown();
			}
		}
		
		void NotifyUp()
		{
			if(onUp != null)
			{
				onUp();
			}
		}
		
		void NotifyClick()
		{
			if(onClick != null)
			{
				onClick();
			}
		}
		
		void NotifyHold()
		{
			if(onHold != null)
			{
				onHold();
			}
		}
	}
}