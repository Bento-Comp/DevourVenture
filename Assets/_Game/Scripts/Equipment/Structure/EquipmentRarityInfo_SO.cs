using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentRarityInfo
{
    public EquipmentRarity m_rarity;
    public Color m_rarityColor;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment Rarity Info Data", order = 4)]
public class EquipmentRarityInfo_SO : ScriptableObject
{
    public List<EquipmentRarityInfo> m_equipmentRarityInfoList;
    public EquipmentRarityInfo m_nullEquipmentRarityInfo;


    public Color GetColor(EquipmentRarity rarity)
    {
        for (int i = 0; i < m_equipmentRarityInfoList.Count; i++)
        {
            if (m_equipmentRarityInfoList[i].m_rarity == rarity)
                return m_equipmentRarityInfoList[i].m_rarityColor;
        }

        return GetDefaultColor();
    }


    public Color GetDefaultColor()
    {
        return m_nullEquipmentRarityInfo.m_rarityColor;
    }
}
