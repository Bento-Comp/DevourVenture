using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/uniButton_Debug_OnClick")]
	public class uniButton_Debug_OnClick : MonoBehaviour
	{
		Button button;
		
		void Awake()
		{
			button = GetComponent<Button>();
			button.onClick += OnEvent;
		}
		
		void OnDestroy()
		{
			button.onClick -= OnEvent;
		}
		
		void OnEvent()
		{
			Debug.Log("On Click : " + this);
		}
	}
}