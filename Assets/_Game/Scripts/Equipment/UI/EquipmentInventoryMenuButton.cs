using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)] //Must execute before Lootbox_Unlocker.cs
public class EquipmentInventoryMenuButton : Game_UI
{
    public static System.Action OnButtonPressed_DisplayEquipmentInventoryMenuUI;


    [SerializeField]
    private GameObject m_tutorialArrow = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        Lootbox_Unlocker.OnLootboxFeatureUnlocked += CheckEquipmentInventoryUnlockedState;
        LootboxNotificationUI.OnLootboxTutorialEnd += OnLootboxTutorialEnd;

        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial += OnStartLootboxEquipmentTutorial;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;

        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Lootbox_Unlocker.OnLootboxFeatureUnlocked -= CheckEquipmentInventoryUnlockedState;
        LootboxNotificationUI.OnLootboxTutorialEnd -= OnLootboxTutorialEnd;

        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial -= OnStartLootboxEquipmentTutorial;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;

        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
    }

    private void Awake()
    {
        ToggleUI(false);
        m_tutorialArrow.SetActive(false);
        //CheckEquipmentInventoryUnlockedState();
    }

    private void OnStartLootboxEquipmentTutorial()
    {
        ToggleUI(false);
        m_tutorialArrow.SetActive(false);
    }

    private void OnLootboxTutorialEnd()
    {
        ToggleUI(true);
        m_tutorialArrow.SetActive(true);
    }

    private void OnBusinessStarted()
    {
        CheckEquipmentInventoryUnlockedState();
    }

    private void OnNotEnterLevelForTheFirstTime()
    {
        CheckEquipmentInventoryUnlockedState();
    }

    private void OnEnterLevelForTheFirstTime()
    {
        ToggleUI(false);
    }

    private void CheckEquipmentInventoryUnlockedState()
    {
        /*
        if (PlayerPrefs.HasKey(Lootbox_Unlocker.m_isLootboxUnlockedPlayerPrefKey))
        {
            int equipmentInventoryFeatureUnlockedState = PlayerPrefs.GetInt(Lootbox_Unlocker.m_isLootboxUnlockedPlayerPrefKey);
            Debug.Log(equipmentInventoryFeatureUnlockedState == 1 ? "true" : "false");
            ToggleUI(equipmentInventoryFeatureUnlockedState == 1);
        }
        */
        ToggleUI(LootboxEquipment_Tutorial.HasTutorialStarted);
    }


    //Called by button
    public void ShowEquipmentInventoryMenuUI()
    {
        OnButtonPressed_DisplayEquipmentInventoryMenuUI?.Invoke();

        m_tutorialArrow.SetActive(false);
    }



}
