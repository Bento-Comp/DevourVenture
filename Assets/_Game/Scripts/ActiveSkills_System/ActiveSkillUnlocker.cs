using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillUnlocker : MonoBehaviour
{
    public static System.Action OnActiveSkillFeatureUnlocked;

    public static readonly string m_isPopularDishSkillUnlockedPlayerPrefKey = "IsPopularDishSkillUnlocked";
    public static bool m_isActiveSkillUnlocked { get => GetUnlockedStatus(); }


    private void OnEnable()
    {
        Manager_Stand.OnAllStandsUnlocked += StartTutorial;
    }

    private void OnDisable()
    {
        Manager_Stand.OnAllStandsUnlocked -= StartTutorial;
    }

    private void Start()
    {
#if UNITY_EDITOR
        StartTutorial();
#endif
    }

    private void StartTutorial()
    {
        if (!PlayerPrefs.HasKey(m_isPopularDishSkillUnlockedPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_isPopularDishSkillUnlockedPlayerPrefKey, 1);
            OnActiveSkillFeatureUnlocked?.Invoke();
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_isPopularDishSkillUnlockedPlayerPrefKey);

            if (unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_isPopularDishSkillUnlockedPlayerPrefKey, 1);
                OnActiveSkillFeatureUnlocked?.Invoke();
            }
        }
    }


    private static bool GetUnlockedStatus()
    {
        if (PlayerPrefs.HasKey(m_isPopularDishSkillUnlockedPlayerPrefKey))
            if (PlayerPrefs.GetInt(m_isPopularDishSkillUnlockedPlayerPrefKey) == 1)
                return true;

        return false;
    }
}
