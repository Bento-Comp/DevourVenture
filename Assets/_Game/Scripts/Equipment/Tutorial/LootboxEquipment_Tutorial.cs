using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]      //must execute after LootboxNotificationUI so the tutorial works
public class LootboxEquipment_Tutorial : MonoBehaviour
{
    public static System.Action OnStartLootboxEquipmentTutorial;
    public static System.Action OnLootboxEquipmentTutorialEnd;

    public static readonly string m_hasTutorialStarted = "HasTutorialStarted";
    public static readonly string m_hasTutorialEnded = "HasTutorialEnded";

    public static bool HasTutorialEnded { get => PlayerPrefs.GetInt(m_hasTutorialEnded) == 1; }
    public static bool HasTutorialStarted { get => PlayerPrefs.GetInt(m_hasTutorialStarted) == 1; }



    private void Start()
    {
        if (!PlayerPrefs.HasKey(m_hasTutorialStarted))
        {
            PlayerPrefs.SetInt(m_hasTutorialStarted, 1);
            PlayerPrefs.SetInt(m_hasTutorialEnded, 0);
            OnStartLootboxEquipmentTutorial?.Invoke();
        }
        else
        {
            int unlockedState = PlayerPrefs.GetInt(m_hasTutorialStarted);

            if (unlockedState == 0)
            {
                PlayerPrefs.SetInt(m_hasTutorialStarted, 1);
                PlayerPrefs.SetInt(m_hasTutorialEnded, 0);
                OnStartLootboxEquipmentTutorial?.Invoke();
            }
        }
    }


    private void OnEnable()
    {
        EquipmentDetail_UI.OnTutorialCompleted += OnTutorialCompleted;
    }

    private void OnDisable()
    {
        EquipmentDetail_UI.OnTutorialCompleted -= OnTutorialCompleted;
    }

    private void OnTutorialCompleted()
    {
        PlayerPrefs.SetInt(m_hasTutorialEnded, 1);
        OnLootboxEquipmentTutorialEnd?.Invoke();
    }



}
