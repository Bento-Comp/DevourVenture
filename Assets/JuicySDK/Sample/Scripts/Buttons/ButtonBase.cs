using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/ButtonBase")]
	public abstract class ButtonBase : MonoBehaviour
	{
		protected Button button;

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