using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_NotifyInterstitialOpportunity")]
	public class Button_NotifyInterstitialOpportunity : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.NotifyInterstitialOpportunity();
		}
	}
}