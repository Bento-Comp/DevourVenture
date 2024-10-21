using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment Data", order = 4)]
public class EquipmentData_SO : ScriptableObject
{
    public string m_name = "";
    public Sprite m_equipmentIcon = null;
    public EquipmentRarity m_rarity;
    public EquipmentType m_type;
    public int m_number;
    public string m_equipmentID { get => Equipment.GetEquipmentID(m_rarity, m_number); }
}
