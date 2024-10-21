using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Juicy;

#if !noFirebaseAnalytics
using Firebase.Analytics;
#endif

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31998)]
    [AddComponentMenu("JuicySDKInternal/FirebaseAnalyticsManager")]
    public class FirebaseAnalyticsManager : MonoBehaviour
    {
#if !noFirebaseAnalytics

        bool initialised = false;
        List<JuicyStoredEvent> storedEvents = new List<JuicyStoredEvent>();

        public void Initialize()
        {
            JuicySDKLog.Log("FirebaseAnalyticsManager : Start");
            FirebaseManager.AddOnInitializationComplete(OnInitialisationComplete);
        }

        void OnInitialisationComplete(bool success)
        {
            JuicySDKLog.Log("FirebaseAnalyticsManager : OnInitialisationComplete : success = " + success);

            if (!success)
                return;

            UpdatePrivacySettings();
            initialised = true;
            SendStoredEvents();
        }

        public void UpdatePrivacySettings()
        {
            JuicySDKLog.Verbose("FirebaseAnalyticsManager : UpdatePrivacySettings : Analytics collection enabled : " + JuicyPrivacyManager.IsAllowedToTrackData);
            FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertyAllowAdPersonalizationSignals, JuicyPrivacyManager.IsAllowedToPersonalizedAds.ToString());
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        }

        public void SendEvent(string eventName, List<EventProperty> eventProperties)
        {
            if (initialised == false)
            {
                if (storedEvents.Count < 10)
                    storedEvents.Add(new JuicyStoredEvent(eventName, eventProperties));
                return;
            }

            JuicySDKLog.Verbose(EventProperty.AddToString("FirebaseAnalyticsManager : SendEvent : eventName = " + eventName, eventProperties));

            Parameter[] parameters = null;
            if (eventProperties != null && eventProperties.Count > 0)
            {
                parameters = new Parameter[eventProperties.Count];
                for (int i = 0; i < parameters.Length; ++i)
                {
                    EventProperty eventProperty = eventProperties[i];
                    object value = eventProperty.value;
                    parameters[i] = new Parameter(eventProperty.name, value.ToString());
                }
            }

            if (parameters == null)
            {
                FirebaseAnalytics.LogEvent(eventName);
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName, parameters);
            }
        }

        public void SendAdImpressionEvent(string adPlatform, string networkName, string adUnitIdentifier, string adFormat, double revenue, string currency)
        {
            var impressionParameters = new[] {
            new Parameter("ad_platform", adPlatform),
            new Parameter("ad_source", networkName),
            new Parameter("ad_unit_name", adUnitIdentifier),
            new Parameter("ad_format", adFormat),
            new Parameter("value", revenue),
            new Parameter("currency", currency)
            };
            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        }

        void SendStoredEvents()
        {
            JuicySDKLog.Verbose("AnalyticsManagerBase : this = " + this + " : SendStoredEvents : " + storedEvents.Count + " events sent");
            foreach (JuicyStoredEvent storedEvent in storedEvents)
                SendEvent(storedEvent.name, storedEvent.properties);
            storedEvents.Clear();
        }
#endif
    }
}
