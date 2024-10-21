using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;

namespace JuicyInternal
{
	[AddComponentMenu("JuicySDKInternal/PrivacySettingsPopUp_ScreenController")]
	[ExecuteInEditMode()]
	public class PrivacySettingsPopUp_ScreenController : MonoBehaviour
	{
		public List<GameObject> screens;

		[SerializeField]
		int selectedScreen = 0;

		int SelectedScreen
		{
			set
			{
				selectedScreen = value;
				UpdateActivation();
			}
		}

		public void SelectScreen(int index)
		{
			SelectedScreen = index;
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;
			
			UpdateActivation();
		}
		#endif

		void UpdateActivation()
		{
			selectedScreen = Mathf.Clamp(selectedScreen, 0, screens.Count - 1);

			for(int i = 0; i < screens.Count; ++i)
			{
				screens[i].SetActive(i == selectedScreen);
			}
		}
	}
}
