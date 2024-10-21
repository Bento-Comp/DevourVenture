using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_ShowPrivacySettings")]
	public class Button_ShowPrivacySettings : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.ShowPrivacySettings();
		}

		void OnSelectToManagePrivacy(bool managePrivacy)
		{
			if(managePrivacy == false)
			{
				Destroy(gameObject);
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
	}
}