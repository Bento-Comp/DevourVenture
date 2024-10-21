using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class Manager_IdleGainsUI : Game_UI
{
    public static System.Action<bool> OnClaimIdleRewardGain_ButtonPress;
    public static System.Action OnIdleRewardWindowClosed;

    [SerializeField]
    private Button m_doubleIdleGainsButton = null;

    [SerializeField]
    private GameObject m_loadingUI = null;

    [SerializeField]
    private TMP_Text m_hoursProgressionText = null;

    [SerializeField]
    private TMP_Text m_gainsText = null;

    [SerializeField]
    private Image m_progressionImage = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        ToggleUI(false);
        Manager_IdleGains.OnSendTotalIdleGains += OnSendTotalIdleGains;
        Manager_IdleGains.OnGainIdleReward += OnGainIdleReward;
        Manager_IdleGains.OnSendRewardedAvailable += OnRewardedAvailable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_IdleGains.OnSendTotalIdleGains -= OnSendTotalIdleGains;
        Manager_IdleGains.OnGainIdleReward -= OnGainIdleReward;
        Manager_IdleGains.OnSendRewardedAvailable -= OnRewardedAvailable;
    }

    private void OnRewardedAvailable(bool isAvailable)
    {
        if (isAvailable)
        {
            m_doubleIdleGainsButton.interactable = true;
            m_loadingUI.SetActive(false);
        }
        else
        {
            m_doubleIdleGainsButton.interactable = false;
            m_loadingUI.SetActive(true);
        }
    }


    protected override void OnAnimationEnd_Disapear()
    {
        base.OnAnimationEnd_Disapear();

        Manager_Session.StartSession();

        OnIdleRewardWindowClosed?.Invoke();
    }

    private void OnGainIdleReward()
    {
        CloseUI();
    }

    //Called by button
    public void Button_PressClaimIdleReward(bool isRewardWithBonus)
    {
        OnClaimIdleRewardGain_ButtonPress?.Invoke(isRewardWithBonus);
    }


    private void OnSendTotalIdleGains(int maxHours, double currentProgression, IdleNumber gainsAmount_IdleNumber)
    {
        OpenUI();

        m_hoursProgressionText.text = HoursToString(currentProgression) + " / " + maxHours + "h";

        m_progressionImage.fillAmount = (float)(currentProgression / maxHours);

        m_gainsText.text = IdleNumber.FormatIdleNumberText(gainsAmount_IdleNumber);
    }

    public string HoursToString(double hours)
    {
        TimeSpan timeSpan = TimeSpan.FromHours(hours);

        string displayString = "";

        if(timeSpan.Hours > 0)
            displayString = timeSpan.Hours + "h";

        if(timeSpan.Minutes > 0 || timeSpan.Hours <= 0)
            displayString += timeSpan.Minutes + "m";

        return displayString;
    }
}
