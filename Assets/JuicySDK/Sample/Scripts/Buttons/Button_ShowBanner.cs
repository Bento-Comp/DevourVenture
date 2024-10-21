using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_ShowBanner")]
	public class Button_ShowBanner : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.ShowBanner();
		}
	}
}