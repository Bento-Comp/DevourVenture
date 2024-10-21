using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTabButton_UI : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private EquipmentType m_correspondingTabEquipmentType;


    private void OnEnable()
    {
        Equipment_InventoryIconsController.OnDisplaySpecificEquipmentType += OnDisplaySpecificEquipmentType;
    }

    private void OnDisable()
    {
        Equipment_InventoryIconsController.OnDisplaySpecificEquipmentType -= OnDisplaySpecificEquipmentType;
    }

    private void OnDisplaySpecificEquipmentType(EquipmentType equipmentType)
    {
        if (m_correspondingTabEquipmentType == equipmentType)
            PlaySelected();
        else
            PlayDefault();
    }

    private void PlaySelected()
    {
        m_animator.SetBool("IsSelected", true);
    }

    private void PlayDefault()
    {
        m_animator.SetBool("IsSelected", false);
    }
}
