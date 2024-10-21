using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackageContentSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private GameObject m_gemImage = null;

    [SerializeField]
    private GameObject m_standardLootboxImage = null;

    [SerializeField]
    private GameObject m_premiumLootboxImage = null;

    [SerializeField]
    private TMP_Text m_amountText = null;


    private void OnEnable()
    {
        ShopItemSlot.OnPackageContentSlotCreated += OnPackageContentSlotCreated;
    }

    private void OnDisable()
    {
        ShopItemSlot.OnPackageContentSlotCreated -= OnPackageContentSlotCreated;
    }


    private void OnPackageContentSlotCreated(GameObject packageContentSlot, PurchasableItem purchasableItem)
    {
        if (m_rootObject == packageContentSlot)
        {
            m_gemImage.SetActive(purchasableItem.m_purchasableItemType == PurchasableItemType.Gems);
            m_standardLootboxImage.SetActive(purchasableItem.m_purchasableItemType == PurchasableItemType.StandardLootbox);
            m_premiumLootboxImage.SetActive(purchasableItem.m_purchasableItemType == PurchasableItemType.PremiumLootbox);

            string amountText = purchasableItem.m_amount > 1000 ? (purchasableItem.m_amount / 1000f).ToString() + "K" : purchasableItem.m_amount.ToString();

            m_amountText.text = amountText;
        }
    }

}
