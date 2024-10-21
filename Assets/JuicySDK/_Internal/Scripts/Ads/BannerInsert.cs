using UnityEngine;
using UnityEngine.UI;
using Juicy;


namespace JuicyInternal {

    public class BannerInsert : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] RectTransform UiInsert;
        [SerializeField] RectTransform NoAdsButton;
        [SerializeField] RectTransform JuicyLogo;
        #pragma warning restore 0649 

        CanvasScaler canvasScaler;

        System.Action OnOrientationChanged;
        ScreenOrientation previousOrientation;

        static BannerInsert instance;
        public static BannerInsert Instance { get { return instance; } }

        float noAdsButtonRatio = .4f;

        void Awake()
        {
            if (JuicyRemoveAdsManager.AdsRemoved)
            {
                Destroy(gameObject);
                return;
            }

            if (instance != null)
            {
                if (instance != this)
                    Destroy(gameObject);

                return;
            }

            instance = this;

            canvasScaler = GetComponent<CanvasScaler>();
            NoAdsButton.GetComponent<Button>().onClick.AddListener(OnNoAdsClick);
            NoAdsButton.gameObject.SetActive(false);
            DisplayBannerInsert(false);

            JuicyRemoveAdsManager.onRemoveAds += OnRemoveAds;
        }

        // Adjust the banner insert to real banner size only in build
#if !UNITY_EDITOR
        void Start()
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            SetInsertHeight();
            OnOrientationChanged += SetInsertHeight;
        }

        private void Update()
        {
            if (Screen.orientation != previousOrientation)
            {
                previousOrientation = Screen.orientation;
                OnOrientationChanged?.Invoke();
            }
        }
#endif

        void OnDestroy()
        {
            instance = null;
            JuicyRemoveAdsManager.onRemoveAds -= OnRemoveAds;
        }

        void SetInsertHeight()
        {
            Vector2 bannerSize = ScreenUtility.GetBannerSize();
            float safeArea = ScreenUtility.GetSafeArea().yMin;

            //Set the insert height and position
            Vector2 insertSize = bannerSize + new Vector2(0, safeArea);
            UiInsert.sizeDelta = new Vector2(UiInsert.sizeDelta.x, insertSize.y);
            UiInsert.anchoredPosition = new Vector2(0, (insertSize.y / 2));

            //Set the button's size to 20% of the banner size
            NoAdsButton.sizeDelta = new Vector2(bannerSize.y * noAdsButtonRatio, bannerSize.y * noAdsButtonRatio);
            NoAdsButton.anchoredPosition = new Vector2(-bannerSize.y * noAdsButtonRatio/2, bannerSize.y * noAdsButtonRatio/2);

            //Set the Juicy's logo size and position
            JuicyLogo.sizeDelta = bannerSize;
            JuicyLogo.anchoredPosition = new Vector2(0, bannerSize.y / 2 + safeArea);
        }

        void OnRemoveAds()
        {
            Destroy(gameObject);
        }

        void OnNoAdsClick()
        {
            JuicySDK.BuyRemoveAds();
        }

        public void DisplayBannerInsert(bool on)
        {
            UiInsert.gameObject.SetActive(on);
        }
    }
}
