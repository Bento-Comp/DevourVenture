using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : Game_UI
{
    public static System.Action<GameObject, PurchasableItem_SO> OnShopItemSlotCreated;

    [SerializeField]
    private List<PurchasableItem_SO> m_purchasableItemDataList = null;

    [SerializeField]
    private GameObject m_shopItemSlotPrefab = null;

    [SerializeField]
    private Transform m_packageShopItemParent = null;

    [SerializeField]
    private Transform m_lootboxShopItemParent = null;

    [SerializeField]
    private Transform m_gemsShopItemParent = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        ShopMenuButton.OnButtonPressed_DisplayShopMenuUI += OnButtonPressed_DisplayShopMenuUI;
        ShopItemSlot.OnGrantPurchasedContent += OnGrantPurchasedContent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ShopMenuButton.OnButtonPressed_DisplayShopMenuUI -= OnButtonPressed_DisplayShopMenuUI;
        ShopItemSlot.OnGrantPurchasedContent -= OnGrantPurchasedContent;
    }

    private void Start()
    {
        Initialize();
        ToggleUI(false);
    }


    private void Initialize()
    {
        for (int i = 0; i < m_purchasableItemDataList.Count; i++)
        {
            Transform shopItemParent = m_packageShopItemParent;

            if (m_purchasableItemDataList[i].m_purchasableItemList.Count > 1)
                shopItemParent = m_packageShopItemParent;

            if (m_purchasableItemDataList[i].m_purchasableItemList.Count == 1)
                shopItemParent = m_purchasableItemDataList[i].m_purchasableItemList[0].m_purchasableItemType == PurchasableItemType.Gems ? m_gemsShopItemParent : m_lootboxShopItemParent;

            GameObject instantiatedShopItemSlot = Instantiate(m_shopItemSlotPrefab, shopItemParent);
            OnShopItemSlotCreated?.Invoke(instantiatedShopItemSlot, m_purchasableItemDataList[i]);
        }
    }

    private void OnGrantPurchasedContent(PurchasableItem_SO purchasableItemData)
    {
        CloseShopUI();
    }

    private void OnButtonPressed_DisplayShopMenuUI()
    {
        if (IsUIOpen)
            return;

        OpenUI();
    }


    //called by button
    public void CloseShopUI()
    {
        CloseUI();
    }



}
