using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvestorBonus_UI : Game_UI
{
    public static System.Action OnAskInvestorBonusAmount;

    [SerializeField]
    private TMP_Text m_gainText = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        Investor_Actor.OnInvestorClicked += OnInvestorClicked;
        Manager_InvestorBonus.OnSendInvestorBonusAmount += OnSendInvestorBonusAmount;
        Manager_InvestorBonus.OnRemoveInvestor+= OnRemoveInvestor;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Investor_Actor.OnInvestorClicked -= OnInvestorClicked;
        Manager_InvestorBonus.OnSendInvestorBonusAmount -= OnSendInvestorBonusAmount;
        Manager_InvestorBonus.OnRemoveInvestor -= OnRemoveInvestor;
    }


    private void Start()
    {
        ToggleUI(false);
    }

    private void OnSendInvestorBonusAmount(IdleNumber amount_IdleNumber)
    {
        m_gainText.text = IdleNumber.FormatIdleNumberText(amount_IdleNumber);
    }

    private void OnInvestorClicked()
    {
        ShowUI();

        OnAskInvestorBonusAmount?.Invoke();
    }

    private void OnRemoveInvestor()
    {
        m_gainText.text = "0";
        HideUI();
    }


    private void ShowUI()
    {
        OpenUI();
    }

    private void HideUI()
    {
        CloseUI();
    }
}
