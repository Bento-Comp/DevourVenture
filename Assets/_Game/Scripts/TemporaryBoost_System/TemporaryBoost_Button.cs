using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemporaryBoost_Button : Game_UI
{
    public static System.Action OnButtonPressed_TemporaryBoost;


    [SerializeField]
    private Button m_temporaryBoostButton = null;

    [SerializeField]
    private GameObject m_loadingUI = null;

    [SerializeField]
    private GameObject m_buttonElementsWhenActive = null;

    [SerializeField]
    private TMP_Text m_timerText = null;


    protected override void OnEnable()
    {
        base.OnEnable();

        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;

        Manager_TemporaryBoost.OnBroadcastTemporaryBoostState += OnBroadcastTemporaryBoostState;
        Manager_TemporaryBoost.OnBroadcastTemporaryBoostAvailability += UpdateLoadingUIActivation;
        Manager_TemporaryBoost.OnBroadcastTemporaryBoostRemainingTime += OnBroadcastTemporaryBoostRemainingTime;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;

        Manager_TemporaryBoost.OnBroadcastTemporaryBoostState -= OnBroadcastTemporaryBoostState;
        Manager_TemporaryBoost.OnBroadcastTemporaryBoostAvailability -= UpdateLoadingUIActivation;
        Manager_TemporaryBoost.OnBroadcastTemporaryBoostRemainingTime -= OnBroadcastTemporaryBoostRemainingTime;
    }

    private void Start()
    {
        //ActivateLoadingUI(true);
        m_buttonElementsWhenActive.SetActive(false);
    }

    private void OnBroadcastTemporaryBoostRemainingTime(float temporaryBosstRemainingTime)
    {
        m_timerText.text = FormatTime.Format_Time((int)temporaryBosstRemainingTime);
    }

    private void UpdateLoadingUIActivation(bool rewardedAdsAvailable)
    {
        ActivateLoadingUI(ShouldDisplayLoadingUI(rewardedAdsAvailable));
    }


    private bool ShouldDisplayLoadingUI(bool rewardedAvailable)
        => rewardedAvailable == false;


    private void ActivateLoadingUI(bool activate)
    {
        m_temporaryBoostButton.interactable = (activate == false);
        m_loadingUI.SetActive(activate);
    }

    private void OnBroadcastTemporaryBoostState(bool state)
    {
        m_buttonElementsWhenActive.SetActive(state);
    }

    private void OnBusinessStarted()
    {
        OpenUI();
    }

    private void OnEnterLevelForTheFirstTime()
    {
        ToggleUI(false);
    }

    private void OnNotEnterLevelForTheFirstTime()
    {
        ToggleUI(true);
    }

    //Called by button
    public void PressButton()
    {
        OnButtonPressed_TemporaryBoost?.Invoke();
    }
}
