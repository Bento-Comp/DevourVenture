using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterEquipedEquipment : UniSingleton.Singleton<MainCharacterEquipedEquipment>
{
    public static System.Action<Equipment> OnEquipmentEquiped;
    public static System.Action<Equipment> OnEquipmentUnequiped;
    public static System.Action<Equipment, Equipment, Equipment> OnSendEquipedEquipment;
    public static System.Action<bool> OnSendEquipmentEquipedState;

    private Equipment m_headEquipment;
    private Equipment m_torsoEquipment;
    private Equipment m_toolEquipment;


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        EquipmentInventory.OnEquipEquipment += OnEquipEquipment;
        EquipmentInventory.OnUnequipEquipment += OnUnequipEquipment;

        Equipment_EquipedEquipmentUI.OnAskEquipedEquipment += SendEquipedEquipment;

        EquipmentDetail_UI.OnAskEquipmentEquipedState += OnAskEquipmentEquipedState;

        EquipmentInventory.Debug_OnInventoryCleared += Debug_OnInventoryCleared;

    }

    private void OnDisable()
    {
        EquipmentInventory.OnEquipEquipment -= OnEquipEquipment;
        EquipmentInventory.OnUnequipEquipment -= OnUnequipEquipment;

        Equipment_EquipedEquipmentUI.OnAskEquipedEquipment -= SendEquipedEquipment;

        EquipmentDetail_UI.OnAskEquipmentEquipedState -= OnAskEquipmentEquipedState;

        EquipmentInventory.Debug_OnInventoryCleared -= Debug_OnInventoryCleared;
    }

    private void Debug_OnInventoryCleared()
    {
        OnUnequipEquipment(m_headEquipment);
        OnUnequipEquipment(m_torsoEquipment);
        OnUnequipEquipment(m_toolEquipment);
    }

    private void OnAskEquipmentEquipedState(Equipment equipmentToTest)
    {
        bool isEquipmentEquiped = equipmentToTest.IsEquipped;


        OnSendEquipmentEquipedState?.Invoke(isEquipmentEquiped);
    }


    private void SendEquipedEquipment()
    {
        OnSendEquipedEquipment?.Invoke(m_headEquipment, m_torsoEquipment, m_toolEquipment);
    }

    private void OnEquipEquipment(Equipment equipmentToEquip)
    {
        if (equipmentToEquip == null)
            return;

        switch (equipmentToEquip.m_type)
        {
            case EquipmentType.Head:
                OnUnequipEquipment(m_headEquipment);
                m_headEquipment = equipmentToEquip;
                break;
            case EquipmentType.Torso:
                OnUnequipEquipment(m_torsoEquipment);
                m_torsoEquipment = equipmentToEquip;
                break;
            case EquipmentType.Tool:
                OnUnequipEquipment(m_toolEquipment);
                m_toolEquipment = equipmentToEquip;
                break;
            default:
                break;
        }

        //Debug.Log("equipped");
        equipmentToEquip.IsEquipped = true;

        OnEquipmentEquiped?.Invoke(equipmentToEquip);
    }

    private void OnUnequipEquipment(Equipment equipmentToUnequip)
    {
        if (equipmentToUnequip == null)
            return;

        equipmentToUnequip.IsEquipped = false;

        switch (equipmentToUnequip.m_type)
        {
            case EquipmentType.Head:
                if (m_headEquipment != null)
                    OnEquipmentUnequiped?.Invoke(m_headEquipment);
                break;
            case EquipmentType.Torso:
                if (m_torsoEquipment != null)
                    OnEquipmentUnequiped?.Invoke(m_torsoEquipment);
                break;
            case EquipmentType.Tool:
                if (m_toolEquipment != null)
                    OnEquipmentUnequiped?.Invoke(m_toolEquipment);
                break;
            default:
                break;
        }

        Debug.Log("unequipped");
    }


    public float GetEquipmentBonusValue(EquipmentEffect equipmentEffect)
    {
        float bonusValue = 0;

        if (m_headEquipment != null)
            bonusValue += m_headEquipment.m_effect == equipmentEffect ? m_headEquipment.m_effectValue : 0;

        if (m_torsoEquipment != null)
            bonusValue += m_torsoEquipment.m_effect == equipmentEffect ? m_torsoEquipment.m_effectValue : 0;

        if (m_toolEquipment != null)
            bonusValue += m_toolEquipment.m_effect == equipmentEffect ? m_toolEquipment.m_effectValue : 0;

        return bonusValue;
    }
}
