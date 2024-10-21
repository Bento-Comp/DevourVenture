#if UNITY_IOS
using UnityEngine;
using System.Runtime.InteropServices;

namespace Juicy.IOS {

    public class JuicyIOSPlugin
    {
        [DllImport("__Internal")]
        private static extern double _GetScreenPixelWidth();

        [DllImport("__Internal")]
        private static extern double _GetScreenPixelHeight();

        [DllImport("__Internal")]
        private static extern double _GetScreenPointWidth();

        [DllImport("__Internal")]
        private static extern double _GetScreenPointHeight();

        [DllImport("__Internal")]
        private static extern double _GetScreenScale();

        [DllImport("__Internal")]
        private static extern double _GetScreenNativeScale();

        [DllImport("__Internal")]
        private static extern string _GetDeviceModel();

        [DllImport("__Internal")]
        private static extern string _GetDeviceSystemVersion();

        [DllImport("__Internal")]
        private static extern double _GetScreenSafeAreaTop();

        [DllImport("__Internal")]
        private static extern double _GetScreenSafeAreaBottom();

        [DllImport("__Internal")]
        private static extern void _CallATTPopUp(string goName, string goMethod);

        [DllImport("__Internal")]
        private static extern int _GetATTStatus();

        [DllImport("__Internal")]
        private static extern bool _IsIOS14OrAbove();

        [DllImport("__Internal")]
        private static extern bool _IsIOS14Dot5OrAbove();

        [DllImport("__Internal")]
        private static extern string _GetIDFA();

        [DllImport("__Internal")]
        private static extern void _ShowRatingPopUp();

        [DllImport("__Internal")]
        private static extern void _UpdateConversionValue(int conversionValue);

        #region DeviceInfo
        public static Vector2 GetScreenPixelSize()
        {
            if (!Application.isEditor)
                return new Vector2((float)_GetScreenPixelWidth(), (float)_GetScreenPixelHeight());

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return Vector2.zero;
        }

        public static Vector2 GetScreenPointSize()
        {
            if (!Application.isEditor)
                return new Vector2((float)_GetScreenPointWidth(), (float)_GetScreenPointHeight());

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return Vector2.zero;
        }

        public static Vector2 GetScreenSafeArea()
        {
            if (!Application.isEditor)
                return new Vector2((float)_GetScreenSafeAreaTop(), (float)_GetScreenSafeAreaBottom());

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return Vector2.zero;
        }

        public static float GetScreenScale()
        {
            if (!Application.isEditor)
                return (float)_GetScreenScale();

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return 1;
        }

        public static float GetScreenNativeScale()
        {
            if (!Application.isEditor)
                return (float)_GetScreenNativeScale();

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return 1;
        }

        public static string GetDeviceModel()
        {
            if (!Application.isEditor)
                return _GetDeviceModel();

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return "";
        }

        public static string GetDeviceSystemVersion()
        {
            if (!Application.isEditor)
                return _GetDeviceSystemVersion();

            Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
            return "";
        }
        #endregion //DeviceInfo
        #region Privacy

        public static void CallATTPopUp(string goName, string goMethod)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
                return;
            }

            try
            {
                _CallATTPopUp(goName, goMethod);
               
            }

            catch
            {
                Debug.LogError("JuicyIOSPlugin : CallATTPopUp : UnExpected error");
            }
        }

        public static int GetATTStatus()
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
                return -1;
            }

            int trackingStatus = -1;
            try
            {
                trackingStatus = _GetATTStatus();
            }

            catch
            {
                trackingStatus = -1;
                Debug.LogError("JuicyIOSPlugin : GetATTStatus : Unexpected error");
            }

            return trackingStatus;
        }

        public static bool IsIOS14OrAbove()
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
                return false;
            }

            bool ios14 = true;
            try
            {
                ios14 = _IsIOS14OrAbove();
            }

            catch
            {
                ios14 = false;
                Debug.LogError("JuicyIOSPlugin : IsIOS14OrAbove : Unexpected error");
            }

            return ios14;
        }

        public static bool IsIOS14Dot5OrAbove()
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
                return false;
            }

            bool ios14 = true;
            try
            {
                ios14 = _IsIOS14Dot5OrAbove();
            }

            catch
            {
                ios14 = false;
                Debug.LogError("JuicyIOSPlugin : IsIOS14Dot5OrAbove : Unexpected error");
            }

            return ios14;
        }

        public static string GetIDFA()
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("JuicyIOSPlugin : Doesn't work in Editor");
                return "0000-0000-0000-0000";
            }

            string adid = "0000-0000-0000-0000";

            try
            {
                adid = _GetIDFA();
            }

            catch
            {
                adid = "0000-0000-0000-0000";
                Debug.LogError("JuicyIOSPlugin : GetIDFA : Unexpected error");
            }

            return adid;
        }
        #endregion //Privacy
        #region Rating
        public static void ShowRatingPopUp()
        {
            _ShowRatingPopUp();
        }
        #endregion //Rating
        #region ConversionValue
        public static void UpdateConversionValue(int conversionValue)
        {
            _UpdateConversionValue(conversionValue);
        }
        #endregion
    }
}
#endif