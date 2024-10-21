using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_RestorePurchases")]
	public class Button_RestorePurchases : ButtonBase
	{

		protected override void OnClick()
		{
			JuicySDK.RestorePurchases();
		}

		void Start()
		{
			if(JuicySDK.RestorePurchasesSupported == false)
			{
				Destroy(gameObject);
			}
		}
	}
}