using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodStats
{
    [Header("Food Stats")]
    public FoodType m_foodType;
    public string m_name;
    public float m_productionTime;
    public int m_maxLevel;
    public IdleNumber m_unlockCost_IdleNumber;

    [Header("Rank Bonus")]
    public List<Stand_RankBonus> m_standRankBonusList;

    [Header("Function parameters")]
    public IdleNumber m_costLinearFactor;
    public float m_costExponentialFactor;

    public IdleNumber m_gainLinearFactor;
    public float m_gainExponentialFactor;


    public IdleNumber EvaluateUpgradeCost(int level)
    {
        IdleNumber result = new IdleNumber(-1, 0);

        if (level == m_maxLevel)
            return result;

        float exp = m_costExponentialFactor * level * Mathf.Log10(Mathf.Exp(1.0f));
        int partieEntiere = (int)exp;
        float partieDecimale = exp - partieEntiere;

        result = m_costLinearFactor * Mathf.Pow(10, partieDecimale);
        result.m_exp += partieEntiere;

        return result;

    }

    public IdleNumber EvaluateGain(int level)
    {
        IdleNumber result = new IdleNumber(0f, 0);

        float exp = m_gainExponentialFactor * level * Mathf.Log10(Mathf.Exp(1.0f));
        int partieEntiere = (int)exp;
        float partieDecimale = exp - partieEntiere;

        result = m_gainLinearFactor * Mathf.Pow(10, partieDecimale);
        result.m_exp += partieEntiere;

        return result;
    }

    public int GetPreviousRankLevel(int currentRank)
    {
        int tmp = 0;

        if (currentRank == 0)
            tmp = 0;

        for (int i = 0; i < m_standRankBonusList.Count; i++)
        {
            if (currentRank - 1 == m_standRankBonusList[i].m_requiredRankToUnlockBonus)
            {
                tmp = m_standRankBonusList[i].m_levelThreshold;
            }
        }

        return tmp;
    }

    public int GetNextRankLevel(int currentRank)
    {
        for (int i = 0; i < m_standRankBonusList.Count; i++)
        {
            if (currentRank == m_standRankBonusList[i].m_requiredRankToUnlockBonus)
            {
                return m_standRankBonusList[i].m_levelThreshold;
            }
        }

        return m_maxLevel;
    }

    public int GetMaxRank()
    {
        int tmp = 0;

        for (int i = 0; i < m_standRankBonusList.Count; i++)
        {
            if (m_standRankBonusList[i].m_requiredRankToUnlockBonus > tmp)
            {
                tmp = m_standRankBonusList[i].m_requiredRankToUnlockBonus;
            }
        }

        return tmp;
    }
}
