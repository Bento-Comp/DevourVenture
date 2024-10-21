using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_ShowRewarded")]
	public class Button_ShowRewarded : ButtonBase
	{
        string id = "Sample_ShowRewardedButton";

        private void Start()
        {
            OnRewardedAvailable(false);
            JuicySDK.AddRewardedAvailableListener(OnRewardedAvailable);
        }

        private void OnDestroy()
        {
            JuicySDK.RemoveRewardedAvailableListener(OnRewardedAvailable);
        }

        void OnRewardedAvailable(bool available)
        {
            if (available)
                JuicySDK.NotifyRewardedOpportunityStart(id);
            else
                JuicySDK.NotifyRewardedOpportunityEnd(id);

            button.interactable = available;
        }

        protected override void OnClick()
		{
            JuicySDK.ShowRewarded(id,OnRewardedEnd);
		}

		void OnRewardedEnd(bool success)
		{
			JuicyInternal.JuicySDKLog.Log("Button_ShowRewarded : OnRewardedEnd : success = " + success);
		}
	}
}