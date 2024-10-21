using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniAds;

[DefaultExecutionOrder(2)]  // must execture after Investor_Unlocker.cs
public class Manager_InvestorBonus : MonoBehaviour
{
    public static System.Action OnRemoveInvestor;
    public static System.Action<IdleNumber> OnSendInvestorBonusAmount;
    public static System.Action<Vector3, IdleNumber> OnSendInvestorBonusAmountGained;

    [SerializeField]
    private GameObject m_investorPrefab = null;

    [SerializeField]
    private Transform m_spawnLocation = null;

    [SerializeField]
    private Transform m_investorParent = null;

    [SerializeField]
    private string m_investorBoostID = "";

    [SerializeField]
    private float m_delayBetweenInvestors = 180f;

    [SerializeField]
    private float m_timeBeforeDisappearing = 20f;

    [SerializeField]
    private float m_tryToSpawnInvestorRefreshRate = 1f;

    [SerializeField]
    private int m_bonusMultiplier = 10;

    [SerializeField]
    private bool m_isInvestorInCooldownAtStart = true;


    private IdleNumber m_currentInvestorBonusAmount_Idlenumber;
    private float m_cooldownTimer = 0f;
    private float m_requestTimer = 0f;
    private float m_disappearTimer = 0f;
    private bool m_isInvestorOnCooldown = false;
    private bool m_isInvestorOnField = false;
    private bool RewardedAdsAvailable => AdsManager.Instance != null && AdsManager.Instance.RewardedAdAvailable;
    private bool m_isInvestorUnlocked = false;

    private void OnEnable()
    {
        InvestorBonus_Button.OnInvestorBonusButtonPressed += OnInvestorBonusButtonPressed;
        InvestorBonus_UI.OnAskInvestorBonusAmount += OnAskInvestorBonusAmount;

        AdsManager.Instance.NotifyRewardedOpportunityStart(m_investorBoostID);

        InvestorBonus_ButtonExit.OnInvestorBonusExitButtonPressed += OnInvestorBonusExitButtonPressed;
        AdsManager.onRewardedAvailable += OnRewardedAvailable;
    }

    private void OnDisable()
    {
        InvestorBonus_UI.OnAskInvestorBonusAmount -= OnAskInvestorBonusAmount;
        InvestorBonus_Button.OnInvestorBonusButtonPressed -= OnInvestorBonusButtonPressed;

        if (AdsManager.Instance != null)
            AdsManager.Instance.NotifyRewardedOpportunityEnd(m_investorBoostID);

        InvestorBonus_ButtonExit.OnInvestorBonusExitButtonPressed -= OnInvestorBonusExitButtonPressed;
        AdsManager.onRewardedAvailable -= OnRewardedAvailable;
    }

    private void Start()
    {
        CheckInvestorUnlockedState();
        m_isInvestorOnCooldown = m_isInvestorInCooldownAtStart;
    }

    private void Update()
    {
        if (m_isInvestorUnlocked)
        {
            ManageInvestorCooldown();

            ManageInvestorSpawn();

            ManageInvestorTimeOnField();
        }
    }

    private void CheckInvestorUnlockedState()
    {
        if (!PlayerPrefs.HasKey(Investor_Unlocker.m_isInvestorUnlockedPlayerPrefKey))
        {
            m_isInvestorUnlocked = false;
        }
        else
        {
            int investorUnlockedState = PlayerPrefs.GetInt(Investor_Unlocker.m_isInvestorUnlockedPlayerPrefKey);

            m_isInvestorUnlocked = investorUnlockedState == 1 ? true : false;
        }
    }

    private void ManageInvestorTimeOnField()
    {
        if (m_isInvestorOnField)
        {
            m_disappearTimer += Time.deltaTime;

            if (m_disappearTimer > m_timeBeforeDisappearing)
            {
                m_disappearTimer = 0f;
                m_isInvestorOnField = false;
                m_cooldownTimer = 0f;
                m_isInvestorOnCooldown = true;
                OnRemoveInvestor?.Invoke();
            }
        }
    }

    private void OnRewardedAvailable(bool rewardedAvailable)
    {
        if (rewardedAvailable == false)
        {
            m_isInvestorOnField = false;
            OnRemoveInvestor?.Invoke();
        }
    }

    private void OnInvestorBonusExitButtonPressed()
    {
        OnRemoveInvestor?.Invoke();
        m_isInvestorOnField = false;
        m_isInvestorOnCooldown = true;
    }


    private void OnAskInvestorBonusAmount()
    {
        m_currentInvestorBonusAmount_Idlenumber = Manager_Stand.Instance.GetTotalStandIncomeStats() * m_bonusMultiplier;

        OnSendInvestorBonusAmount?.Invoke(m_currentInvestorBonusAmount_Idlenumber);
    }

    private void ManageInvestorSpawn()
    {
        if (m_isInvestorOnCooldown == false)
        {
            m_requestTimer += Time.deltaTime;

            if (m_requestTimer > m_tryToSpawnInvestorRefreshRate)
            {
                m_requestTimer = 0f;
                TrySpawnInvestor();
            }
        }
    }

    private void ManageInvestorCooldown()
    {
        if (m_isInvestorOnCooldown)
        {
            m_cooldownTimer += Time.deltaTime;

            if (m_cooldownTimer > m_delayBetweenInvestors)
            {
                m_cooldownTimer = 0f;
                m_isInvestorOnCooldown = false;

                TrySpawnInvestor();
            }
        }
    }

    private void TrySpawnInvestor()
    {
        if (RewardedAdsAvailable == false)
            return;

        if (m_isInvestorOnCooldown)
            return;

        if (m_isInvestorOnField)
            return;

        SpawnInvestor();
    }

    private void SpawnInvestor()
    {
        GameObject investor = Instantiate(m_investorPrefab, m_spawnLocation.position, Quaternion.Euler(0, 180f, 0f), m_investorParent);

        m_isInvestorOnField = true;
    }

    private void OnInvestorBonusButtonPressed()
    {
        m_isInvestorOnCooldown = true;
        m_cooldownTimer = 0f;

        LaunchInvestorBonusAd();
    }


    public void LaunchInvestorBonusAd()
    {
        AdsManager.Instance.ShowRewardedAd(m_investorBoostID, OnRewardedEnd);
    }

    void OnRewardedEnd(bool success)
    {
        if (success)
        {
            OnRemoveInvestor?.Invoke();
            m_isInvestorOnField = false;
            GiveReward();
        }
    }

    private void GiveReward()
    {
        OnSendInvestorBonusAmountGained?.Invoke(m_spawnLocation.position, m_currentInvestorBonusAmount_Idlenumber);
    }
}
