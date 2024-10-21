using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Juicy;

public class ShopItemSlot : MonoBehaviour
{
    public System.Action<PurchasableItem_SO> OnShopItemSlotInstantiated;
    public static System.Action<GameObject, PurchasableItem> OnPackageContentSlotCreated;
    public static System.Action<ShopItemSlot, PurchasableItem_SO> OnTryPurchasePackWithGems;
    public static System.Action OnTryPurchasePackWithRealMoney;
    public static System.Action OnCancelPurchaseWithRealMoney;
    public static System.Action<PurchasableItem_SO> OnGrantPurchasedContent;
    public static System.Action OnAskCurrentGemsCount;

    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private GameObject m_gemImage = null;

    [SerializeField]
    private GameObject m_standardLootboxImage = null;

    [SerializeField]
    private GameObject m_premiumLootboxImage = null;

    [SerializeField]
    private TMP_Text m_titleText = null;

    [SerializeField]
    private TMP_Text m_priceText = null;

    [SerializeField]
    private GameObject m_gemCurrencyImage = null;

    [SerializeField]
    private GameObject m_fakeDiscountObject = null;

    [SerializeField]
    private TMP_Text m_fakeDiscountText = null;

    [SerializeField]
    private GameObject m_packageContentSlotPrefab = null;

    [SerializeField]
    private Transform m_packageContentSlotParent = null;

    [SerializeField]
    private Image m_buttonOutline = null;

    [SerializeField]
    private Image m_buttonBackground = null;

    [SerializeField]
    private Color m_purchasableColor = Color.white;

    [SerializeField]
    private Color m_notPurchasableColor = Color.red;

    [SerializeField]
    private Color m_buttonDisabledColor = Color.white;

    [SerializeField]
    private Color m_buttonDisabledBackgroundColor = Color.grey;

    [SerializeField]
    private Color m_buttonEnabledColor = Color.green;

    [SerializeField]
    private Color m_buttonEnabledBackgroundColor = Color.green;

    private PurchasableItem_SO m_purchasableItemData;


    private void OnEnable()
    {
        ShopUI.OnShopItemSlotCreated += OnShopItemSlotCreated;
        Manager_Gems.OnPurchaseWithGemsCompleted += OnPurchaseCompleted;
        //ShopMenuButton.OnButtonPressed_DisplayShopMenuUI += OnButtonPressed_DisplayShopMenuUI;
        OnButtonPressed_DisplayShopMenuUI();

        Manager_Gems.OnUpdateGemsCount += OnUpdateGemsCount;
        OnUpdatePurchasableState();
    }

    private void OnDisable()
    {
        ShopUI.OnShopItemSlotCreated -= OnShopItemSlotCreated;
        Manager_Gems.OnPurchaseWithGemsCompleted -= OnPurchaseCompleted;
        //ShopMenuButton.OnButtonPressed_DisplayShopMenuUI -= OnButtonPressed_DisplayShopMenuUI;

        Manager_Gems.OnUpdateGemsCount -= OnUpdateGemsCount;
    }

    void Start()
    {
        JuicySDK.AddProductDeliveryListener(OnProductDelivery);
        JuicySDK.AddPurchaseFailedCallback(OnPurchaseFailed);
    }

    void OnDestroy()
    {
        JuicySDK.RemoveProductDeliveryListener(OnProductDelivery);
        JuicySDK.RemovePurchaseFailedCallback(OnPurchaseFailed);
    }

    private void OnButtonPressed_DisplayShopMenuUI()
    {
        Debug.Log("Valentin : On open shop UI");
        if (m_purchasableItemData != null)
        {
            Debug.Log("Valentin : On open shop UI SUCCESS");
            UpdatePriceText();
        }
    }

    private void OnUpdatePurchasableState()
    {
        OnAskCurrentGemsCount?.Invoke();
    }

    private void OnUpdateGemsCount(int amount)
    {
        if (m_purchasableItemData == null)
            return;

        if (m_purchasableItemData.m_currencyType == CurrencyType.Gems)
            UpdatePurchasableState(amount >= m_purchasableItemData.m_price);
    }


    private void UpdatePurchasableState(bool isPurchasable)
    {
        if (isPurchasable)
        {
            m_priceText.color = m_purchasableColor;
            m_buttonOutline.color = m_buttonEnabledColor;
            m_buttonBackground.color = m_buttonEnabledBackgroundColor;
        }
        else
        {
            m_priceText.color = m_notPurchasableColor;
            m_buttonOutline.color = m_buttonDisabledColor;
            m_buttonBackground.color = m_buttonDisabledBackgroundColor;
        }
    }


    private void OnShopItemSlotCreated(GameObject shopItemSlot, PurchasableItem_SO purchasableItemData)
    {
        if (m_rootObject == shopItemSlot)
        {
            m_purchasableItemData = purchasableItemData;

            Initialize();
        }
    }


