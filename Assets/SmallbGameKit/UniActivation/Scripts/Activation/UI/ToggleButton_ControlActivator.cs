using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/ToggleButton_ControlActivator")]
	public class ToggleButton_ControlActivator : MonoBehaviour
	{
		public Activator activator;

		Toggle button;

		void Awake()
		{
			button = GetComponent<Toggle>();
			if(Application.isPlaying)
			{
				button.onValueChanged.AddListener(OnValueChange);
			}
			else
			{
				if(activator == null)
					return;
			}

			OnValueChange(button.isOn);
		}

		void OnDestroy()
		{
			if(Application.isPlaying)
			{
				if(button != null)
					button.onValueChanged.RemoveListener(OnValueChange);
			}
		}

		#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(activator == null)
				return;
			
			if(button == null)
				button = GetComponent<Toggle>();

			if(button == null)
				return;

			OnValueChange(button.isOn);
		}
		#endif

		void OnValueChange(bool value)
		{
			activator.SelectedIndex = value ? 1 : 0;
		}
	}
}