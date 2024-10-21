using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentPool
{
    public EquipmentRarity m_rarity;
    public EquipmentType m_type;
    public List<EquipmentData_SO> m_equipmentDataList;
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment Pool Data", order = 3)]
public class EquipmentPoolData_SO : ScriptableObject
{
    public List<EquipmentPool> m_equipmentPoolList;
}
