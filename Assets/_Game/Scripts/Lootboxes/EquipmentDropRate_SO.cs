using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRarityDropRate
{
    public EquipmentRarity m_rarity;
    [Range(0f,1f)]
    public float m_dropRate;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment Drop Rate Table", order = 2)]
public class EquipmentDropRate_SO : ScriptableObject
{
    public List<ItemRarityDropRate> m_itemRarityDropRateList = null;

    public float GetDropRate(EquipmentRarity rarity)
    {
        for (int i = 0; i < m_itemRarityDropRateList.Count; i++)
        {
            if (m_itemRarityDropRateList[i].m_rarity == rarity)
            {
                return m_itemRarityDropRateList[i].m_dropRate;
            }
        }

        return -1f;
    }
}
