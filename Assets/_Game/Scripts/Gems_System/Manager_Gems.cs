using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]      //must be after Gems_UI script
public class Manager_Gems : MonoBehaviour
{
    public static System.Action<int> OnUpdateGemsCount;
    public static System.Action<ShopItemSlot> OnPurchaseWithGemsCompleted;

    public const string m_gemsPlayerPrefsKey = "Gems";

    private int m_gemsCount;


    private void OnEnable()
    {
        Stand.OnStandRankUp += OnStandRankUp;
        ShopItemSlot.OnTryPurchasePackWithGems += OnTryPurchasePackWithGems;
        ShopItemSlot.OnGrantPurchasedContent += OnGrantPurchasedContent;
        ShopItemSlot.OnAskCurrentGemsCount+= OnAskCurrentGemsCount;
    }

    private void OnDisable()
    {
        Stand.OnStandRankUp -= OnStandRankUp;
        ShopItemSlot.OnTryPurchasePackWithGems -= OnTryPurchasePackWithGems;
        ShopItemSlot.OnGrantPurchasedContent -= OnGrantPurchasedContent;
        ShopItemSlot.OnAskCurrentGemsCount -= OnAskCurrentGemsCount;
    }


    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GainGems(50);
        }
#endif
    }

    private void OnAskCurrentGemsCount()
    {
        OnUpdateGemsCount?.Invoke(m_gemsCount);
    }

    private void OnGrantPurchasedContent(PurchasableItem_SO purchasableItemData)
    {
        for (int i = 0; i < purchasableItemData.m_purchasableItemList.Count; i++)
        {
            if (purchasableItemData.m_purchasableItemList[i].m_purchasableItemType == PurchasableItemType.Gems)
            {
                GainGems(purchasableItemData.m_purchasableItemList[i].m_amount);
                return;
            }
        }
    }

    private void OnTryPurchasePackWithGems(ShopItemSlot shopItemSlotReference, PurchasableItem_SO purchasableItemData)
    {
        if(purchasableItemData.m_currencyType == CurrencyType.Gems)
        {
            if (HasEnoughGems((int)purchasableItemData.m_price))
            {
                SpendGems((int)purchasableItemData.m_price);
                OnPurchaseWithGemsCompleted?.Invoke(shopItemSlotReference);
            }
        }
    }

    private void OnStandRankUp(bool isLoadingData)
    {
        if (isLoadingData)
            return;

        GainGems(1);
    }

    public bool HasEnoughGems(int amount)
    {
        return m_gemsCount >= amount;
    }

    private void GainGems(int amount)
    {
        if(amount <= 0)
        {
            Debug.Log("SFM_debug : Cannot gain négative amount of gems");
            return;
        }

        m_gemsCount += amount;

        SaveGems();
    }

    private void SpendGems(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("SFM_debug : Cannot spend négative amount of gems");
            return;
        }

        if (HasEnoughGems(amount) == false)
            return;

        m_gemsCount -= amount;

        SaveGems();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(m_gemsPlayerPrefsKey))
        {
            m_gemsCount= PlayerPrefs.GetInt(m_gemsPlayerPrefsKey);
        }
        else
        {
            m_gemsCount = 0;
            SaveGems();
        }
        OnUpdateGemsCount?.Invoke(m_gemsCount);
    }


    private void SaveGems()
    {
        PlayerPrefs.SetInt(m_gemsPlayerPrefsKey, m_gemsCount);

        OnUpdateGemsCount?.Invoke(m_gemsCount);
    }
}
