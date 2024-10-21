using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodTypeMoneyMultiplier
{
    public FoodType m_foodType;
    public float m_multiplier;

    public FoodTypeMoneyMultiplier(FoodType foodType, float multiplier)
    {
        m_foodType = foodType;
        m_multiplier = multiplier;
    }
}

public class Manager_MoneyMultiplier : UniSingleton.Singleton<Manager_MoneyMultiplier>
{
    [SerializeField]
    private int m_intialGlobalMoneyIncomeMultiplier = 5;


    private List<FoodTypeMoneyMultiplier> m_moneyMultiplierList;
    private float m_globalMoneyIncomeBaseMultiplier = 1f;


    public List<FoodTypeMoneyMultiplier> MoneyMultiplierList { get => m_moneyMultiplierList; }


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Stand.OnIncreaseGainMultipler += OnIncreaseGainMultipler;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;
    }

    private void OnDisable()
    {
        Stand.OnIncreaseGainMultipler -= OnIncreaseGainMultipler;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;
    }

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        m_moneyMultiplierList = new List<FoodTypeMoneyMultiplier>();

        for (int i = 0; i < Manager_FoodStats.Instance.FoodStatsData.m_foodStatList.Count; i++)
        {
            m_moneyMultiplierList.Add(new FoodTypeMoneyMultiplier(Manager_FoodStats.Instance.FoodStatsData.m_foodStatList[i].m_foodType, 1f));
        }
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.GlobalGainMultiplier)
        {
            IncreaseGlobalMoneyIncomeMultiplier(globalUpgrade.m_profitMultiplier);
        }

        if (globalUpgrade.m_bonus == Bonus.FoodGainMultiplier)
        {
            IncreaseFoodTypeMoneyMultiplier(globalUpgrade.m_foodType, globalUpgrade.m_profitMultiplier);
        }
    }

    private void OnIncreaseGainMultipler(FoodType foodType, float multiplierGain)
    {
        IncreaseFoodTypeMoneyMultiplier(foodType, multiplierGain);
    }


    private void IncreaseFoodTypeMoneyMultiplier(FoodType foodType, float multiplierGain)
    {
        FoodTypeMoneyMultiplier foodMultiplier = GetFoodTypeMoneyMultiplier(foodType);

        //foodMultiplier.m_multiplier += multiplierGain;
        foodMultiplier.m_multiplier *= multiplierGain;
    }

    private void IncreaseGlobalMoneyIncomeMultiplier(float multiplier)
    {
        m_globalMoneyIncomeBaseMultiplier *= multiplier;
    }


    public float GetFoodMultiplier(FoodType foodType)
    {
        FoodTypeMoneyMultiplier foodMultiplier = GetFoodTypeMoneyMultiplier(foodType);

        return foodMultiplier.m_multiplier;
    }

    public float GetGlobalMoneyTemporaryMultiplier()
    {
        Manager_TemporaryBoost manager_TemporaryBoost = Manager_TemporaryBoost.Instance;

        if (manager_TemporaryBoost == null)
            return 1.0f;

        return manager_TemporaryBoost.IsTemporaryBoostActive ? manager_TemporaryBoost.TemporaryBoostMultiplier : 1.0f;
    }

    public float GetGlobalMoneyMultiplier()
    {
        //return m_globalMoneyIncomeBaseMultiplier + m_intialGlobalMoneyIncomeMultiplier * m_globalMoneyIncomeBoostCount;

        return m_globalMoneyIncomeBaseMultiplier;
    }

    public float GetEquipmentMoneyMultiplier()
    {
        float baseMultiplier = 1f;

        if (MainCharacterEquipedEquipment.Instance != null)
            baseMultiplier += MainCharacterEquipedEquipment.Instance.GetEquipmentBonusValue(EquipmentEffect.ProfitMultiplier);

        return baseMultiplier;
    }

    private FoodTypeMoneyMultiplier GetFoodTypeMoneyMultiplier(FoodType foodType)
    {
        for (int i = 0; i < m_moneyMultiplierList.Count; i++)
        {
            if (m_moneyMultiplierList[i].m_foodType == foodType)
            {
                return m_moneyMultiplierList[i];
            }
        }
        return null;
    }

}
