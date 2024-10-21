using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipment_InventoryIcon : MonoBehaviour
{
    public static System.Action<Equipment_InventoryIcon> OnEquipmentInventoryIconInitialized;
    public static System.Action<Equipment> OnSendSelectedEquipmentInfo;


    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private Image m_equipmentIcon = null;

    [SerializeField]
    private Image m_outline = null;

    [SerializeField]
    private TMP_Text m_equipmentRarityText = null;

    [SerializeField]
    private Image m_equipmentRarityTextBackground = null;

    [SerializeField]
    private EquipmentRarityInfo_SO m_equipmentRarityInfoData = null;

    [SerializeField]
    private GameObject m_equippedIndicator = null;


    private Equipment m_equipment;
    private bool m_isInventoryIcon;

    public Equipment Equipment { get => m_equipment; }


    private void OnEnable()
    {
        MainCharacterEquipedEquipment.OnEquipmentEquiped += OnEquipEquipment;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped += OnUnequipEquipment;
        EquipmentInventory.Debug_OnInventoryCleared += Debug_OnInventoryCleared;

        CheckEquipmentEquippedState();
    }

    private void OnDisable()
    {
        MainCharacterEquipedEquipment.OnEquipmentEquiped -= OnEquipEquipment;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped -= OnUnequipEquipment;
        EquipmentInventory.Debug_OnInventoryCleared -= Debug_OnInventoryCleared;
    }

    private void Debug_OnInventoryCleared()
    {
        Destroy(m_rootObject);
    }

    private void CheckEquipmentEquippedState()
    {
        if (m_equipment == null)
            return;

        m_equippedIndicator.SetActive(m_equipment.IsEquipped);
    }

    private void OnEquipEquipment(Equipment equipment)
    {
        if (m_equipment != equipment)
            return;

        m_equippedIndicator.SetActive(true);
    }

    private void OnUnequipEquipment(Equipment equipment)
    {
        if (m_equipment != equipment)
            return;

        m_equippedIndicator.SetActive(false);
    }


    public void InitializeInventoryIcon(Equipment equipment, bool isInventoryIcon)
    {
        m_equipment = equipment;

        if (EquipmentGenerator.Instance != null)
            m_equipmentIcon.sprite = EquipmentGenerator.Instance.GetEquipmentSprite(equipment);

        m_equipmentRarityText.text = equipment.m_rarity.ToString();

        Color rarityColor = m_equipmentRarityInfoData.GetColor(equipment.m_rarity);
        m_outline.color = rarityColor;
        m_equipmentRarityTextBackground.color = rarityColor;

        m_isInventoryIcon = isInventoryIcon;

        m_equippedIndicator.SetActive(false);

        //If is inventory icon : the icon is added to a list managed in the inventory to be displayed
        //Else it is just an icon to see the item gained in a lootbox so it doesn't go to the list of icons.
        if (isInventoryIcon)
            OnEquipmentInventoryIconInitialized?.Invoke(this);
    }

    //called by button
    public void OnSelectEquipment_ButtonPressed()
    {
        if (m_isInventoryIcon)
            OnSendSelectedEquipmentInfo?.Invoke(m_equipment);
    }
}
