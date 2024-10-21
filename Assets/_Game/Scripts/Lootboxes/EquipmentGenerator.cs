using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGenerator : UniSingleton.Singleton<EquipmentGenerator>
{
    public static System.Action<Equipment> OnEquipmentGenerated;
    public static System.Action<List<Equipment>, LootboxType> OnEquipmentGeneratedFromLootbox;


    [SerializeField]
    private EquipmentDropRate_SO m_standardLootboxEquipmentDropRate = null;

    [SerializeField]
    private EquipmentDropRate_SO m_premiumLootboxEquipmentDropRate = null;

    [SerializeField]
    private EquipmentPoolData_SO m_equipmentPoolData_SO = null;

    [SerializeField]
    private float m_defaultValue_BonusMovementSpeed = 0.15f;

    [SerializeField]
    private float m_defaultValue_ProfitMultiplier = 0.1f;

    [SerializeField]
    private int m_equipmentPerLootbox = 2;

    [SerializeField]
    private float m_equipmentEffectMultiplierFactor_Common = 1f;

    [SerializeField]
    private float m_equipmentEffectMultiplierFactor_Rare = 1.25f;

    [SerializeField]
    private float m_equipmentEffectMultiplierFactor_Legendary = 1.5f;


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Lootbox_Inventory.OnLootboxOpened += OnLootboxOpened;
    }

    private void OnDisable()
    {
        Lootbox_Inventory.OnLootboxOpened -= OnLootboxOpened;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            GenerateRandomEquipment(false);
    }

    private void OnLootboxOpened(LootboxType lootboxType)
    {
        List<Equipment> generatedEquipmentList = new List<Equipment>();

        if (LootboxEquipment_Tutorial.HasTutorialEnded == false)
        {
            generatedEquipmentList.Add(GenerateTutorialEquipment(EquipmentType.Head));
            generatedEquipmentList.Add(GenerateTutorialEquipment(EquipmentType.Tool));

            OnEquipmentGeneratedFromLootbox?.Invoke(generatedEquipmentList, lootboxType);
            return;
        }

        switch (lootboxType)
        {
            case LootboxType.Standard:
                for (int i = 0; i < m_equipmentPerLootbox; i++)
                    generatedEquipmentList.Add(GenerateRandomEquipment(false));
                break;
            case LootboxType.Premium:
                for (int i = 0; i < m_equipmentPerLootbox; i++)
                    generatedEquipmentList.Add(GenerateRandomEquipment(true));
                break;
            default:
                break;
        }

        OnEquipmentGeneratedFromLootbox?.Invoke(generatedEquipmentList, lootboxType);
    }


    public Sprite GetEquipmentSprite(Equipment equipment)
    {
        for (int i = 0; i < m_equipmentPoolData_SO.m_equipmentPoolList.Count; i++)
        {
            if (m_equipmentPoolData_SO.m_equipmentPoolList[i].m_rarity == equipment.m_rarity
                && m_equipmentPoolData_SO.m_equipmentPoolList[i].m_type == equipment.m_type)
            {
                for (int j = 0; j < m_equipmentPoolData_SO.m_equipmentPoolList[i].m_equipmentDataList.Count; j++)
                {
                    if (m_equipmentPoolData_SO.m_equipmentPoolList[i].m_equipmentDataList[j].m_number == equipment.m_number)
                        return m_equipmentPoolData_SO.m_equipmentPoolList[i].m_equipmentDataList[j].m_equipmentIcon;
                }
            }
        }

        Debug.LogError("Could not retreive sprite");
        return null;
    }

    private Equipment GenerateTutorialEquipment(EquipmentType equipmentType)
    {
        EquipmentData_SO generatedEquipmentData = GetGeneratedEquipmentData(EquipmentRarity.Common, equipmentType);

        Equipment generatedEquipment = new Equipment(EquipmentEffect.ProfitMultiplier, generatedEquipmentData.m_rarity, generatedEquipmentData.m_type, generatedEquipmentData.m_number, m_defaultValue_ProfitMultiplier, generatedEquipmentData.m_name);

        OnEquipmentGenerated?.Invoke(generatedEquipment);

        return generatedEquipment;
    }

    private Equipment GenerateRandomEquipment(bool isPremium)
    {
        EquipmentDropRate_SO selectedDropRatesSO = isPremium ? m_premiumLootboxEquipmentDropRate : m_standardLootboxEquipmentDropRate;

        EquipmentRarity rarity = DetarmineRarity(selectedDropRatesSO);

        EquipmentType type = DetermineType();

        EquipmentEffect effect = DetermineEffect();

        float effectValue = DetermineEffectValue(effect, rarity);

        EquipmentData_SO generatedEquipmentData = GetGeneratedEquipmentData(rarity, type);

        Equipment generatedEquipment = new Equipment(effect, generatedEquipmentData.m_rarity, generatedEquipmentData.m_type, generatedEquipmentData.m_number, effectValue, generatedEquipmentData.m_name);

        OnEquipmentGenerated?.Invoke(generatedEquipment);

        return generatedEquipment;
    }


    private float DetermineEffectValue(EquipmentEffect effect, EquipmentRarity equipmentRarity)
    {
        float effectMultiplierFactor = 1f;

        switch (equipmentRarity)
        {
            case EquipmentRarity.Common:
                effectMultiplierFactor = m_equipmentEffectMultiplierFactor_Common;
                break;
            case EquipmentRarity.Rare:
                effectMultiplierFactor = m_equipmentEffectMultiplierFactor_Rare;
                break;
            case EquipmentRarity.Legendary:
                effectMultiplierFactor = m_equipmentEffectMultiplierFactor_Legendary;
                break;
            default:
                break;
        }

        switch (effect)
        {
            case EquipmentEffect.MovementSpeed:
                return m_defaultValue_BonusMovementSpeed * effectMultiplierFactor;
            case EquipmentEffect.ProfitMultiplier:
                return m_defaultValue_ProfitMultiplier * effectMultiplierFactor;
            default:
                return 0;
        }
    }


    private EquipmentRarity DetarmineRarity(EquipmentDropRate_SO equipmentDropRate_SO)
    {
        float random = Random.Range(0f, 1f);
        float accumulatedChance = 0f;

        for (int i = 0; i < equipmentDropRate_SO.m_itemRarityDropRateList.Count; i++)
        {
            accumulatedChance += equipmentDropRate_SO.m_itemRarityDropRateList[i].m_dropRate;

            if (random < accumulatedChance)
                return equipmentDropRate_SO.m_itemRarityDropRateList[i].m_rarity;
        }

        return EquipmentRarity.Common;
    }


    private EquipmentType DetermineType()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(EquipmentType)).Length);

        return (EquipmentType)random;
    }


    private EquipmentEffect DetermineEffect()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(EquipmentEffect)).Length);

        return (EquipmentEffect)random;
    }


    private EquipmentData_SO GetGeneratedEquipmentData(EquipmentRarity rarity, EquipmentType type)
    {
        for (int i = 0; i < m_equipmentPoolData_SO.m_equipmentPoolList.Count; i++)
        {
            if (m_equipmentPoolData_SO.m_equipmentPoolList[i].m_rarity == rarity && m_equipmentPoolData_SO.m_equipmentPoolList[i].m_type == type)
            {
                int randomIndex = Random.Range(0, m_equipmentPoolData_SO.m_equipmentPoolList[i].m_equipmentDataList.Count);

                return m_equipmentPoolData_SO.m_equipmentPoolList[i].m_equipmentDataList[randomIndex];
            }
        }

        return null;
    }
}
