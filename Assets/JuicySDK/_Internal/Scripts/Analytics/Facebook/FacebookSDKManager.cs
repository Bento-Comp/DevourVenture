using UnityEngine;
using System.Collections;

using Juicy;
using System.Collections.Generic;

#if !noFacebook
using Facebook.Unity;
#endif

#if UNITY_IOS
using Juicy.IOS;
#endif

namespace JuicyInternal
{
	[DefaultExecutionOrder(-31998)]
	[AddComponentMenu("JuicySDKInternal/FacebookSDKManager")]
	public class FacebookSDKManager : MonoBehaviour
	{
#if !noFacebook

        bool initialized;

        public void Initialize()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                JuicySDKLog.Log("FacebookSDKManager : Awake : Init");
                FB.Init(InitCallback);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                JuicySDKLog.Log("FacebookSDKManager : Awake : ActivateApp : AppId = " + FB.AppId);
                FB.ActivateApp();
            }
        }

        public void UpdatePrivacySettings()
        {
            FB.Mobile.SetAdvertiserIDCollectionEnabled(JuicyPrivacyManager.IsAllowedToTrackData);
            FB.Mobile.SetAutoLogAppEventsEnabled(JuicyPrivacyManager.IsAllowedToTrackData);

            #if UNITY_IOS
            if (JuicyIOSPlugin.IsIOS14OrAbove())
            {
                bool adTrackingEnabled = !JuicyPrivacyManager.GetLATStatus() && JuicyPrivacyManager.IsAllowedToPersonalizedAds;
                FB.Mobile.SetAdvertiserTrackingEnabled(adTrackingEnabled);
            }
            #endif

			JuicySDKLog.Verbose("FacebookSDKManager : UpdatePrivacySettings : Auto log app events : " + JuicyPrivacyManager.IsAllowedToTrackData);
        }

        public void SendEvent(string eventName, List<EventProperty> eventProperties)
        {
            if(initialized == false)
                return;

            JuicySDKLog.Verbose(EventProperty.AddToString("FacebookSDKManager : SendEvent : eventName = " + eventName, eventProperties));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
			if(eventProperties != null)
			{
				foreach(EventProperty eventProperty in eventProperties)
				{
					parameters.Add(eventProperty.name, eventProperty.value);
				}
			}

            FB.LogAppEvent(eventName, null, parameters);
        }

		public void NotifyProductDelivery(ProductSummary productSummary)
		{
			JuicySDKLog.Verbose("FacebookSDKManager : NotifyProductDelivery : productId = " + productSummary.productId);

			var parameters = new Dictionary<string, object>();
			parameters[AppEventParameterName.NumItems] = 1;
			parameters[AppEventParameterName.ContentType] = productSummary.productType;
			parameters[AppEventParameterName.ContentID] = productSummary.productId;
			parameters[AppEventParameterName.Currency] = productSummary.isoCurrencyCode;

			FB.LogPurchase
			( 
				(float)productSummary.localizedPrice,
				productSummary.isoCurrencyCode,
				parameters
			);
		}

        private void InitCallback ()
		{
			if (FB.IsInitialized)
			{
				// Signal an app activation App Event
				JuicySDKLog.Log("FacebookSDKManager : InitCallback : ActivateApp : AppId = " + FB.AppId);
                UpdatePrivacySettings();
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
                initialized = true;
            }
			else
			{
				JuicySDKLog.Log("Failed to Initialize the Facebook SDK");
			}
		}
		#endif
	}
}