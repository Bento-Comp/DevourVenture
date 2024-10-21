using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Stand : UniSingleton.Singleton<Manager_Stand>
{
    public static System.Action OnAllStandsUnlocked;

    [SerializeField]
    private List<Stand> m_standList = null;

    private int m_unlockedStandCount = 0;


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Stand.OnStandUnlocked += OnStandUnlocked;
    }

    private void OnDisable()
    {
        Stand.OnStandUnlocked -= OnStandUnlocked;
    }

    private void OnStandUnlocked(Stand stand)
    {
        if (m_unlockedStandCount < m_standList.Count)
            m_unlockedStandCount++;

        if (m_unlockedStandCount == m_standList.Count)
            OnAllStandsUnlocked?.Invoke();
    }

    public Stand GetStand(FoodType foodType)
    {
        for (int i = 0; i < m_standList.Count; i++)
        {
            if (m_standList[i].StandFoodType == foodType)
            {
                return m_standList[i];
            }
        }

        return null;
    }


    public IdleNumber GetTotalStandIncomeStats()
    {
        IdleNumber totalIncome_IdleNumber = new IdleNumber(0, 0);

        for (int i = 0; i < m_standList.Count; i++)
        {
            if (m_standList[i].State1 == Stand.State.Active)
            {
                totalIncome_IdleNumber += Manager_Money.Instance.CalculateGains(m_standList[i].CurrentFoodStats, m_standList[i]);
            }
        }

        return totalIncome_IdleNumber;
    }
}
