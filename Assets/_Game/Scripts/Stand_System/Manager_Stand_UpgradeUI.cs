using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_Stand_UpgradeUI : Game_UI
{
    public static System.Action<Stand> OnButtonPress_TryUpgrade;

    [SerializeField]
    private TMP_Text m_upgradeCostText = null;

    [SerializeField]
    private Button m_upgradeButton = null;

    [SerializeField]
    private TMP_Text m_foodTypeText = null;

    [SerializeField]
    private TMP_Text m_levelText = null;

    [SerializeField]
    private TMP_Text m_gainText = null;

    [SerializeField]
    private TMP_Text m_productionTimeText = null;


    [Header("Slider")]
    [SerializeField]
    private Slider m_rankProgressionSlider = null;

    [SerializeField]
    private GameObject m_stars_1to5_Parent = null;

    [SerializeField]
    private List<Image> m_rankStar_1to5_ImageList = null;

    [SerializeField]
    private GameObject m_stars_6andAbove_Parent = null;

    [SerializeField]
    private List<Image> m_rankStar_6andAbove_ImageList = null;

    [SerializeField]
    private float m_progressionBarStartOffset = 0.1f;

    [SerializeField]
    private float m_upgradeUIOffsetScreenPercent = 0.075f;


    [Header("Colors")]
    [SerializeField]
    private Color m_rankActivatedColor_Rank_1to5 = Color.green;

    [SerializeField]
    private Color m_rankActivatedColor_Rank_6andAbove = Color.blue;

    [SerializeField]
    private Color m_rankDeactivatedColor = Color.grey;

    [SerializeField]
    private Color m_purchasableUpgradeColor = Color.white;

    [SerializeField]
    private Color m_notPurchasableUpgradeColor = Color.red;


    [Header("Gems feedback")]
    [SerializeField]
    private GameObject m_gemImage = null;

    [SerializeField]
    private GameObject m_gemFeedbackPrefab = null;


    private Stand m_selectedStand;


    protected override void OnEnable()
    {
        base.OnEnable();
        Stand.OnShowUI += OnShowUI;
        Manager_Money.OnUpdateMoney += UpdateButtonsActivation;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired += OnGlobalUpgradeAquired;
        Stand.OnStandSelected += OnStandSelected;
        Stand.OnUpdateLevel += OnUpdateLevel;
        Stand.OnStandRankUp += OnStandRankUp;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Stand.OnShowUI -= OnShowUI;
        Manager_Money.OnUpdateMoney -= UpdateButtonsActivation;
        Manager_GlobalUpgrades.OnGlobalUpgradeAquired -= OnGlobalUpgradeAquired;
        Stand.OnStandSelected -= OnStandSelected;
        Stand.OnUpdateLevel -= OnUpdateLevel;
        Stand.OnStandRankUp -= OnStandRankUp;
    }

    private void OnDestroy()
    {
        HideUIArea.onClickHideUIArea -= UnselectStand;
    }

    private void Start()
    {
        ToggleUI(false);
    }


    private void OnStandRankUp(bool isLoadingData)
    {
        if (isLoadingData)
            return;

        GameObject instantiatedGem = Instantiate(m_gemFeedbackPrefab, this.gameObject.transform);// m_gemImage.transform);

        RectTransform feedbackGemRectTransform = instantiatedGem.GetComponent<RectTransform>();
        feedbackGemRectTransform.position = m_gemImage.GetComponent<RectTransform>().position;
    }

    private void OnShowUI(Stand.State state)
    {
        if (IsUIOpen)
            return;

        HideUIArea.onClickHideUIArea += UnselectStand;

        if (state == Stand.State.Active)
            OpenUI();

        UpdateButtonsActivation();

        UpdateTextUpgradeCost();

        UpdateTextGain();
    }

    public void UnselectStand()
    {
        HideUIArea.onClickHideUIArea -= UnselectStand;

        m_selectedStand = null;

        CloseUI();
    }

    private void OnStandSelected(Stand stand, Vector3 standPosition)
    {
        m_selectedStand = stand;
        m_foodTypeText.text = m_selectedStand.CurrentFoodStats.m_name;
        
        m_gemImage.SetActive(!m_selectedStand.IsStandMaxed);

        UpdateTextLevel();
        UpdateStarRankImages(m_selectedStand);
        UpgradeRankProgressionBar();
        UpdateTextUpgradeCost();
        UpdateTextGain();
    }

    private void OnUpdateLevel()
    {
        UpdateTextLevel();
        UpdateStarRankImages(m_selectedStand);
        UpgradeRankProgressionBar();
        UpdateTextUpgradeCost();
        UpdateTextGain();
    }

    private void OnGlobalUpgradeAquired(GlobalUpgrade globalUpgrade)
    {
        UpdateTextLevel();
        UpdateButtonsActivation();
    }

    private void UpdateTextLevel()
    {
        if (m_selectedStand != null)
        {
            m_levelText.text = "Level " + m_selectedStand.Level.ToString();
            m_productionTimeText.text = (m_selectedStand.ProdcutionTime / Manager_ProductionTimerMultiplier.Instance.GetProductionTimeMultiplier(m_selectedStand.StandFoodType)).ToString("F2") + "s";
        }
    }

    private void UpdateButtonsActivation()
    {
        if (IsUIOpen)
        {
            UpdateUpgradeButtonCostActivation();
        }
    }

    private void UpdateTextGain()
    {
        m_gainText.text = IdleNumber.FormatIdleNumberText(m_selectedStand.GainAmount_IdleNumber
            * (int)Manager_MoneyMultiplier.Instance.GetFoodMultiplier(m_selectedStand.StandFoodType)
            * (int)Manager_MoneyMultiplier.Instance.GetGlobalMoneyMultiplier());
    }

    private void UpdateTextUpgradeCost()
    {
        if (m_selectedStand.UpgradeCost_IdleNumber.m_value == -1)
            m_upgradeCostText.text = "MAX";
        else
            m_upgradeCostText.text = IdleNumber.FormatIdleNumberText(m_selectedStand.UpgradeCost_IdleNumber);


        UpdateUpgradeButtonCostActivation();
    }

    private void UpdateStarRankImages(Stand stand)
    {
        int standMaxRank = Manager_FoodStats.Instance.GetMaxRank(stand.StandFoodType);
        int standCurrentRank = stand.Rank;
        int starsToActivate = 0;

        if (standCurrentRank > 5)
        {
            m_stars_1to5_Parent.SetActive(false);
            m_stars_6andAbove_Parent.SetActive(true);

            starsToActivate = standCurrentRank - 5;

            for (int i = 0; i < m_rankStar_6andAbove_ImageList.Count; i++)
            {
                m_rankStar_6andAbove_ImageList[i].gameObject.SetActive(i <= starsToActivate);
                m_rankStar_6andAbove_ImageList[i].color = i <= starsToActivate ? m_rankActivatedColor_Rank_6andAbove : m_rankDeactivatedColor;
            }
        }
        else
        {
            m_stars_6andAbove_Parent.SetActive(false);
            m_stars_1to5_Parent.SetActive(true);

            starsToActivate = standMaxRank;

            for (int i = 0; i < m_rankStar_1to5_ImageList.Count; i++)
            {
                m_rankStar_1to5_ImageList[i].gameObject.SetActive(i <= starsToActivate);
                m_rankStar_1to5_ImageList[i].color = stand.Rank > i ? m_rankActivatedColor_Rank_1to5 : m_rankDeactivatedColor;
            }
        }
    }

    private void UpgradeRankProgressionBar()
    {
        if (m_selectedStand.Level >= m_selectedStand.CurrentFoodStats.m_maxLevel)
        {
            m_rankProgressionSlider.value = 1f;
            return;
        }

        FoodStats foodStats = Manager_FoodStats.Instance.GetFoodStats(m_selectedStand.StandFoodType);


        int previousRankLevelThreshold = foodStats.GetPreviousRankLevel(m_selectedStand.Rank);
        int nextRankLevelThreshold = foodStats.GetNextRankLevel(m_selectedStand.Rank);

        float progression = (float)(m_selectedStand.Level - previousRankLevelThreshold) / (nextRankLevelThreshold - previousRankLevelThreshold);
        m_rankProgressionSlider.value = (1f - m_progressionBarStartOffset) * progression + m_progressionBarStartOffset;
    }

    private void UpdateUpgradeButtonCostActivation()
    {
        if (m_selectedStand == null)
            return;

        if (m_selectedStand.UpgradeCost_IdleNumber.m_value == -1)
        {
            m_upgradeCostText.color = m_purchasableUpgradeColor;
            m_upgradeButton.interactable = false;
            return;
        }

        if (Manager_Money.Instance.HasEnoughMoney(m_selectedStand.UpgradeCost_IdleNumber))
        {
            m_upgradeCostText.color = m_purchasableUpgradeColor;
            m_upgradeButton.interactable = true;
        }
        else
        {
            m_upgradeCostText.color = m_notPurchasableUpgradeColor;
            m_upgradeButton.interactable = false;
        }
    }



    public void ButtonPress_TryUpgrade()
    {
        OnButtonPress_TryUpgrade?.Invoke(m_selectedStand);
    }
}
