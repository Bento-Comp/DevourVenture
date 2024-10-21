using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisual : MonoBehaviour
{
    public static System.Action<int> OnSendLevelVisualIndex;

    [SerializeField]
    private GameObject m_cityVisualPrefab = null;

    [SerializeField]
    private GameObject m_beachVisualPrefab = null;

    [SerializeField]
    private string m_levelVisualStatePlayerPrefsKey = "LevelVisual";


    private int m_levelVisualIndex;


    private void OnEnable()
    {
        Manager_LevelSelector.OnAllLevelsCompleted += OnAllLevelsCompleted;
    }

    private void OnDisable()
    {
        Manager_LevelSelector.OnAllLevelsCompleted -= OnAllLevelsCompleted;
    }


    private void Start()
    {
        LoadLevelVisual();
    }


    private void LoadLevelVisual()
    {
        if (PlayerPrefs.HasKey(m_levelVisualStatePlayerPrefsKey))
            m_levelVisualIndex = PlayerPrefs.GetInt(m_levelVisualStatePlayerPrefsKey);
        else
        {
            m_levelVisualIndex = 0;
            PlayerPrefs.SetInt(m_levelVisualStatePlayerPrefsKey, m_levelVisualIndex);
        }

        if (m_levelVisualIndex % 2 == 0)
            Instantiate(m_cityVisualPrefab);
        else
            Instantiate(m_beachVisualPrefab);
    }


    private void OnAllLevelsCompleted()
    {
        m_levelVisualIndex++;
        m_levelVisualIndex %= 2;
        OnSendLevelVisualIndex?.Invoke(m_levelVisualIndex);
        PlayerPrefs.SetInt(m_levelVisualStatePlayerPrefsKey, m_levelVisualIndex);
    }

}
