using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using Juicy;

using UnityEngine.Purchasing;
using AppsFlyerSDK;
using System.Text;

#if UNITY_IOS
using Juicy.IOS;
#endif

namespace JuicyInternal
{
    [DefaultExecutionOrder(-31998)]
    public class AppsFlyerManager : MonoBehaviour
    {
        private const string API_KEY = "MoRa9vBuJbP8oKfKH9spRj";
        private Dictionary<string, string> baseEventParameters = new Dictionary<string, string>();

        public void Initialize()
        {
            JuicySDKLog.Verbose("AppsFlyerManager : Initialize");
            baseEventParameters = GetBaseEventParameters();
            Connect();
        }

        public void UpdatePrivacySettings()
        {
            JuicySDKLog.Verbose("AppsFlyerManager : UpdatePrivacySettings");
            AppsFlyer.instance.anonymizeUser(!JuicyPrivacyManager.IsAllowedToTrackData);
        }

        public void SendMonetizationEvent(string eventName, List<EventProperty> parameters)
        {
            SendEvent(eventName, baseEventParameters);
        }

        public void SendGameStart(int level)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>(baseEventParameters);
            dict.Add("level_index", level.ToString());
            dict.Add("total_game_time", JuicySnapshot.TotalGameTime.ToString());
            dict.Add("session_count", JuicySnapshot.SessionCount.ToString());
            SendEvent("game_start", dict);
        }

        public void SendGameEnd(int level, bool success, float gameTime)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>(baseEventParameters);
            dict.Add("level_index", level.ToString());
            dict.Add("game_time", gameTime.ToString());
            dict.Add("success", success.ToString());
            dict.Add("total_game_time", JuicySnapshot.TotalGameTime.ToString());
            dict.Add("session_count", JuicySnapshot.SessionCount.ToString());
            SendEvent("game_end", dict);

            //if the app doesnt have "levels", take gameCount value
            int levelIndex = (level == -1) ? JuicySnapshot.GameCount : level;
            //Send unique index per level for appsflyer dashboard
            if (HaveToSendUniqueEvent(levelIndex))
            {
                string successString = success ? "success" : "fail";
                SendEvent($"game_end_game_{levelIndex}_{successString}", dict);
            }
        }

        Dictionary<string, string> GetBaseEventParameters()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ab_test_variant_index", JuicySDK.ABTestCohortVariantIndex.ToString());
            dict.Add("ab_test_name", JuicySDKSettings.Instance.AbTestName);
            dict.Add("juicysdk_version", JuicySDK.version);
            return dict;
        }

        private bool HaveToSendUniqueEvent(int levelIndex)
        {
            if(levelIndex <= 20)
                return true;

            if (levelIndex % 10 == 0 && levelIndex <= 100)
                return true;

            return false;
        }

        public void SendEvent(string eventName, Dictionary<string, string> dict_parameters)
        {
            JuicySDKLog.Verbose("AppsFlyerManager : SendEvent : eventName = " + eventName + " : " + dict_parameters.ToString());
            AppsFlyer.sendEvent(eventName, dict_parameters);
        }

        public void SendAdRevenue(string networkName, double revenue, string currency)
        {
            JuicySDKLog.Verbose("AppsFlyerManager : SendAdRevenue : " + networkName + " " + revenue + " " + currency);
            string abTestName = JuicySDKSettings.Instance.AbTestName;
            string abtestCohortIndex = JuicySDK.ABTestCohortVariantIndex.ToString();
            string sdkVersion = JuicySDK.version;

            AppsFlyerAdRevenue.logAdRevenue(networkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, revenue, currency, baseEventParameters);
        }


        public void NotifyProductDelivery(ProductSummary productSummary)
        {
            JuicySDKLog.Verbose("AppsFlyerManager : NotifyProductDelivery : productId = " + productSummary.productId);

            var price = productSummary.localizedPrice;
            double lPrice = decimal.ToDouble(price);
            var currencyCode = productSummary.isoCurrencyCode;

            var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(productSummary.receipt);
            if (null == wrapper)
            {
                return;
            }

            var payload = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt
            var productId = productSummary.productId;

#if UNITY_ANDROID

			var gpDetails = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
			var gpJson    = (string)gpDetails["json"];
			var gpSig     = (string)gpDetails["signature"];

			CompletedAndroidPurchase(productId, currencyCode, 1, lPrice, gpJson, gpSig);

#elif UNITY_IOS

			var transactionId = productSummary.transactionId;

			CompletedIosPurchase(productId, currencyCode, 1, lPrice , transactionId, payload);
#endif
        }

#if UNITY_IOS
        public void UpdateIOSConversionValue(int value)
        {
            //JuicyIOSPlugin.UpdateConversionValue(value);
            //instance.UpdateConversionValue(value);
        }
#endif

        void OnApplicationPause(bool pauseStatus)
        {
            if (AppsFlyer.instance.isInit == false)
                return;

            if (!pauseStatus)
            {
                Connect();
            }
        }

        void Connect()
        {
            if (AppsFlyer.instance != null)
                return;

            JuicySDKLog.Log("AppsFlyerManager : Connect : apiKey = " + API_KEY);
            string appStoreID = null;
#if UNITY_IOS
            appStoreID = JuicySDKSettings.Instance.BaseConfig.AppStoreId;
#endif
            AppsFlyer.OnRequestResponse += StartCallback;
            AppsFlyer.initSDK(API_KEY, appStoreID, this);
            AppsFlyer.setIsDebug(JuicySDKSettings.IsDebugMode);
            //string customerId = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(AppsFlyer.getAppsFlyerId())));
            //AppsFlyer.setCustomerUserId(customerId);
            AppsFlyer.startSDK();
            AppsFlyerAdRevenue.start();
        }

        void StartCallback(object sender, EventArgs e)
        {
            UpdatePrivacySettings();
        }

        void CompletedAndroidPurchase(string productId, string currencyCode, int quantity, double unitPrice, string receipt, string signature)
        {
            JuicySDKLog.Verbose("AppsFlyer : CompletedAndroidPurchase : productId = " + productId
                                       + " | currencyCode = " + currencyCode
                                       + " | quanty = " + quantity
                                       + " | unitPrice = " + unitPrice
                                       + " | signature = " + signature
                                       + " | receipt = " + receipt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(AFInAppEvents.CURRENCY, currencyCode);
            parameters.Add(AFInAppEvents.PRICE, (unitPrice * quantity).ToString());
            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, parameters);
        }

        void CompletedIosPurchase(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt)
        {
            JuicySDKLog.Verbose("AppsFlyer : CompletedIosPurchase : productId = " + productId
                           + " | currencyCode = " + currencyCode
                           + " | quanty = " + quantity
                           + " | unitPrice = " + unitPrice
                           + " | transactionId = " + transactionId
                           + " | receipt = " + receipt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(AFInAppEvents.CURRENCY, currencyCode);
            parameters.Add(AFInAppEvents.PRICE, (unitPrice * quantity).ToString());
            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, parameters);
        }

        public void onConversionDataSuccess(string conversionData)
        {
            AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
            Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
            // add deferred deeplink logic here
        }

        public void onConversionDataFail(string error)
        {
            AppsFlyer.AFLog("onConversionDataFail", error);
        }

        public void onAppOpenAttribution(string attributionData)
        {
            AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
        }

        public void onAppOpenAttributionFailure(string error)
        {
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }

    }
}