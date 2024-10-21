using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[DefaultExecutionOrder(0)]
public class Manager_FoodStats : UniSingleton.Singleton<Manager_FoodStats>
{
    [SerializeField]
    private FoodStats_ScriptableObject m_foodStatsData = null;

    public FoodStats_ScriptableObject FoodStatsData { get => m_foodStatsData; }


    public FoodStats GetFoodStats(FoodType foodType)
    {
        for (int i = 0; i < m_foodStatsData.m_foodStatList.Count; i++)
            if (m_foodStatsData.m_foodStatList[i].m_foodType == foodType)
                return m_foodStatsData.m_foodStatList[i];

        return null;
    }


    public int GetMaxRank(FoodType foodType)
    {
        FoodStats foodStats = GetFoodStats(foodType);

        return foodStats.GetMaxRank();
    }


    public List<Bonus> TryGetRankBonus(FoodType foodType, int currentRank, int currentLevel)
    {
        List<Bonus> rankBonusList = new List<Bonus>();

        FoodStats foodStats = GetFoodStats(foodType);

        for (int i = 0; i < foodStats.m_standRankBonusList.Count; i++)
        {
            if (foodStats.m_standRankBonusList[i].m_requiredRankToUnlockBonus == currentRank)
            {
                if (currentLevel >= foodStats.m_standRankBonusList[i].m_levelThreshold)
                {
                    rankBonusList.Add(foodStats.m_standRankBonusList[i].m_rankBonus);
                }
            }
        }

        return rankBonusList;
    }
}
