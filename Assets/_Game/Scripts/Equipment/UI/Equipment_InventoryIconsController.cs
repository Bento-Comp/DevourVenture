using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_InventoryIconsController : MonoBehaviour
{
    public static System.Action<GameObject, Equipment, bool> OnInventoryIconCreated;
    public static System.Action<EquipmentType> OnDisplaySpecificEquipmentType;

    [Header("Inventory")]
    [SerializeField]
    private GameObject m_equipmentInInventoryIconPrefab = null;

    [SerializeField]
    private Transform m_equipmentInInventoryParent = null;


    private List<Equipment_InventoryIcon> m_equipmentInventoryIconList = new List<Equipment_InventoryIcon>();
    private int m_currentOpenTabIndex;

    private void OnEnable()
    {
        Manager_EquipmentInventoryUI.OnEquipmentInventoryOpen += OnEquipmentInventoryOpen;
        EquipmentInventory.OnNewEquipmentAddedToinventory += OnNewEquipmentAddedToinventory;
        Equipment_InventoryIcon.OnEquipmentInventoryIconInitialized += OnEquipmentInventoryIconInitialized;

        EquipmentInventory.Debug_OnInventoryCleared += Debug_OnInventoryCleared;
    }

    private void OnDisable()
    {
        Manager_EquipmentInventoryUI.OnEquipmentInventoryOpen -= OnEquipmentInventoryOpen;
        EquipmentInventory.OnNewEquipmentAddedToinventory -= OnNewEquipmentAddedToinventory;
        Equipment_InventoryIcon.OnEquipmentInventoryIconInitialized -= OnEquipmentInventoryIconInitialized;

        EquipmentInventory.Debug_OnInventoryCleared -= Debug_OnInventoryCleared;
    }

    private void Debug_OnInventoryCleared()
    {
        DestroyTransformChildren.DestroyAllTransformChildren(m_equipmentInInventoryParent);
    }

    private void OnEquipmentInventoryIconInitialized(Equipment_InventoryIcon equipmentInventoryIcon)
    {
        if(m_equipmentInventoryIconList.Contains(equipmentInventoryIcon) == false)
        {
            m_equipmentInventoryIconList.Add(equipmentInventoryIcon);
        }
    }


    private void OnNewEquipmentAddedToinventory(Equipment newEquipment)
    {
        GameObject instantiatedEquipmentIcon = Instantiate(m_equipmentInInventoryIconPrefab, m_equipmentInInventoryParent);

        Equipment_InventoryIcon equipment_InventoryIcon = instantiatedEquipmentIcon.GetComponent<Equipment_InventoryIcon>();

        equipment_InventoryIcon.InitializeInventoryIcon(newEquipment, true);

        DisplayEquipmentType(m_currentOpenTabIndex);
    }


    private void OnEquipmentInventoryOpen()
    {
        DisplayEquipmentType(0);
    }


    //called by buttons
    public void DisplayEquipmentType(int equipmentType)
    {
        m_currentOpenTabIndex = equipmentType;

        for (int i = 0; i < m_equipmentInventoryIconList.Count; i++)
        {
            if(m_equipmentInventoryIconList[i].Equipment.m_type == (EquipmentType)equipmentType)
                m_equipmentInventoryIconList[i].gameObject.SetActive(true);
            else
                m_equipmentInventoryIconList[i].gameObject.SetActive(false);
        }

        OnDisplaySpecificEquipmentType?.Invoke((EquipmentType)equipmentType);
    }

}
