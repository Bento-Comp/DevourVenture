using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juicy;

namespace JuicyInternal {
	public class JuicyAnalyticsEmulationManager : MonoBehaviour
	{
        bool isInitialized = false;
        public  bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        public  void Initialize()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Analytics : Initialize");
            isInitialized = true;
        }

        public  void UpdatePrivacySettings()
        {
            JuicySDKLog.Verbose("Juicy Emulation : Analytics : Update Privacy Settings");
        }

        public  void SendEvent(string eventName, List<EventProperty> eventProperties)
        {
            string log = $"Juicy Emulation : Analytics : Send event : {eventName} ";
            foreach (EventProperty property in eventProperties)
                log += property.ToString() + "  ";
            JuicySDKLog.Verbose(log);
        }

        public void NotifyProductDelivery(ProductSummary productSummary)
        {
            JuicySDKLog.Verbose("Juicy Emulation : Analytics : Notify Product Delivery : " + productSummary.productId);
        }
    }
}
