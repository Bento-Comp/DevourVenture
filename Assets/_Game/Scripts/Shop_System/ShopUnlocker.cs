using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUnlocker : MonoBehaviour
{
    public static System.Action OnShopFeatureUnlocked;

    public static readonly string m_isShopUnlockedPlayerPrefKey = "IsShopUnlocked";


    private void Awake()
    {
        if (!PlayerPrefs.HasKey(m_isShopUnlockedPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_isShopUnlockedPlayerPrefKey, 1);
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_isShopUnlockedPlayerPrefKey);

            if (unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_isShopUnlockedPlayerPrefKey, 1);
            }
        }

        OnShopFeatureUnlocked?.Invoke();
    }
}
