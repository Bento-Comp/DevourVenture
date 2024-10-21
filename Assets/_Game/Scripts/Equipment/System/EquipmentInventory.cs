using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public static System.Action<Equipment> OnNewEquipmentAddedToinventory;
    public static System.Action Debug_OnInventoryCleared;

    public static System.Action<Equipment> OnEquipEquipment;
    public static System.Action<Equipment> OnUnequipEquipment;

    private List<Equipment> m_headEquipmentList = new List<Equipment>();
    private List<Equipment> m_torsoEquipmentList = new List<Equipment>();
    private List<Equipment> m_toolEquipmentList = new List<Equipment>();


    private void OnEnable()
    {
        EquipmentGenerator.OnEquipmentGenerated += OnEquipmentGenerated;

        EquipmentDetail_UI.OnTryEquipEquipment += OnTryEquipEquipment;
        EquipmentDetail_UI.OnTryUnequipEquipment += OnTryUnequipEquipment;

        MainCharacterEquipedEquipment.OnEquipmentEquiped += OnEquipmentEquiped;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped += OnEquipmentUnequiped;
    }

    private void OnDisable()
    {
        EquipmentGenerator.OnEquipmentGenerated -= OnEquipmentGenerated;

        EquipmentDetail_UI.OnTryEquipEquipment -= OnTryEquipEquipment;
        EquipmentDetail_UI.OnTryUnequipEquipment -= OnTryUnequipEquipment;

        MainCharacterEquipedEquipment.OnEquipmentEquiped -= OnEquipmentEquiped;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped -= OnEquipmentUnequiped;
    }


    private void Start()
    {
        LoadEquipmentInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug_ClearInventory();
        }
    }

    private void Debug_ClearInventory()
    {
        m_headEquipmentList = new List<Equipment>();
        m_torsoEquipmentList = new List<Equipment>();
        m_toolEquipmentList = new List<Equipment>();
        Debug_OnInventoryCleared?.Invoke();

        SaveEquipmentInventory();
    }

    private void OnTryEquipEquipment(Equipment equipmentToEquip)
    {
        if (equipmentToEquip == null)
            return;

        if (IsEquipmentInInventory(equipmentToEquip))
            OnEquipEquipment?.Invoke(equipmentToEquip);
    }

    private void OnTryUnequipEquipment(Equipment equipmentToUnequip)
    {
        if (equipmentToUnequip == null)
            return;

        if (IsEquipmentInInventory(equipmentToUnequip))
            OnUnequipEquipment?.Invoke(equipmentToUnequip);
    }

    private void OnEquipmentEquiped(Equipment equippedEquipment)
    {
        SaveEquipmentInventory();
    }

    private void OnEquipmentUnequiped(Equipment unequippedEquipment)
    {
        SaveEquipmentInventory();
    }

    private bool IsEquipmentInInventory(Equipment equipment)
    {
        return m_headEquipmentList.Contains(equipment) || m_toolEquipmentList.Contains(equipment) || m_torsoEquipmentList.Contains(equipment);
    }


    private void OnEquipmentGenerated(Equipment generatedEquipment)
    {
        switch (generatedEquipment.m_type)
        {
            case EquipmentType.Head:
                m_headEquipmentList.Add(generatedEquipment);
                break;
            case EquipmentType.Torso:
                m_torsoEquipmentList.Add(generatedEquipment);
                break;
            case EquipmentType.Tool:
                m_toolEquipmentList.Add(generatedEquipment);
                break;
            default:
                break;
        }

        OnNewEquipmentAddedToinventory?.Invoke(generatedEquipment);

        SaveEquipmentInventory();
    }


    private void SaveEquipmentInventory()
    {
        SaveSystem_Equipment.SaveEquipments(m_headEquipmentList, m_torsoEquipmentList, m_toolEquipmentList);

        /*
        Debug.Log("try save");
        for (int i = 0; i < m_headEquipmentList.Count; i++)
            if (m_headEquipmentList[i].IsEquipped == true)
                Debug.Log("saved " + m_headEquipmentList[i].m_name);

        for (int j = 0; j < m_torsoEquipmentList.Count; j++)
            if (m_torsoEquipmentList[j].IsEquipped == true)
                Debug.Log("saved " + m_torsoEquipmentList[j].m_name);

        for (int k = 0; k < m_toolEquipmentList.Count; k++)
            if (m_toolEquipmentList[k].IsEquipped == true)
                Debug.Log("saved " + m_toolEquipmentList[k].m_name);

        Debug.Log("saved");
        */
    }

    private void LoadEquipmentInventory()
    {
        InventorySaveData loadedInventoryData = SaveSystem_Equipment.LoadEquipments();

        if (loadedInventoryData == null)
            return;

        m_headEquipmentList = new List<Equipment>(loadedInventoryData.m_headEquipmentList);
        m_torsoEquipmentList = new List<Equipment>(loadedInventoryData.m_torsoEquipmentList);
        m_toolEquipmentList = new List<Equipment>(loadedInventoryData.m_toolEquipmentList);

        InitializeLoadedEquipmentList(m_headEquipmentList);
        InitializeLoadedEquipmentList(m_torsoEquipmentList);
        InitializeLoadedEquipmentList(m_toolEquipmentList);

        EquipEquipmentFromSave(m_headEquipmentList);
        EquipEquipmentFromSave(m_torsoEquipmentList);
        EquipEquipmentFromSave(m_toolEquipmentList);
    }

    private void InitializeLoadedEquipmentList(List<Equipment> equipmentList)
    {
        for (int i = 0; i < equipmentList.Count; i++)
        {
            OnNewEquipmentAddedToinventory?.Invoke(equipmentList[i]);
        }
    }

    private void EquipEquipmentFromSave(List<Equipment> loadedEquipmentList)
    {
        for (int i = 0; i < loadedEquipmentList.Count; i++)
        {
            if (loadedEquipmentList[i].IsEquipped)
            {
                //Debug.Log("Equip " + loadedEquipmentList[i].m_name);
                OnEquipEquipment?.Invoke(loadedEquipmentList[i]);
            }
        }
    }
}
