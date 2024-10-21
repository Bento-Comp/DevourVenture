using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseLoadingScreen_UI : Game_UI
{
    [SerializeField]
    private float m_loadingScreenTimeOut = 5f;


    protected override void OnEnable()
    {
        base.OnEnable();
        ShopItemSlot.OnTryPurchasePackWithRealMoney += OnTryPurchasePackWithRealMoney;
        ShopItemSlot.OnGrantPurchasedContent+= OnGrantPurchasedContent;
        ShopItemSlot.OnCancelPurchaseWithRealMoney+= OnCancelPurchaseWithRealMoney;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ShopItemSlot.OnTryPurchasePackWithRealMoney -= OnTryPurchasePackWithRealMoney;
        ShopItemSlot.OnGrantPurchasedContent -= OnGrantPurchasedContent;
        ShopItemSlot.OnCancelPurchaseWithRealMoney -= OnCancelPurchaseWithRealMoney;
    }


    void Start()
    {
        CloseLoadingScreen();
    }


    private void OnTryPurchasePackWithRealMoney()
    {
        ToggleUI(true);
        Invoke("CloseLoadingScreen", m_loadingScreenTimeOut);
    }

    private void OnGrantPurchasedContent(PurchasableItem_SO purchasableItemData)
    {
        CloseLoadingScreen();
    }

    private void OnCancelPurchaseWithRealMoney()
    {
        CloseLoadingScreen();
    }

    private void CloseLoadingScreen()
    {
        ToggleUI(false);
    }
}
