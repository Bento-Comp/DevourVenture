using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/ControlUIButtonWithUniButton")]
	public class ControlUIButtonWithUniButton : MonoBehaviour
	{
		public Button button;

		public UnityEngine.UI.Button uiButton;

		PointerEventData eventData;

		PointerEventData EventData
		{
			get
			{
				if(eventData == null)
					eventData = new PointerEventData(EventSystem.current);

				return eventData;
			}
		}

		void Awake()
		{
			button.onDown += OnDown;
			button.onUp += OnUp;
			button.onClick += OnClick;
		}

		void OnDestroy()
		{
			button.onDown -= OnDown;
			button.onUp -= OnUp;
			button.onClick -= OnClick;
		}


		void OnDown()
		{
			PointerEventData eventData = EventData;
			uiButton.OnPointerEnter(eventData);
			uiButton.OnPointerDown(eventData);
		}

		void OnUp()
		{
			uiButton.OnPointerUp(EventData);
		}

		void OnClick()
		{
			uiButton.OnPointerClick(EventData);
		}
	}
}