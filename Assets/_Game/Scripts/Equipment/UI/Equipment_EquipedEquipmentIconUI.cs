using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_EquipedEquipmentIconUI : MonoBehaviour
{
    [SerializeField]
    private EquipmentType m_type;

    [SerializeField]
    private Image m_icon = null;

    [SerializeField]
    private Image m_outline = null;

    [SerializeField]
    private EquipmentRarityInfo_SO m_equipmentRarityInfoData = null;


    private void Awake()
    {
        m_icon.sprite = null;
        Color rarityColor = m_equipmentRarityInfoData.GetDefaultColor();
        m_outline.color = rarityColor;
    }

    private void OnEnable()
    {
        EquipmentInventory.OnEquipEquipment += OnEquipEquipment;
        EquipmentInventory.OnUnequipEquipment += OnUnequipEquipment;
        EquipmentInventory.Debug_OnInventoryCleared += Debug_OnInventoryCleared;
    }

    private void OnDisable()
    {
        EquipmentInventory.OnEquipEquipment -= OnEquipEquipment;
        EquipmentInventory.OnUnequipEquipment -= OnUnequipEquipment;
        EquipmentInventory.Debug_OnInventoryCleared -= Debug_OnInventoryCleared;
    }

    private void Debug_OnInventoryCleared()
    {
        m_icon.sprite = null;
        m_outline.color = m_equipmentRarityInfoData.m_nullEquipmentRarityInfo.m_rarityColor;
    }

    private void OnEquipEquipment(Equipment equipment)
    {
        if (m_type != equipment.m_type)
            return;

        if (EquipmentGenerator.Instance != null)
            m_icon.sprite = EquipmentGenerator.Instance.GetEquipmentSprite(equipment);

        Color rarityColor = m_equipmentRarityInfoData.GetColor(equipment.m_rarity);
        m_outline.color = rarityColor;
    }

    private void OnUnequipEquipment(Equipment equipment)
    {
        if (m_type != equipment.m_type)
            return;

        m_icon.sprite = null;
        m_outline.color = m_equipmentRarityInfoData.m_nullEquipmentRarityInfo.m_rarityColor;
    }

}
