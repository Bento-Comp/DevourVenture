using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(1)]
public class LootboxNotificationUI : Game_UI
{
    public static System.Action OnShowOpeningLootboxUI;
    public static System.Action OnAskLootboxInInventoryCount;
    public static System.Action OnLootboxTutorialEnd;


    [SerializeField]
    private TMP_Text m_lootboxCountText = null;

    [SerializeField]
    private GameObject m_tutorialArrow = null;

    private int m_standardLootBoxCount;
    private int m_premiumLootBoxCount;
    private int m_totalLootbox { get => m_standardLootBoxCount + m_premiumLootBoxCount; }
    private bool m_isTutorialCompleted = true;

    private void Awake()
    {
        m_tutorialArrow.SetActive(false);
    }

    protected override void OnEnable()
    {
        Lootbox_Inventory.OnUpdateLootboxCount += OnUpdateLootboxCount;
        Lootbox_Inventory.OnLootboxPurchased += OnLootboxPurchased;
        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial += OnStartLootboxEquipmentTutorial;
        OnAskLootboxInInventoryCount?.Invoke();
    }

    protected override void OnDisable()
    {
        Lootbox_Inventory.OnUpdateLootboxCount -= OnUpdateLootboxCount;
        Lootbox_Inventory.OnLootboxPurchased -= OnLootboxPurchased;
        LootboxEquipment_Tutorial.OnStartLootboxEquipmentTutorial -= OnStartLootboxEquipmentTutorial;
    }


    private void OnStartLootboxEquipmentTutorial()
    {
        m_isTutorialCompleted = false;
        m_tutorialArrow.SetActive(true);
    }

    private void OnLootboxPurchased()
    {
        ShowOpeningLootboxInterface();
    }

    private void OnUpdateLootboxCount(int standardLootboxCount, int premiumLootboxCount)
    {
        m_standardLootBoxCount = standardLootboxCount;
        m_premiumLootBoxCount = premiumLootboxCount;

        UpdateLootboxNotificationUI();
    }


    private void UpdateLootboxNotificationUI()
    {
        if (m_totalLootbox <= 0)
        {
            ToggleUI(false);
            return;
        }
        else
        {
            OpenUI();
            m_lootboxCountText.text = m_totalLootbox.ToString();
        }
    }


    //called by button
    public void ShowOpeningLootboxInterface()
    {
        if (m_totalLootbox <= 0)
            return;

        OnShowOpeningLootboxUI?.Invoke();

        if (m_isTutorialCompleted == false)
        {
            OnLootboxTutorialEnd?.Invoke();
            m_isTutorialCompleted = true;
        }

        m_tutorialArrow.SetActive(false);
    }
}
