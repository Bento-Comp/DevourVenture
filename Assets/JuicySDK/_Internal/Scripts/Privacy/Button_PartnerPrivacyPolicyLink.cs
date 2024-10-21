using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicyInternal
{
	[AddComponentMenu("JuicySDKInternal/Button_PartnerPrivacyPolicyLink")]
	[ExecuteInEditMode()]
	public class Button_PartnerPrivacyPolicyLink : PrivacyButtonBase
	{
		public string url;

		public Text textComponent;

		protected override void OnClick()
		{
			Application.OpenURL(url);
		}

		private void Start()
		{
			UpdateText();
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;
			
			UpdateText();
		}
		#endif

		void UpdateText()
		{
			if(textComponent == null)
				return;

			textComponent.text = url;
		}
	}
}