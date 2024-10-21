using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuButton : Game_UI
{
    public static System.Action OnButtonPressed_DisplayShopMenuUI;

    [SerializeField]
    private GameObject m_tutorialArrow = null;

    [SerializeField]
    private string m_shopTutorialPlayerPrefKey = "IsShopTutorialCompleted";


    private void Awake()
    {
        m_tutorialArrow.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
        ShopUnlocker.OnShopFeatureUnlocked += CheckShopUnlockedState;

        LootboxEquipment_Tutorial.OnLootboxEquipmentTutorialEnd += OnLootboxEquipmentTutorialEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
        ShopUnlocker.OnShopFeatureUnlocked -= CheckShopUnlockedState;

        LootboxEquipment_Tutorial.OnLootboxEquipmentTutorialEnd -= OnLootboxEquipmentTutorialEnd;
    }

    private void Start()
    {
        CheckShopUnlockedState();
    }

    private void OnLootboxEquipmentTutorialEnd()
    {
        ToggleUI(true);
        StartShopTutorial();
    }

    private void StartShopTutorial()
    {
        if (!PlayerPrefs.HasKey(m_shopTutorialPlayerPrefKey))
        {
            PlayerPrefs.SetInt(m_shopTutorialPlayerPrefKey, 0);
            m_tutorialArrow.SetActive(true);
        }
        else
        {
            int shopTutorialCompletedState = PlayerPrefs.GetInt(m_shopTutorialPlayerPrefKey);

            m_tutorialArrow.SetActive(shopTutorialCompletedState == 0);
        }
    }

    private void CheckShopUnlockedState()
    {
        if (!PlayerPrefs.HasKey(ShopUnlocker.m_isShopUnlockedPlayerPrefKey) || LootboxEquipment_Tutorial.HasTutorialEnded == false)
        {
            ToggleUI(false);
        }
        else
        {
            int lootboxFeatureUnlockedState = PlayerPrefs.GetInt(ShopUnlocker.m_isShopUnlockedPlayerPrefKey);

            ToggleUI(lootboxFeatureUnlockedState == 1);
        }
    }

    private void OnBusinessStarted()
    {
        CheckShopUnlockedState();
    }

    private void OnEnterLevelForTheFirstTime()
    {
        ToggleUI(false);
    }

    private void OnNotEnterLevelForTheFirstTime()
    {
        CheckShopUnlockedState();
    }

    //called by button
    public void DisableTutorial()
    {
        if (PlayerPrefs.HasKey(m_shopTutorialPlayerPrefKey) && PlayerPrefs.GetInt(m_shopTutorialPlayerPrefKey) == 0)
        {
            PlayerPrefs.SetInt(m_shopTutorialPlayerPrefKey, 1);
            m_tutorialArrow.SetActive(false);
        }
    }

    //Called by button
    public void ShowShopMenuUI()
    {
        OnButtonPressed_DisplayShopMenuUI?.Invoke();
    }

}
