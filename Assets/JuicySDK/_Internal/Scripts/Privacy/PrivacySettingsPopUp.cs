using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

using Juicy;

namespace JuicyInternal
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("JuicySDKInternal/PrivacySettingsPopUp")]
	public class PrivacySettingsPopUp : MonoBehaviour
	{
        #pragma warning disable 0649
        [SerializeField] protected PrivacySettingsPopUp_ScreenController screenController = null;
        #pragma warning restore 0649

        public virtual void Open()
		{
			screenController.SelectScreen(0);
			SetActive(true);
		}

        public virtual void Close()
		{
			SetActive(false);
		}

		public virtual void Awake()
		{
			//SetActive(false);
		}

        public void OpenPage(int index)
        {
			screenController.SelectScreen(index);
        }

		void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}
