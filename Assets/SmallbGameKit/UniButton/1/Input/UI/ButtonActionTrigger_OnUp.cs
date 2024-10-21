using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/ButtonActionTrigger_OnUp")]
	public class ButtonActionTrigger_OnUp : MonoBehaviour
	{
		public Button button;
		
		public UnityEvent onEvent;
		
		void Awake()
		{
			button.onUp += OnEvent;
		}
		
		void OnDestroy()
		{
			button.onUp -= OnEvent;
		}
		
		void OnEvent()
		{
			onEvent.Invoke();
		}
	}
}