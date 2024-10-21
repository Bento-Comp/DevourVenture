using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropRatesInfos_UI : Game_UI
{

    [SerializeField]
    private EquipmentDropRate_SO m_standardLootboxDropRateData = null;

    [SerializeField]
    private EquipmentDropRate_SO m_premiumLootboxDropRateData = null;

    [SerializeField]
    private TMP_Text m_titleText = null;

    [SerializeField]
    private TMP_Text m_commonDropRate = null;

    [SerializeField]
    private TMP_Text m_rareDropRate = null;

    [SerializeField]
    private TMP_Text m_legendaryDropRate = null;

    private LootboxType m_lootboxType;


    protected override void OnEnable()
    {
        base.OnEnable();
        Button_LootboxInfo.OnSendLootboxType += OnSendLootboxType;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Button_LootboxInfo.OnSendLootboxType -= OnSendLootboxType;
    }


    private void Start()
    {
        ToggleUI(false);
    }


    private void OnSendLootboxType(LootboxType lootboxType)
    {
        m_lootboxType = lootboxType;
        ShowDropRatesInfos();
    }


    public void ShowDropRatesInfos()
    {
        OpenUI();

        if (m_lootboxType == LootboxType.Standard)
        {
            m_titleText.text = "Standard Lootboxe";
            m_commonDropRate.text = (m_standardLootboxDropRateData.GetDropRate(EquipmentRarity.Common) * 100f).ToString() + "%";
            m_rareDropRate.text = (m_standardLootboxDropRateData.GetDropRate(EquipmentRarity.Rare) * 100f).ToString() + "%";
            m_legendaryDropRate.text = (m_standardLootboxDropRateData.GetDropRate(EquipmentRarity.Legendary) * 100f).ToString() + "%";
        }
        else if (m_lootboxType == LootboxType.Premium)
        {
            m_titleText.text = "Premium Lootboxe";
            m_commonDropRate.text = (m_premiumLootboxDropRateData.GetDropRate(EquipmentRarity.Common) * 100f).ToString() + "%";
            m_rareDropRate.text = (m_premiumLootboxDropRateData.GetDropRate(EquipmentRarity.Rare) * 100f).ToString() + "%";
            m_legendaryDropRate.text = (m_premiumLootboxDropRateData.GetDropRate(EquipmentRarity.Legendary) * 100f).ToString() + "%";
        }
    }

    //called by buttons
    public void CloseDropRatesInfos()
    {
        CloseUI();
    }


}
