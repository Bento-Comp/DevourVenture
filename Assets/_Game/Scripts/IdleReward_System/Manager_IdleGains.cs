using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniAds;


[DefaultExecutionOrder(3)]
public class Manager_IdleGains : MonoBehaviour
{
    public static Action<int, double, IdleNumber> OnSendTotalIdleGains;
    public static Action<bool> OnSendRewardedAvailable;
    public static Action OnGainIdleReward;


    [SerializeField]
    private string m_doubleRwardID = "DoubleRewwardID";

    [SerializeField]
    private int m_maxTimeIdleRewardStackInHours = 3;

    [SerializeField]
    private int m_minTimeToDisplayPopUpInSeconds = 300;

    [SerializeField]
    private int m_gainsTick = 30;

    [SerializeField]
    private int m_bonusMultiplier = 2;

    [SerializeField]
    private bool debug_useTimeMultiplier = false;

    [SerializeField]
    private bool m_isDebugEnabled = false;

    [SerializeField]
    private double debug_timeMultiplier = 60.0f;

    private double m_totalSeconds;
    private double m_totalHours;
    private IdleNumber m_totalIdleGains_Idlenumber;
    private int m_tickCount;

    private bool started;

    private void OnEnable()
    {
        Manager_IdleGainsUI.OnClaimIdleRewardGain_ButtonPress += ClaimIdleGains;
        Manager_IdleGainsUI.OnIdleRewardWindowClosed += OnIdleRewardWindowClosed;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_LevelSelector.OnChangeLevel += ClearLevelSave;

        //hack : after watching a rewarded, update the quit time to avoid the bug of idle gain rewards timer
        Manager_TemporaryBoost.OnTryToStartTemporaryBoost += OnTryToStartTemporaryBoost;
    }

    private void OnDisable()
    {
        Manager_IdleGainsUI.OnClaimIdleRewardGain_ButtonPress -= ClaimIdleGains;
        Manager_IdleGainsUI.OnIdleRewardWindowClosed -= OnIdleRewardWindowClosed;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_LevelSelector.OnChangeLevel -= ClearLevelSave;

        Manager_TemporaryBoost.OnTryToStartTemporaryBoost -= OnTryToStartTemporaryBoost;
    }

    private void Start()
    {
        started = true;
    }

    private void ClearLevelSave()
    {
        if (PlayerPrefs.HasKey("QuitTime"))
            PlayerPrefs.DeleteKey("QuitTime");
    }

    private void OnIdleRewardWindowClosed()
    {
        EndRewardedOpportunity();
    }

    void BeginRewardedOpportunity()
    {
        OnRewardedAvailable(AdsManager.Instance.RewardedAdAvailable);
        AdsManager.onRewardedAvailable += OnRewardedAvailable;
        AdsManager.Instance.NotifyRewardedOpportunityStart(m_doubleRwardID);
    }

    void EndRewardedOpportunity()
    {
        AdsManager.onRewardedAvailable -= OnRewardedAvailable;
        AdsManager.Instance.NotifyRewardedOpportunityEnd(m_doubleRwardID);
    }


    private void OnRewardedAvailable(bool rewardedAvailable)
    {
        OnSendRewardedAvailable?.Invoke(rewardedAvailable);
    }

    private void OnNotEnterLevelForTheFirstTime()
    {
        bool popUpDisplayed = CalculateIdleGains();

        if (popUpDisplayed == false)
            Manager_Session.StartSession();
    }

    private bool CalculateIdleGains()
    {
        if (!PlayerPrefs.HasKey("QuitTime"))
            return false;

        DateTime currentDate = DateTime.Now;
        TimeSpan difference;
        TimeSpan maxRewardTime = new TimeSpan(m_maxTimeIdleRewardStackInHours, 0, 0);

        long temp = Convert.ToInt64(PlayerPrefs.GetString("QuitTime"));

        DateTime oldDate = DateTime.FromBinary(temp);

        difference = currentDate.Subtract(oldDate);

        if (debug_useTimeMultiplier)
            difference = TimeSpan.FromMilliseconds(debug_timeMultiplier * difference.TotalMilliseconds);

        if (difference.TotalSeconds < m_minTimeToDisplayPopUpInSeconds)
            return false;

        m_totalSeconds = 0;

        if (difference.TotalHours > m_maxTimeIdleRewardStackInHours)
        {
            m_totalSeconds = maxRewardTime.TotalSeconds;
            m_totalHours = m_maxTimeIdleRewardStackInHours;
        }
        else
        {
            m_totalSeconds = difference.TotalSeconds;
            m_totalHours = difference.TotalHours;
        }

        m_tickCount = (int)(m_totalSeconds / m_gainsTick);

        m_totalIdleGains_Idlenumber = Manager_Stand.Instance.GetTotalStandIncomeStats() * m_tickCount;

        if (m_totalIdleGains_Idlenumber.m_value <= 0)
            return false;

        OnSendTotalIdleGains?.Invoke(m_maxTimeIdleRewardStackInHours, m_totalHours, m_totalIdleGains_Idlenumber);

        BeginRewardedOpportunity();

        return true;
    }


    private void ClaimIdleGains(bool isRewardWithBonus)
    {
        if (isRewardWithBonus)
            AdsManager.Instance.ShowRewardedAd(m_doubleRwardID, OnRewardedEnd);
        else
            GrantIdleReward(false);
    }

    void OnRewardedEnd(bool success)
    {
        if (success)
            GrantIdleReward(true);
    }

    private void GrantIdleReward(bool isRewardWithBonus)
    {
        if (isRewardWithBonus)
            Manager_Money.Instance.GainMoney(m_totalIdleGains_Idlenumber * m_bonusMultiplier);
        else
            Manager_Money.Instance.GainMoney(m_totalIdleGains_Idlenumber);

        m_totalIdleGains_Idlenumber.m_value = 0.0f;
        m_totalIdleGains_Idlenumber.m_exp = 0;

        OnGainIdleReward?.Invoke();
    }


    private void OnApplicationFocus(bool focus)
    {
        if (m_isDebugEnabled)
            Debug.Log("OnApplicationFocus", this);

        if (focus)
        {
            if (started)
                OnNotEnterLevelForTheFirstTime();
        }
        else
        {
            OnLeaveApplicationFocus();
        }
    }

    private void OnLeaveApplicationFocus()
    {
        if (m_isDebugEnabled)
            Debug.Log("On pause : SessionActive = " + Manager_Session.SessionActive);

        if (Manager_Session.SessionActive == false)
            return;

        if (m_isDebugEnabled)
            Debug.Log("Update Quit Time");

        Debug.Log("Valentin : Update quit time");

        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());

        Manager_Session.StopSession();
    }

    private void OnTryToStartTemporaryBoost()
    {
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        OnLeaveApplicationFocus();
    }
#endif

    private void OnApplicationPause()
    {
        OnLeaveApplicationFocus();
    }
}
