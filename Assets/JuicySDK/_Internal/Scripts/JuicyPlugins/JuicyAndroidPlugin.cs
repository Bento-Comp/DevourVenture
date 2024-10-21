#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juicy.Android
{
    public enum PopUpResponse
    {
        Undefined = -1,
        Dismiss = 0,
        Positive = 1,
        Negative = 2
    }

    public class JuicyAndroidPlugin
    {
        public static string GetGAID()
        {
            string advertisingID = "0000-0000-0000-0000";

            if (!GetLATStatus())
            {
                try
                {
                    AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                    AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

                    advertisingID = adInfo.Call<string>("getId").ToString();
                }

                catch
                {
                    Debug.LogError("JuicyANdroidPlugin : GetLATStatus : Can't access GAID");
                }
            }

            return advertisingID;
        }

        public static bool GetLATStatus()
        {
            bool limitTracking = true;

            try
            {
                AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

                limitTracking = (adInfo.Call<bool>("isLimitAdTrackingEnabled"));
            }

            catch
            {
                Debug.LogError("JuicyAndroidPlugin : GetLATStatus : Can't access lat status");
            }

            return limitTracking;
        }

        public static void ShowPopUp(string title, string message, string yes, string no, string goName, string goMethod)
        {
            AndroidJavaClass rating = new AndroidJavaClass("com.juicy.androidplugin.PopUp");
            rating.CallStatic("ShowDialogPopup",title, message, yes, no, goName, goMethod);
        }

        public static PopUpResponse GetPopUpResponse(string message)
        {
            int messageVal;
            if (!int.TryParse(message, out messageVal))
                return PopUpResponse.Undefined;
            

            switch (messageVal)
            {
                case 0:
                    return PopUpResponse.Dismiss;
                case 1:
                    return PopUpResponse.Positive;
                case 2:
                    return PopUpResponse.Negative;
                default:
                    return PopUpResponse.Undefined;
            }
        }
    }
}
#endif