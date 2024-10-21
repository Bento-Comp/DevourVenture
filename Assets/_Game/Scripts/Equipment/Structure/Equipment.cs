using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentEffect
{
    MovementSpeed,
    ProfitMultiplier
}

public enum EquipmentRarity
{
    Common,
    Rare,
    Legendary
}

public enum EquipmentType
{
    Head,
    Torso,
    Tool
}

[System.Serializable]
public class Equipment
{
    public EquipmentEffect m_effect;
    public EquipmentRarity m_rarity;
    public EquipmentType m_type;
    public string m_name;
    public float m_effectValue;
    public int m_number;
    private bool isEquipped;
    public string m_equipmentID { get => GetEquipmentID(m_rarity, m_number); }
    public bool IsEquipped { get => isEquipped; set => isEquipped = value; }

    public Equipment(EquipmentEffect effect, EquipmentRarity rarity, EquipmentType type, int number, float effectValue, string name)
    {
        m_effect = effect;
        m_rarity = rarity;
        m_type = type;
        m_number = number;
        m_effectValue = effectValue;
        m_name = name;
    }

    public string GenerateID()
    {
        return (int)m_rarity + "_" + m_number;
    }

    public static string GetEquipmentID(EquipmentRarity rarity, int number)
    {
        return ((int)rarity).ToString() + "_" + number.ToString();
    }

    public string FormatEffectText()
    {
        string effectValue = (m_effectValue * 100f).ToString();

        string effectDescription = "";

        switch (m_effect)
        {
            case EquipmentEffect.MovementSpeed:
                effectDescription = "Movement speed";
                break;
            case EquipmentEffect.ProfitMultiplier:
                effectDescription = "All profits";
                break;
            default:
                break;
        }

        return "+" + (m_effectValue * 100f).ToString() + "% " + effectDescription;
    }
}
