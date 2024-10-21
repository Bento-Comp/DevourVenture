using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/InputButtonMapping")]
	public abstract class InputButtonMapping : MonoBehaviour
	{	
		public bool repeat;

		public float repeatDelay = 0.01f;

		float repeatRemainingTime;

		bool triggerRepeat;

		public virtual bool GetButtonDown()
		{
			return _GetButtonDown() || triggerRepeat;
		}
		
		public virtual bool GetButton()
		{
			return false;
		}

		public virtual float GetAxisValue()
		{
			return 0.0f;
		}

		protected virtual bool _GetButtonDown()
		{
			return false;
		}

		protected virtual void Update()
		{
			triggerRepeat = false;
			if(repeat)
			{
				if(GetButton())
				{
					repeatRemainingTime -= Time.unscaledDeltaTime;
					if(repeatRemainingTime <= 0.0f)
					{
						repeatRemainingTime = repeatDelay;
						triggerRepeat = true;
					}
				}
				else
				{
					repeatRemainingTime = repeatDelay;
				}
			}
		}
	}
}