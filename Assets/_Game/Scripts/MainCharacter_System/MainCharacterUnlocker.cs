using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterUnlocker : MonoBehaviour
{
    public static System.Action OnMainCharacterUnlocked;

    public static readonly string m_isMainCharacterPlayerPrefKey = "IsMainCharacterUnlocked";



    private void Awake()
    {
        if (!PlayerPrefs.HasKey(m_isMainCharacterPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_isMainCharacterPlayerPrefKey, 1);
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_isMainCharacterPlayerPrefKey);

            if (unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_isMainCharacterPlayerPrefKey, 1);
                OnMainCharacterUnlocked?.Invoke();
            }
        }

    }



}
