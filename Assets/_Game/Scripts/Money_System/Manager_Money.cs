using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Manager_Money : UniSingleton.Singleton<Manager_Money>
{
    public static System.Action OnUpdateMoney;
    //Vector3 : gains position in scene
    public static System.Action<Vector3, IdleNumber> OnMoneyGainCreateFx;

    [SerializeField]
    private IdleNumber m_initialAmount_IdleNumber = null;

    private string m_moneyValueKey;
    private string m_moneyExpKey;
    private IdleNumber m_money_IdleNumber = new IdleNumber(0, 0);

    public IdleNumber Money { get => m_money_IdleNumber; }


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        Manager_Order.OnGainOrderMoney += OnGainOrderMoney;
        Manager_InvestorBonus.OnSendInvestorBonusAmountGained += OnSendInvestorBonusAmountGained;

        Manager_LevelSelector.OnChangeLevel += ClearLevelSave;
    }

    private void OnDisable()
    {
        Manager_Order.OnGainOrderMoney -= OnGainOrderMoney;
        Manager_InvestorBonus.OnSendInvestorBonusAmountGained -= OnSendInvestorBonusAmountGained;

        Manager_LevelSelector.OnChangeLevel -= ClearLevelSave;
    }


    private void Start()
    {
        m_moneyValueKey = Manager_SceneManagement.LevelName + "MoneyValue";
        m_moneyExpKey = Manager_SceneManagement.LevelName + "MoneyExp";
        LoadData();
        Initialize();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(m_moneyValueKey))
        {
            m_money_IdleNumber.m_value = double.Parse(PlayerPrefs.GetString(m_moneyValueKey));
            m_money_IdleNumber.m_exp = PlayerPrefs.GetInt(m_moneyExpKey);
        }
        else
        {
            m_money_IdleNumber = new IdleNumber(m_initialAmount_IdleNumber);
            SaveMoney();
        }
    }

    private void ClearLevelSave()
    {
        if (PlayerPrefs.HasKey(m_moneyValueKey))
            PlayerPrefs.DeleteKey(m_moneyValueKey);

        if (PlayerPrefs.HasKey(m_moneyExpKey))
            PlayerPrefs.DeleteKey(m_moneyExpKey);
    }

    private void Initialize()
    {
        OnUpdateMoney?.Invoke();
    }

    private void OnSendInvestorBonusAmountGained(Vector3 investorPosition, IdleNumber amount_IdleNumber)
    {
        GainMoney(amount_IdleNumber);

        OnMoneyGainCreateFx?.Invoke(investorPosition, amount_IdleNumber);
    }

    private void OnGainOrderMoney(Order orderServed, Vector3 fxSpawnPosition)
    {
        FoodStats foodStats = Manager_FoodStats.Instance.GetFoodStats(orderServed.m_foodType);

        Stand stand = Manager_Stand.Instance.GetStand(orderServed.m_foodType);

        IdleNumber gains_IdleNumber = CalculateGains(foodStats, stand);

        GainMoney(gains_IdleNumber);

        OnMoneyGainCreateFx?.Invoke(fxSpawnPosition, gains_IdleNumber);
    }

    public IdleNumber CalculateGains(FoodStats foodStats, Stand stand)
    {
        IdleNumber gains = foodStats.EvaluateGain(stand.Level)
            * Manager_MoneyMultiplier.Instance.GetFoodMultiplier(foodStats.m_foodType)
            * Manager_MoneyMultiplier.Instance.GetGlobalMoneyMultiplier()
            * Manager_MoneyMultiplier.Instance.GetGlobalMoneyTemporaryMultiplier()
            * Manager_MoneyMultiplier.Instance.GetEquipmentMoneyMultiplier();

        return gains;
    }


    public bool HasEnoughMoney(IdleNumber amount_IdleNumber)
    {
        IdleNumber result = m_money_IdleNumber - amount_IdleNumber;
        return result.m_value > 0;
    }

    public void GainMoney(IdleNumber amount_IdleNumber)
    {
        m_money_IdleNumber += amount_IdleNumber;
        OnUpdateMoney?.Invoke();
        SaveMoney();
    }

    public void SpendMoney(IdleNumber amount_IdleNumber)
    {
        if (HasEnoughMoney(amount_IdleNumber))
        {
            m_money_IdleNumber -= amount_IdleNumber;
            OnUpdateMoney?.Invoke();
            SaveMoney();
        }
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetString(m_moneyValueKey, m_money_IdleNumber.m_value.ToString());
        PlayerPrefs.SetInt(m_moneyExpKey, m_money_IdleNumber.m_exp);
    }
}
