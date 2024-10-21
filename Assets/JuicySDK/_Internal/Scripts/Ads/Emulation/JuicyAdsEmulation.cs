using UnityEngine;

namespace JuicyInternal
{
    public class JuicyAdsEmulation : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] Canvas ScreenSizeCanvas;
        [SerializeField] JuicyEmulatedBanner banner;
        [SerializeField] JuicyEmulatedInterstitial interstitial;
        [SerializeField] JuicyEmulatedRewarded rewarded;
        #pragma warning restore 0649

        static JuicyAdsEmulation instance;
        public static JuicyAdsEmulation Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                    Destroy(gameObject);

                return;
            }
            instance = this;
        }

        public JuicyEmulatedBanner CreateEmulatedBanner()
        {
            JuicyEmulatedBanner instanciatedBanner = Instantiate(banner, ScreenSizeCanvas.transform);
            SetBannerPosition(instanciatedBanner.gameObject.GetComponent<RectTransform>());
            return instanciatedBanner;
        }

        public JuicyEmulatedInterstitial CreateEmulatedInterstitial()
        {
            return Instantiate(interstitial, ScreenSizeCanvas.transform);
        }

        public JuicyEmulatedRewarded CreateEmulatedReawarded()
        {
            return Instantiate(rewarded, ScreenSizeCanvas.transform);
        }


        private void SetBannerPosition(RectTransform rectTransform)
        {
            Vector3 initialPos = rectTransform.anchoredPosition;
            BannerPosition bannerPos = Juicy.JuicySDK.Settings.BannerPosition;
            if(bannerPos != BannerPosition.Bottom)
            {
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.anchoredPosition = new Vector3(initialPos.x, -initialPos.y);
            }

        }
    }
}
