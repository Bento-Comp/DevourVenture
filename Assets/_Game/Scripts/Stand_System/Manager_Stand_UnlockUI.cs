using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_Stand_UnlockUI : Game_UI
{
    public static System.Action<Stand> OnButtonPress_TryUnlock;

    [SerializeField]
    private TMP_Text m_unlockCostText = null;

    [SerializeField]
    private Button m_unlockButton = null;

    [SerializeField]
    private Color m_purchasableUpgradeColor = Color.white;

    [SerializeField]
    private Color m_notPurchasableUpgradeColor = Color.red;

    [SerializeField]
    private float m_upgradeUIOffsetScreenPercent = 0.075f;


    private Stand m_selectedStand;


    protected override void OnEnable()
    {
        base.OnEnable();
        Stand.OnShowUI += OnShowUI;
        Manager_Money.OnUpdateMoney += UpdateButtonsActivation;
        Stand.OnStandSelected += OnStandSelected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Stand.OnShowUI -= OnShowUI;
        Manager_Money.OnUpdateMoney -= UpdateButtonsActivation;
        Stand.OnStandSelected -= OnStandSelected;
    }

    private void OnDestroy()
    {
        HideUIArea.onClickHideUIArea -= HideUI;
    }

    private void Start()
    {
        ToggleUI(false);
    }


    private void OnShowUI(Stand.State state)
    {
        if (IsUIOpen)
            return;

        HideUIArea.onClickHideUIArea += HideUI;


        if (state == Stand.State.NotActive)
            OpenUI();

        UpdateButtonsActivation();

        m_unlockCostText.text = IdleNumber.FormatIdleNumberText(m_selectedStand.UnlockCost_IdleNumber);
    }

    public void HideUI()
    {
        HideUIArea.onClickHideUIArea -= HideUI;

        CloseUI();
    }

    private void OnStandSelected(Stand stand, Vector3 standPosition)
    {
        m_selectedStand = stand;
    }

    void UpdateButtonsActivation()
    {
        if (IsUIOpen)
        {
            UpdateUnlockButtonActivation();
        }
    }

    void UpdateUnlockButtonActivation()
    {
        if (m_selectedStand == null)
            return;

        if (Manager_Money.Instance.HasEnoughMoney(m_selectedStand.UnlockCost_IdleNumber))
        {
            m_unlockCostText.color = m_purchasableUpgradeColor;
            m_unlockButton.interactable = true;
        }
        else
        {
            m_unlockCostText.color = m_notPurchasableUpgradeColor;
            m_unlockButton.interactable = false;
        }
    }


    //Called by button
    public void ButtonPress_TryUnlock()
    {
        OnButtonPress_TryUnlock?.Invoke(m_selectedStand);
        HideUI();
    }
}
