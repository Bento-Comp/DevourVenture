using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniAds;

[DefaultExecutionOrder(-31999)]
public class Manager_TemporaryBoost : UniSingleton.Singleton<Manager_TemporaryBoost>
{
    public static System.Action<bool> OnBroadcastTemporaryBoostState;
    public static System.Action<bool> OnBroadcastTemporaryBoostAvailability;
    public static System.Action<float> OnBroadcastTemporaryBoostRemainingTime;
    public static System.Action OnTemporaryBoostStarted;
    public static System.Action OnTryToStartTemporaryBoost;


    [SerializeField]
    private string m_temporaryBoostID = "";

    [SerializeField]
    private float m_temporaryBoostDuration = 180f;

    [SerializeField]
    private int m_temporaryBoostMultiplier = 2;

    [SerializeField]
    private float m_checkAdAvailabilityRefreshRate = 5f;

    private Coroutine temporaryBoostCoroutine;
    private float m_temporaryBoostRemainingTime;
    private bool m_isTemporaryBoostActive;

    private float m_checkAdAvailabilityRefreshRateTimer;
    private bool m_rewardedAvailable;

    public bool IsTemporaryBoostActive { get => m_isTemporaryBoostActive; }
    public int TemporaryBoostMultiplier { get => m_temporaryBoostMultiplier; }

    bool RewardedAdsAvailable => AdsManager.Instance != null && AdsManager.Instance.RewardedAdAvailable;

    protected override void OnSingletonEnable()
    {
        UpdateLoadingUIActivation();
        AdsManager.onRewardedAvailable += OnRewardedAvailable;
        AdsManager.Instance.NotifyRewardedOpportunityStart(m_temporaryBoostID);
        TemporaryBoost_Button.OnButtonPressed_TemporaryBoost += OnButtonPressed_TemporaryBoost;

        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
    }

    private void OnDisable()
    {
        AdsManager.onRewardedAvailable -= OnRewardedAvailable;

        if (AdsManager.Instance != null)
            AdsManager.Instance.NotifyRewardedOpportunityEnd(m_temporaryBoostID);

        TemporaryBoost_Button.OnButtonPressed_TemporaryBoost -= OnButtonPressed_TemporaryBoost;

        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
    }


    private void Start()
    {
        m_isTemporaryBoostActive = false;

        OnBroadcastTemporaryBoostState?.Invoke(m_isTemporaryBoostActive);

        UpdateLoadingUIActivation();
    }

    private void Update()
    {
        CheckAdAvailability();
    }

    private void OnBusinessStarted()
    {
        UpdateLoadingUIActivation();
    }

    private void CheckAdAvailability()
    {
        if (m_rewardedAvailable == false)
        {
            m_checkAdAvailabilityRefreshRateTimer += Time.deltaTime;

            if (m_checkAdAvailabilityRefreshRateTimer < m_checkAdAvailabilityRefreshRate)
            {
                m_checkAdAvailabilityRefreshRateTimer = 0f;
                m_rewardedAvailable = RewardedAdsAvailable;
                Debug.Log("Valentin :  rewarded " + (RewardedAdsAvailable == true ? "available" : "not available"));
                UpdateLoadingUIActivation();
            }
        }
    }

    private void OnRewardedAvailable(bool rewardedAvailable)
    {
        m_rewardedAvailable = rewardedAvailable;
        UpdateLoadingUIActivation();
    }

    private void UpdateLoadingUIActivation()
    {
        OnBroadcastTemporaryBoostAvailability?.Invoke(RewardedAdsAvailable);
    }

    private void OnButtonPressed_TemporaryBoost()
    {
        OnTryToStartTemporaryBoost?.Invoke();
        StartTemporaryBoost();
    }

    private void StartTemporaryBoost()
    {
        AdsManager.Instance.ShowRewardedAd(m_temporaryBoostID, OnRewardedEnd);
    }

    private void OnRewardedEnd(bool success)
    {
        if (success)
        {
            if (temporaryBoostCoroutine == null)
            {
                temporaryBoostCoroutine = StartCoroutine(TemporaryBoost());
            }
            else
            {
                m_temporaryBoostRemainingTime += m_temporaryBoostDuration;
            }

            OnTemporaryBoostStarted?.Invoke();
        }

        UpdateLoadingUIActivation();
    }

    private IEnumerator TemporaryBoost()
    {
        _OnTemporaryBoostStart();

        while (m_temporaryBoostRemainingTime > 0.0f)
        {
            OnBroadcastTemporaryBoostRemainingTime?.Invoke(m_temporaryBoostRemainingTime);
            m_temporaryBoostRemainingTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _OnTemporaryBoostEnd();

        temporaryBoostCoroutine = null;
    }

    private void _OnTemporaryBoostStart()
    {
        m_isTemporaryBoostActive = true;
        m_temporaryBoostRemainingTime = m_temporaryBoostDuration;
        OnBroadcastTemporaryBoostState?.Invoke(m_isTemporaryBoostActive);
        UpdateLoadingUIActivation();
    }

    private void _OnTemporaryBoostEnd()
    {
        m_isTemporaryBoostActive = false;
        OnBroadcastTemporaryBoostState?.Invoke(m_isTemporaryBoostActive);
        UpdateLoadingUIActivation();
    }
}
