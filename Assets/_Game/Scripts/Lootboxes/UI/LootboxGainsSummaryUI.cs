using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootboxGainsSummaryUI : Game_UI
{
    public static System.Action<GameObject, Equipment, bool> OnEquipmentIconCreated;
    public static System.Action OnCloseLootboxGainsSummary;


    [SerializeField]
    private GameObject m_equipmentIconPrefab = null;

    [SerializeField]
    private Transform m_equipmentIconParent = null;

    [SerializeField]
    private Image m_oulineImage = null;

    [SerializeField]
    private EquipmentRarityInfo_SO m_equipmentRarityInfoData = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        EquipmentGenerator.OnEquipmentGeneratedFromLootbox += OnEquipmentGeneratedFromLootbox;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EquipmentGenerator.OnEquipmentGeneratedFromLootbox -= OnEquipmentGeneratedFromLootbox;
    }

    private void Start()
    {
        ToggleUI(false);
    }


    private void OnEquipmentGeneratedFromLootbox(List<Equipment> generatedEquipmentFromLootboxList, LootboxType lootboxType)
    {
        OpenUI();

        DestroyTransformChildren.DestroyAllTransformChildren(m_equipmentIconParent);

        if (lootboxType == LootboxType.Premium)
        {
            for (int i = 0; i < m_equipmentRarityInfoData.m_equipmentRarityInfoList.Count; i++)
            {
                if (m_equipmentRarityInfoData.m_equipmentRarityInfoList[i].m_rarity == EquipmentRarity.Legendary)
                {
                    m_oulineImage.color = m_equipmentRarityInfoData.m_equipmentRarityInfoList[i].m_rarityColor;
                }
            }
        }
        else
        {
            for (int i = 0; i < m_equipmentRarityInfoData.m_equipmentRarityInfoList.Count; i++)
            {
                if (m_equipmentRarityInfoData.m_equipmentRarityInfoList[i].m_rarity == EquipmentRarity.Common)
                {
                    m_oulineImage.color = m_equipmentRarityInfoData.m_equipmentRarityInfoList[i].m_rarityColor;
                }
            }
        }


        for (int i = 0; i < generatedEquipmentFromLootboxList.Count; i++)
        {
            GameObject equipmentIcon = Instantiate(m_equipmentIconPrefab, m_equipmentIconParent);

            Equipment_InventoryIcon equipment_InventoryIcon = equipmentIcon.GetComponent<Equipment_InventoryIcon>();

            equipment_InventoryIcon.InitializeInventoryIcon(generatedEquipmentFromLootboxList[i], false);

            OnEquipmentIconCreated?.Invoke(equipmentIcon, generatedEquipmentFromLootboxList[i], false);
        }
    }


    //called by button
    public void CloseLootboxGainsSummary()
    {
        CloseUI();
        OnCloseLootboxGainsSummary?.Invoke();
    }
}
