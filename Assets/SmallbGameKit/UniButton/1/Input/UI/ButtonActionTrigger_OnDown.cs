using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/ButtonActionTrigger_OnDown")]
	public class ButtonActionTrigger_OnDown : MonoBehaviour
	{
		public Button button;

		public UnityEvent onEvent;

		void Awake()
		{
			button.onDown += OnEvent;
		}

		void OnDestroy()
		{
			button.onDown -= OnEvent;
		}

		void OnEvent()
		{
			onEvent.Invoke();
		}
	}
}