using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/TransmitClickToGameScreenButtonWhenClickAgain")]
	public class TransmitClickToGameScreenButtonWhenClickAgain : MonoBehaviour
	{
		Toggle button;

		void Awake()
		{
			button = GetComponent<Toggle>();

			EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

			EventTrigger.Entry pointerDown = new EventTrigger.Entry();
			pointerDown.eventID = EventTriggerType.PointerDown;
			pointerDown.callback.AddListener(OnPointerDown);
			trigger.triggers.Add(pointerDown);
		}

		void OnPointerDown(BaseEventData eventData)
		{
			if(button.isOn)
			{
				OnClickAgain();
			}
		}

		void OnClickAgain()
		{
			GameScreenButton.Instance.TriggerDown();
		}
	}
}
