using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Lootbox_Unlocker : MonoBehaviour
{
    public static System.Action OnLootboxFeatureUnlocked;

    public static readonly string m_isLootboxUnlockedPlayerPrefKey = "IsLootboxUnlocked";

    public static bool IsLootboxFeatureUnlocked { get => CheckIsLootboxFeatureUnlocked(); }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(m_isLootboxUnlockedPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_isLootboxUnlockedPlayerPrefKey, 1);
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_isLootboxUnlockedPlayerPrefKey);

            if (unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_isLootboxUnlockedPlayerPrefKey, 1);
            }
        }

        OnLootboxFeatureUnlocked?.Invoke();
    }


    private static bool CheckIsLootboxFeatureUnlocked()
    {
        if (PlayerPrefs.HasKey(m_isLootboxUnlockedPlayerPrefKey))
        {
            return PlayerPrefs.GetInt(m_isLootboxUnlockedPlayerPrefKey) == 1;
        }

        return false;
    }
}