    private void Initialize()
    {
        m_titleText.text = m_purchasableItemData.m_purchasableItemTitle;
        m_fakeDiscountObject.SetActive(m_purchasableItemData.m_hasFakeDiscount);
        m_fakeDiscountText.text = "-" + m_purchasableItemData.m_fakeDiscount.ToString() + "%";

        string costPrefixe = m_purchasableItemData.m_currencyType == CurrencyType.Gems ? "" : "$";
        UpdatePriceText();

        m_gemCurrencyImage.SetActive(m_purchasableItemData.m_currencyType == CurrencyType.Gems);


        m_gemImage.SetActive(false);
        m_standardLootboxImage.SetActive(false);
        m_premiumLootboxImage.SetActive(false);

        for (int i = 0; i < m_purchasableItemData.m_purchasableItemList.Count; i++)
        {
            switch (m_purchasableItemData.m_purchasableItemList[i].m_purchasableItemType)
            {
                case PurchasableItemType.StandardLootbox:
                    m_standardLootboxImage.SetActive(true);
                    break;
                case PurchasableItemType.PremiumLootbox:
                    m_premiumLootboxImage.SetActive(true);
                    break;
                case PurchasableItemType.Gems:
                    m_gemImage.SetActive(true);
                    break;
                default:
                    break;
            }

            GameObject instantiatedPackageContentSlot = Instantiate(m_packageContentSlotPrefab, m_packageContentSlotParent);
            OnPackageContentSlotCreated?.Invoke(instantiatedPackageContentSlot, m_purchasableItemData.m_purchasableItemList[i]);
        }

        OnShopItemSlotInstantiated?.Invoke(m_purchasableItemData);
    }

    private void UpdatePriceText()
    {
        Debug.Log("Valentin : start update price text");

        if (m_purchasableItemData.m_currencyType == CurrencyType.RealMoney)
        {
            List<Juicy.ProductSummary> productSummaryList = JuicySDK.GetProductSummaryList();
            Juicy.ProductSummary juicyProductSummary = null;

            if (productSummaryList == null)
            {
                Debug.LogError("List of product summary is null");
                Debug.Log("Valentin : list of product summary is null");
            }

            if(productSummaryList.Count <= 0)
            {
                Debug.Log("Valentin : list of product summary is empty");
            }

            for (int i = 0; i < productSummaryList.Count; i++)
            {
                if (productSummaryList[i].productId == m_purchasableItemData.m_productID)
                {
                    juicyProductSummary = productSummaryList[i];
                }
            }

            UniPurchase.ProductSummary productSummary = null;

            if (juicyProductSummary != null)
            {
                productSummary = ConvertJuicySDKProductToJuicyGameKitProduct(juicyProductSummary);
            }
            else
            {
                Debug.Log("Valentin : juicy product summary is null");
            }

            if (productSummary != null)
            {
                Debug.Log(productSummary.localizedPriceString);
                m_priceText.text = productSummary.localizedPriceString;
            }
            else
            {
                Debug.Log("Valentin : product summary is null");
                m_priceText.text = "_";
            }
        }
        else if (m_purchasableItemData.m_currencyType == CurrencyType.Gems)
        {
            m_priceText.text = m_purchasableItemData.m_price.ToString();
        }
    }

    private UniPurchase.ProductSummary ConvertJuicySDKProductToJuicyGameKitProduct(Juicy.ProductSummary sdkProduct)
    {
        UniPurchase.ProductSummary gameKitProduct = new UniPurchase.ProductSummary();

        gameKitProduct.productId = sdkProduct.productId;
        gameKitProduct.productType = (UniPurchase.ProductType)((int)sdkProduct.productType);
        gameKitProduct.localizedPriceString = sdkProduct.localizedPriceString;
        gameKitProduct.localizedPrice = sdkProduct.localizedPrice;
        gameKitProduct.receipt = sdkProduct.receipt;
        gameKitProduct.isoCurrencyCode = sdkProduct.isoCurrencyCode;

        return gameKitProduct;
    }

    private void OnPurchaseCompleted(ShopItemSlot shopItemSlot)
    {
        if (this == shopItemSlot)
        {
            OnGrantPurchasedContent?.Invoke(m_purchasableItemData);
        }
    }


    //called by button
    public void PurchasePack_ButtonPressed()
    {
        if (m_purchasableItemData.m_currencyType == CurrencyType.Gems)
            OnTryPurchasePackWithGems?.Invoke(this, m_purchasableItemData);

        if (m_purchasableItemData.m_currencyType == CurrencyType.RealMoney)
        {
            OnTryPurchasePackWithRealMoney?.Invoke();
            JuicySDK.BuyProduct(m_purchasableItemData.m_productID);
        }
    }


    void OnProductDelivery(ProductSummary productSummary)
    {
        if (productSummary.productId == m_purchasableItemData.m_productID)
        {
            OnPurchaseCompleted(this);
        }
    }

    private void OnPurchaseFailed()
    {
        OnCancelPurchaseWithRealMoney?.Invoke();
    }
}
