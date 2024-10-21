using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_SimulateCrash")]
	public class Button_SimulateCrash : ButtonBase
	{
		protected override void OnClick()
		{
            throw new System.Exception("Juicy SDK Sample Crash Test");
		}

		void Start()
		{
#if !debugJuicySDK
			Destroy(gameObject);
#endif
		}
	}
}
