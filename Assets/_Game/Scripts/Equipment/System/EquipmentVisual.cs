using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentVisual : MonoBehaviour
{
    [SerializeField]
    private EquipmentRarity m_rarity;

    [SerializeField]
    private EquipmentType m_type;

    [SerializeField]
    private int m_number;

    public string m_equipmentID { get => Equipment.GetEquipmentID(m_rarity, m_number); }
    public EquipmentType EquipmentType { get => m_type; }


    public void EnableVisual()
    {
        gameObject.SetActive(true);
    }

    public void DisableVisual()
    {
        gameObject.SetActive(false);
    }

}
