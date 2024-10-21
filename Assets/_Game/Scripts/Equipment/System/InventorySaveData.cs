using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySaveData 
{
    public List<Equipment> m_headEquipmentList;
    public List<Equipment> m_torsoEquipmentList;
    public List<Equipment> m_toolEquipmentList;


    public InventorySaveData(List<Equipment> headEquipmentList, List<Equipment> torsoEquipmentList, List<Equipment> toolEquipmentList)
    {
        m_headEquipmentList = new List<Equipment>(headEquipmentList);
        m_torsoEquipmentList = new List<Equipment>(torsoEquipmentList);
        m_toolEquipmentList = new List<Equipment>(toolEquipmentList);
    }
}
