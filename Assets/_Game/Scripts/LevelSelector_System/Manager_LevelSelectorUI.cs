using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(2)]
public class Manager_LevelSelectorUI : Game_UI
{
    public static System.Action OnButtonPress_TryToGoToNextLevel;


    [SerializeField]
    private Button m_nextLevelButton = null;

    [SerializeField]
    private TMP_Text m_costText = null;

    [SerializeField]
    private Image m_coinImage = null;

    [SerializeField]
    private TMP_Text m_upgradeText = null;

    [SerializeField]
    private Color m_purchasableColor = Color.white;

    [SerializeField]
    private Color m_notPurchasableColor = Color.red;

    [SerializeField]
    private Image m_buttonImage = null;

    [SerializeField]
    private Image m_buttonBackgroundImage = null;

    [SerializeField]
    private Color m_buttonDisabledColor = Color.white;

    [SerializeField]
    private Color m_buttonDisabledBackgroundColor = Color.grey;

    [SerializeField]
    private Color m_buttonEnabledColor = Color.blue;

    [SerializeField]
    private Color m_buttonEnabledBackgroundColor = Color.cyan;

    [SerializeField]
    private TMP_Text m_indicationText = null;


    private IdleNumber m_nextLevelCost;


    protected override void OnEnable()
    {
        base.OnEnable();
        LevelSelector_LevelSelectionButton.OnButtonPressed_ShowLevelSelectionUI += ShowUI;
        Manager_LevelSelector.OnUpdateNextLevelConditions += OnUpdateNextLevelConditions;
        Manager_LevelSelector.OnNewLevelIsAffordable += OnNewLevelIsAffordable;
        Manager_LevelSelector.OnSendNextLevelCost += OnSendNextLevelCost;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelSelector_LevelSelectionButton.OnButtonPressed_ShowLevelSelectionUI -= ShowUI;
        Manager_LevelSelector.OnUpdateNextLevelConditions -= OnUpdateNextLevelConditions;
        Manager_LevelSelector.OnNewLevelIsAffordable -= OnNewLevelIsAffordable;
        Manager_LevelSelector.OnSendNextLevelCost -= OnSendNextLevelCost;
    }

    private void OnDestroy()
    {
        HideUIArea.onClickHideUIArea -= HideUI;
    }

    private void Start()
    {
        ToggleUI(false);
    }


    public void HideUI()
    {
        HideUIArea.onClickHideUIArea -= HideUI;

        CloseUI();
    }

    private void ShowUI()
    {
        if (IsUIOpen)
            return;

        HideUIArea.onClickHideUIArea += HideUI;

        OpenUI();
    }

    public void ButtonPress_TryToGoToNextLevel()
    {
        OnButtonPress_TryToGoToNextLevel?.Invoke();
    }

    private void OnSendNextLevelCost(IdleNumber nextLevelCost_IdleNumber)
    {
        m_nextLevelCost = new IdleNumber(nextLevelCost_IdleNumber);
    }

    private void OnNewLevelIsAffordable(bool canGoToNextLevel)
    {
        if (canGoToNextLevel)
            OnUpdateNextLevelConditions(true);
    }

    private void OnUpdateNextLevelConditions(bool canGoToNextLevel)
    {
        if (canGoToNextLevel)
        {
            m_costText.text = IdleNumber.FormatIdleNumberText(m_nextLevelCost);

            if (Manager_Money.Instance.HasEnoughMoney(m_nextLevelCost))
            {
                m_costText.color = m_purchasableColor;
                m_nextLevelButton.interactable = true;
            }
            else
            {
                m_costText.color = m_notPurchasableColor;
                m_nextLevelButton.interactable = false;
            }

            m_buttonImage.color = m_buttonEnabledColor;
            m_buttonBackgroundImage.color = m_buttonEnabledBackgroundColor;

            m_costText.gameObject.SetActive(true);
            m_coinImage.gameObject.SetActive(true);
            m_upgradeText.gameObject.SetActive(false);


            m_indicationText.gameObject.SetActive(false);
        }
        else
        {
            m_buttonImage.color = m_buttonDisabledColor;
            m_buttonBackgroundImage.color = m_buttonDisabledBackgroundColor;

            m_costText.gameObject.SetActive(false);
            m_coinImage.gameObject.SetActive(false);
            m_upgradeText.gameObject.SetActive(true);


            m_indicationText.gameObject.SetActive(true);
            m_indicationText.text = "Upgrade your stands!";

            m_nextLevelButton.interactable = false;
        }
    }
}
