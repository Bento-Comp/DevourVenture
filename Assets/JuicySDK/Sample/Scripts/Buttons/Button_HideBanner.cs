using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_HideBanner")]
	public class Button_HideBanner : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.HideBanner();
		}
	}
}