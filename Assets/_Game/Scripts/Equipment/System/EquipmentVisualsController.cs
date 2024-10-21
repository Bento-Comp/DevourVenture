using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentVisualsController : MonoBehaviour
{
    [SerializeField]
    private EquipmentVisual m_defaultShirt = null;

    [SerializeField]
    private List<EquipmentVisual> m_equipmentVisualList = null;


    private void Awake()
    {
        m_defaultShirt.EnableVisual();
    }

    private void OnEnable()
    {
        MainCharacterEquipedEquipment.OnEquipmentEquiped += OnEquipEquipment;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped += OnUnequipEquipment;
    }

    private void OnDisable()
    {
        MainCharacterEquipedEquipment.OnEquipmentEquiped -= OnEquipEquipment;
        MainCharacterEquipedEquipment.OnEquipmentUnequiped -= OnUnequipEquipment;
    }


    private void OnEquipEquipment(Equipment equipmentToEquip)
    {
        for (int i = 0; i < m_equipmentVisualList.Count; i++)
        {
            if (equipmentToEquip.m_equipmentID == m_equipmentVisualList[i].m_equipmentID)
            {
                if (m_equipmentVisualList[i].EquipmentType == EquipmentType.Torso)
                    m_defaultShirt.DisableVisual();

                m_equipmentVisualList[i].EnableVisual();
                return;
            }
        }
    }

    private void OnUnequipEquipment(Equipment equipmentToUnequip)
    {
        for (int i = 0; i < m_equipmentVisualList.Count; i++)
        {
            if (equipmentToUnequip.m_equipmentID == m_equipmentVisualList[i].m_equipmentID)
            {
                if (m_equipmentVisualList[i].EquipmentType == EquipmentType.Torso)
                    m_defaultShirt.EnableVisual();

                m_equipmentVisualList[i].DisableVisual();
                return;
            }
        }
    }


}
