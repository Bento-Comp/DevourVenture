using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicyInternal
{
	[AddComponentMenu("JuicySDKInternal/PrivacyButtonBase")]
	public abstract class PrivacyButtonBase : MonoBehaviour
	{
		Button button;

		protected abstract void OnClick();

		void Awake()
		{
			button = GetComponent<Button>();
			
			if(button != null)
				button.onClick.AddListener(OnClick);
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);
		}
	}
}