using UnityEngine;

#if UNITY_IOS
using Juicy.IOS;
#endif

namespace JuicyInternal
{
    //Use this class in build only
    public class ScreenUtility : MonoBehaviour
    {
        const float DEFAULT_DPI = 300;

        public static Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        public static float GetScreenDpi()
        {
            return Screen.dpi > 0 ? Screen.dpi : DEFAULT_DPI;
        }

        public static float PixelToDp(float pixel)
        {
#if UNITY_IOS
            return pixel / JuicyIOSPlugin.GetScreenNativeScale();
#else
            return pixel / (GetScreenDpi() / 160);
#endif
        }

        public static float DpToPixel(float dp)
        {
#if UNITY_IOS
            return dp * JuicyIOSPlugin.GetScreenNativeScale();
#else
            return dp * (GetScreenDpi() / 160);
#endif
        }

        public static bool IsTablet()
        {
            float screenWidth = Screen.width / GetScreenDpi();
            float screenHeight = Screen.height / GetScreenDpi();
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            return diagonalInches > 8f;
        }

        public static Vector2 GetBannerSize()
        {
            if (IsTablet())
                return new Vector2(DpToPixel(728), DpToPixel(90));
            else
                return new Vector2(DpToPixel(320), DpToPixel(50));
        }

        public static Vector2 GetBannerPosition(Vector2 bannerSize)
        {
            Vector2 position = new Vector2(0, (bannerSize.y/2) + GetSafeArea().yMin);
            return position;
        }
    }
}
