using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Investor_Unlocker : MonoBehaviour
{
    public static readonly string m_isInvestorUnlockedPlayerPrefKey = "IsInvestorUnlocked";


    private void Awake()
    {
        if (!PlayerPrefs.HasKey(m_isInvestorUnlockedPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_isInvestorUnlockedPlayerPrefKey, 1);
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_isInvestorUnlockedPlayerPrefKey);

            if(unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_isInvestorUnlockedPlayerPrefKey, 1);
            }
        }
    }

}
