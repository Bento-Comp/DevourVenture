using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodTypeProductionTimeMultiplier
{
    public FoodType m_foodType;
    public float m_multiplier;

    public FoodTypeProductionTimeMultiplier(FoodType foodType, float multiplier)
    {
        m_foodType = foodType;
        m_multiplier = multiplier;
    }
}

public class Manager_ProductionTimerMultiplier : UniSingleton.Singleton<Manager_ProductionTimerMultiplier>
{
    private List<FoodTypeProductionTimeMultiplier> m_productionTimeMultiplierList;
    public List<FoodTypeProductionTimeMultiplier> ProductionTimeMultiplierList { get => m_productionTimeMultiplierList; }


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Stand.OnImproveProductionTime += ImproveProductionTime;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;
    }

    private void OnDisable()
    {
        Stand.OnImproveProductionTime -= ImproveProductionTime;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;
    }

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        m_productionTimeMultiplierList = new List<FoodTypeProductionTimeMultiplier>();

        for (int i = 0; i < Manager_FoodStats.Instance.FoodStatsData.m_foodStatList.Count; i++)
        {
            m_productionTimeMultiplierList.Add(new FoodTypeProductionTimeMultiplier(Manager_FoodStats.Instance.FoodStatsData.m_foodStatList[i].m_foodType, 1f));
        }
    }


    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        if (globalUpgrade.m_bonus == Bonus.ProductionTimeBoost)
        {
            FoodTypeProductionTimeMultiplier foodMultiplier = GetFoodTypeProductionTimeMultiplier(globalUpgrade.m_foodType);

            foodMultiplier.m_multiplier += 1f; //TODO : make it in global upgrade parameters
        }
    }


    public void ImproveProductionTime(FoodType foodType, float multiplierGain)
    {
        FoodTypeProductionTimeMultiplier foodMultiplier = GetFoodTypeProductionTimeMultiplier(foodType);

        foodMultiplier.m_multiplier += multiplierGain;
    }


    public float GetProductionTimeMultiplier(FoodType foodType)
    {
        FoodTypeProductionTimeMultiplier foodProductionTimeMultiplier = GetFoodTypeProductionTimeMultiplier(foodType);

        return foodProductionTimeMultiplier.m_multiplier;
    }


    public FoodTypeProductionTimeMultiplier GetFoodTypeProductionTimeMultiplier(FoodType foodType)
    {
        for (int i = 0; i < m_productionTimeMultiplierList.Count; i++)
        {
            if (m_productionTimeMultiplierList[i].m_foodType == foodType)
            {
                return m_productionTimeMultiplierList[i];
            }
        }

        return null;
    }
}
