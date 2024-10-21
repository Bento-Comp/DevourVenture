using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PurchasableItemType
{
    StandardLootbox,
    PremiumLootbox,
    Gems
}

public enum CurrencyType
{
    RealMoney,
    Gems
}


[System.Serializable]
public class PurchasableItem
{
    public PurchasableItemType m_purchasableItemType;
    public int m_amount;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Purchasable Item", order = 5)]
public class PurchasableItem_SO : ScriptableObject
{
    public List<PurchasableItem> m_purchasableItemList = null;
    public CurrencyType m_currencyType;
    public string m_productID = "";
    public string m_purchasableItemTitle = null;
    public float m_price = 0;
    public float m_fakeDiscount = 50;
    public bool m_hasFakeDiscount = false;
}
