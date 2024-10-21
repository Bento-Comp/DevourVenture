using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_RemoveAds")]
	public class Button_RemoveAds : ButtonBase
	{
		protected override void OnClick()
		{
			JuicySDK.BuyRemoveAds();
		}

		void Start()
		{
			JuicySDK.AddRemoveAdsListener(OnRemoveAds);
		}

		void OnDestroy()
		{
			JuicySDK.RemoveRemoveAdsListener(OnRemoveAds);
		}

		void OnRemoveAds()
		{
			Destroy(gameObject);
		}
	}
}