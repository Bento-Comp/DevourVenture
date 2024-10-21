using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_LootboxInfo : Game_UI
{
    public static System.Action<LootboxType> OnSendLootboxType;

    [SerializeField]
    private ShopItemSlot m_shopItemSlotReference = null;

    private LootboxType m_lootboxType;


    protected override void OnEnable()
    {
        base.OnEnable();
        m_shopItemSlotReference.OnShopItemSlotInstantiated += OnShopItemSlotInstantiated;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_shopItemSlotReference.OnShopItemSlotInstantiated -= OnShopItemSlotInstantiated;
    }


    private void OnShopItemSlotInstantiated(PurchasableItem_SO purchasableItemData)
    {
        ToggleUI(false);

        if (purchasableItemData.m_purchasableItemList.Count == 1)
        {
            if (purchasableItemData.m_purchasableItemList[0].m_purchasableItemType == PurchasableItemType.StandardLootbox)
            {
                m_lootboxType = LootboxType.Standard;
                ToggleUI(true);
            }
            else if(purchasableItemData.m_purchasableItemList[0].m_purchasableItemType == PurchasableItemType.PremiumLootbox)
            {
                m_lootboxType = LootboxType.Premium;
                ToggleUI(true);
            }
        }
    }

    //called by button
    public void ButtonPressed_ShowDropRatesInfo()
    {
        OnSendLootboxType?.Invoke(m_lootboxType);
    }
}
