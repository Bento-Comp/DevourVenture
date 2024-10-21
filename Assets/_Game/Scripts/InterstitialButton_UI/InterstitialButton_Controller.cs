using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterstitialButton_Controller : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float m_timeBetweenInterstitial = 120f;


    [Header("References")]
    [SerializeField]
    private GameObject m_loadingUI = null;

    [SerializeField]
    private Button m_adButton = null;

    [SerializeField]
    private TMP_Text m_timerText = null;


    private float m_timer;


    bool IsInterstitialAvailable => Hack.JuicySDKHack.IsInterstitialAvailable;
    bool IsInterstitialAllowedToBeDisplayed => Hack.JuicySDKHack.IsInterstitialAllowedToBeDisplayed;
    bool IsInterstitialDisplayable => IsInterstitialAvailable && IsInterstitialAllowedToBeDisplayed;

    private void Start()
    {
        m_timer = m_timeBetweenInterstitial;
    }


    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        //if(IsInterstitialAllowedToBeDisplayed)
        m_timer -= Time.deltaTime;

        UpdateLoadingUIActivation(m_timer <= 0f);

        m_timerText.text = FormatTime.Format_Time((int)m_timer);

        if (m_timer <= 0f)
        {
            if (IsInterstitialDisplayable)
            {
                TriggerInterstitialAd();
            }
        }
    }


    public void TriggerInterstitialAd()
    {
        m_timer += m_timeBetweenInterstitial;
        Juicy.JuicySDK.NotifyInterstitialOpportunity();
    }


    private void UpdateLoadingUIActivation(bool hasTimerReachedZero)
    {
        ActivateLoadingUI(ShouldShowLoadingInterface(hasTimerReachedZero),
            ShouldEnableInterstitialButton(IsInterstitialDisplayable));
    }

    private bool ShouldShowLoadingInterface(bool hasTimerReachedZero)
        => hasTimerReachedZero == true;

    private bool ShouldEnableInterstitialButton(bool isInterstitialAvailable)
        => isInterstitialAvailable == true;

    private void ActivateLoadingUI(bool loadingUIState, bool buttonState)
    {
        m_adButton.interactable = buttonState;
        m_loadingUI.SetActive(loadingUIState);
    }
}
