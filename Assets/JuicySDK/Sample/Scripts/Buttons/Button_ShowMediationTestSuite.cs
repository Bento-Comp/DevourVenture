using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_ShowMediationTestSuite")]
	public class Button_ShowMediationTestSuite : ButtonBase
	{
		protected override void OnClick()
		{
			JuicyInternal.JuicyAdsManager.Instance.ShowMediationTestSuite();
        }

        private void Start()
        {
#if !debugJuicySDK
			Destroy(this.gameObject);
#endif
		}
    }
}