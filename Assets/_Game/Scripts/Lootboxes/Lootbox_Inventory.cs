using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootboxType
{
    Standard,
    Premium
}


public class Lootbox_Inventory : MonoBehaviour
{
    //int : standard lootbox count; int : premium lootbox count
    public static System.Action<int, int> OnUpdateLootboxCount;
    public static System.Action OnLootboxPurchased;
    public static System.Action<LootboxType> OnSendLootboxToOpen;
    public static System.Action<LootboxType> OnLootboxOpened;
    public static System.Action OnNoLootboxToOpen;


    public const string m_standardLootBoxKey = "Lootbox_Standard_InInventory";
    public const string m_premiumLootBoxKey = "Lootbox_Premium_InInventory";
    private int m_standardLootBoxCount;
    private int m_premiumLootBoxCount;


    private void OnEnable()
    {
        LootboxNotificationUI.OnShowOpeningLootboxUI += SendLootboxToOpen;
        OpenLootboxUI.OnTapOnLootboxToOpen += OnTapOnLootboxToOpen;
        LootboxGainsSummaryUI.OnCloseLootboxGainsSummary += OnCloseLootboxGainsSummary;
        LootboxNotificationUI.OnAskLootboxInInventoryCount += OnAskLootboxInInventoryCount;

        ShopItemSlot.OnGrantPurchasedContent += OnGrantPurchasedContent;
        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial += OnStartLootboxEquipmentTutorial;
    }

    private void OnDisable()
    {
        LootboxNotificationUI.OnShowOpeningLootboxUI -= SendLootboxToOpen;
        OpenLootboxUI.OnTapOnLootboxToOpen -= OnTapOnLootboxToOpen;
        LootboxGainsSummaryUI.OnCloseLootboxGainsSummary -= OnCloseLootboxGainsSummary;
        LootboxNotificationUI.OnAskLootboxInInventoryCount -= OnAskLootboxInInventoryCount;

        ShopItemSlot.OnGrantPurchasedContent -= OnGrantPurchasedContent;
        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial -= OnStartLootboxEquipmentTutorial;
    }

    private void Start()
    {
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GainLootBox(LootboxType.Standard, 1);
        if (Input.GetKeyDown(KeyCode.G))
            GainLootBox(LootboxType.Premium, 1);
    }


    private void OnStartLootboxEquipmentTutorial()
    {
        GainLootBox(LootboxType.Standard, 1);
    }

    private void OnGrantPurchasedContent(PurchasableItem_SO purchasableItemData)
    {
        for (int i = 0; i < purchasableItemData.m_purchasableItemList.Count; i++)
        {
            if (purchasableItemData.m_purchasableItemList[i].m_purchasableItemType == PurchasableItemType.StandardLootbox)
            {
                GainLootBox(LootboxType.Standard, purchasableItemData.m_purchasableItemList[i].m_amount);
            }
            else if (purchasableItemData.m_purchasableItemList[i].m_purchasableItemType == PurchasableItemType.PremiumLootbox)
            {
                GainLootBox(LootboxType.Premium, purchasableItemData.m_purchasableItemList[i].m_amount);
            }
        }

        if (purchasableItemData.m_purchasableItemList.Count > 0)
        {
            OnLootboxPurchased?.Invoke();
        }
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(m_standardLootBoxKey))
            m_standardLootBoxCount = PlayerPrefs.GetInt(m_standardLootBoxKey);

        if (PlayerPrefs.HasKey(m_premiumLootBoxKey))
            m_premiumLootBoxCount = PlayerPrefs.GetInt(m_premiumLootBoxKey);

        SaveData();

        OnUpdateLootboxCount?.Invoke(m_standardLootBoxCount, m_premiumLootBoxCount);
    }


    private void SaveData()
    {
        PlayerPrefs.SetInt(m_standardLootBoxKey, m_standardLootBoxCount);
        PlayerPrefs.SetInt(m_premiumLootBoxKey, m_premiumLootBoxCount);
    }

    private void OnAskLootboxInInventoryCount()
    {
        OnUpdateLootboxCount?.Invoke(m_standardLootBoxCount, m_premiumLootBoxCount);
    }

    private void SendLootboxToOpen()
    {
        if (m_premiumLootBoxCount > 0)
            OnSendLootboxToOpen?.Invoke(LootboxType.Premium);
        else if (m_standardLootBoxCount > 0)
            OnSendLootboxToOpen?.Invoke(LootboxType.Standard);
        else
            OnNoLootboxToOpen?.Invoke();
    }


    private void OnTapOnLootboxToOpen(LootboxType lootboxType)
    {
        switch (lootboxType)
        {
            case LootboxType.Standard:
                m_standardLootBoxCount--;
                OnLootboxOpened?.Invoke(LootboxType.Standard);
                break;
            case LootboxType.Premium:
                m_premiumLootBoxCount--;
                OnLootboxOpened?.Invoke(LootboxType.Premium);
                break;
            default:
                break;
        }

        SaveData();
        OnUpdateLootboxCount?.Invoke(m_standardLootBoxCount, m_premiumLootBoxCount);
    }


    private void OnCloseLootboxGainsSummary()
    {
        SendLootboxToOpen();
    }


    private void GainLootBox(LootboxType lootboxType, int count)
    {
        switch (lootboxType)
        {
            case LootboxType.Standard:
                m_standardLootBoxCount += count;
                break;
            case LootboxType.Premium:
                m_premiumLootBoxCount += count;
                break;
            default:
                break;
        }

        SaveData();
        OnUpdateLootboxCount?.Invoke(m_standardLootBoxCount, m_premiumLootBoxCount);
    }
}
