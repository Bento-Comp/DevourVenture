using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_WinCondition : UniSingleton.Singleton<Manager_WinCondition>
{
    public static System.Action OnAllStandMaxedOut;

    [SerializeField]
    private List<Stand> m_standList = null;

    public bool CanGoToNextLevel { get => AreStandsMaxedOut(); }


    public bool AreStandsMaxedOut()
    {
        for (int i = 0; i < m_standList.Count; i++)
        {
            if (m_standList[i].Level < m_standList[i].CurrentFoodStats.m_maxLevel)
                return false;
        }

        return true;
    }
}
