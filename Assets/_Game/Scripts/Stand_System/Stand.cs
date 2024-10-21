using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[DefaultExecutionOrder(1)]
public class Stand : MonoBehaviour
{
    public enum State
    {
        Active,
        NotActive
    }

    public static System.Action<Stand, Vector3> OnStandSelected;
    public static System.Action<Stand.State> OnShowUI;
    public static System.Action<Machine, Machine.State> OnToggleMachineState;
    public static System.Action OnHideUI;
    public static System.Action OnUpdateLevel;
    public static System.Action OnStandMaxedOut;
    public static System.Action<bool> OnStandRankUp;
    public static System.Action OnMachineUnlocked;
    public static System.Action<FoodType, float> OnIncreaseGainMultipler;
    public static System.Action<FoodType, float> OnImproveProductionTime;
    public static System.Action<Stand> OnStandUnlocked;
    public System.Action<Stand.State> OnUpdateState;
    public System.Action OnUpgradePurchased;


    [SerializeField]
    private List<Machine> m_machineToManageList = null;

    [SerializeField]
    private Collider m_interactableCollider = null;

    [SerializeField]
    private FoodType m_standFoodType = FoodType.Lemonade;


    private Stand.State m_state;
    private FoodStats m_currentFoodStats;
    private string m_standStatePlayerPrefsKey;
    private string m_standLevelPlayerPrefsKey;
    private float m_prodcutionTime;
    private IdleNumber m_upgradeCost_IdleNumber;
    private IdleNumber m_gainAmount_IdleNumber;
    private IdleNumber m_unlockCost_IdleNumber;
    private int m_level;
    private int m_rank;


    public FoodStats CurrentFoodStats { get => m_currentFoodStats; }
    public FoodType StandFoodType { get => m_standFoodType; }
    public float ProdcutionTime { get => m_prodcutionTime; }
    public IdleNumber GainAmount_IdleNumber { get => m_gainAmount_IdleNumber; }
    public IdleNumber UpgradeCost_IdleNumber { get => m_upgradeCost_IdleNumber; }
    public IdleNumber UnlockCost_IdleNumber { get => m_unlockCost_IdleNumber; }
    public int Level { get => m_level; }
    public int Rank { get => m_rank; }
    public State State1 { get => m_state; }
    public bool IsStandMaxed { get => m_level >= m_currentFoodStats.m_maxLevel; }

    private void OnEnable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem += OnHitInteractableItem;

        Manager_Stand_UnlockUI.OnButtonPress_TryUnlock += OnButtonPress_TryUnlock;
        Manager_Stand_UpgradeUI.OnButtonPress_TryUpgrade += OnButtonPress_TryUpgrade;

        Machine.OnMachineSelected += OnMachineSelected;

        Manager_LevelSelector.OnChangeLevel += ClearLevelSave;
    }


    private void OnDisable()
    {
        Manager_RaycastFromScreen.OnHitInteractableItem -= OnHitInteractableItem;

        Manager_Stand_UnlockUI.OnButtonPress_TryUnlock -= OnButtonPress_TryUnlock;
        Manager_Stand_UpgradeUI.OnButtonPress_TryUpgrade -= OnButtonPress_TryUpgrade;

        Machine.OnMachineSelected -= OnMachineSelected;

        Manager_LevelSelector.OnChangeLevel -= ClearLevelSave;
    }


    private void Start()
    {
        m_standStatePlayerPrefsKey = Manager_SceneManagement.LevelName + m_standFoodType.ToString() + "State";
        m_standLevelPlayerPrefsKey = Manager_SceneManagement.LevelName + m_standFoodType.ToString() + "Level";

        LoadData();
        Initialize();
    }


    private void LoadData()
    {
        LoadStandState();
        LoadStandLevel();
    }

    private void ClearLevelSave()
    {
        if (PlayerPrefs.HasKey(m_standStatePlayerPrefsKey))
            PlayerPrefs.DeleteKey(m_standStatePlayerPrefsKey);

        if (PlayerPrefs.HasKey(m_standLevelPlayerPrefsKey))
            PlayerPrefs.DeleteKey(m_standLevelPlayerPrefsKey);
    }

    private void LoadStandState()
    {
        if (PlayerPrefs.HasKey(m_standStatePlayerPrefsKey))
        {
            m_state = PlayerPrefs.GetInt(m_standStatePlayerPrefsKey) == 0 ? State.NotActive : State.Active;

            if (m_state == State.Active)
            {
                OnStandUnlocked?.Invoke(this);

                if (m_machineToManageList.Count > 0)
                    OnToggleMachineState?.Invoke(m_machineToManageList[0], Machine.State.Active);
            }
            else
                if (m_machineToManageList.Count > 0)
                OnToggleMachineState?.Invoke(m_machineToManageList[0], Machine.State.NotActive);
        }
        else
        {
            m_state = State.NotActive;
            SaveToPlayerPrefs_State();

            if (m_machineToManageList.Count > 0)
                OnToggleMachineState?.Invoke(m_machineToManageList[0], Machine.State.NotActive);
        }

        OnUpdateState?.Invoke(m_state);
    }

    private void SaveToPlayerPrefs_State()
    {
        PlayerPrefs.SetInt(m_standStatePlayerPrefsKey, m_state == State.NotActive ? 0 : 1);
    }

    private void LoadStandLevel()
    {
        if (PlayerPrefs.HasKey(m_standLevelPlayerPrefsKey))
        {
            m_level = PlayerPrefs.GetInt(m_standLevelPlayerPrefsKey);
        }
        else
        {
            m_level = 1;
            SaveToPlayerPrefs_Level();
        }
    }

    private void SaveToPlayerPrefs_Level()
    {
        PlayerPrefs.SetInt(m_standLevelPlayerPrefsKey, m_level);
    }


    private void Initialize()
    {
        for (int i = 1; i < m_machineToManageList.Count; i++)
            OnToggleMachineState?.Invoke(m_machineToManageList[i], Machine.State.NotActive);


        m_rank = 0;

        if (Manager_FoodStats.Instance == null)
            return;

        m_currentFoodStats = Manager_FoodStats.Instance.GetFoodStats(m_standFoodType);

        if (m_currentFoodStats == null)
            return;

        for (int i = 0; i < m_currentFoodStats.m_standRankBonusList.Count; i++)
        {
            CheckForRankBonus(true);
        }

        m_prodcutionTime = m_currentFoodStats.m_productionTime;
        m_gainAmount_IdleNumber = m_currentFoodStats.EvaluateGain(m_level);
        m_upgradeCost_IdleNumber = m_currentFoodStats.EvaluateUpgradeCost(m_level);
        m_unlockCost_IdleNumber = new IdleNumber(m_currentFoodStats.m_unlockCost_IdleNumber);
    }


    private void OnButtonPress_TryUnlock(Stand standReference)
    {
        if (standReference == this)
        {
            if (Manager_Money.Instance.HasEnoughMoney(m_unlockCost_IdleNumber))
            {
                m_state = State.Active;
                OnUpdateState?.Invoke(m_state);
                SaveToPlayerPrefs_State();
                Manager_Money.Instance.SpendMoney(m_unlockCost_IdleNumber);

                OnStandUnlocked?.Invoke(this);
                OnToggleMachineState?.Invoke(m_machineToManageList[0], Machine.State.ToUnlock);
            }
        }
    }


    private void OnButtonPress_TryUpgrade(Stand standReference)
    {
        if (standReference == this)
        {
            if (Manager_Money.Instance.HasEnoughMoney(m_upgradeCost_IdleNumber))
            {
                IdleNumber upgradeToPay_Idlenumber = new IdleNumber(m_upgradeCost_IdleNumber);

                if (m_level < m_currentFoodStats.m_maxLevel)
                {
                    m_level++;
                    SaveToPlayerPrefs_Level();

                    m_gainAmount_IdleNumber = m_currentFoodStats.EvaluateGain(m_level);
                    m_upgradeCost_IdleNumber = m_currentFoodStats.EvaluateUpgradeCost(m_level);

                    Manager_Money.Instance.SpendMoney(upgradeToPay_Idlenumber);

                    CheckForRankBonus(false);

                    if (m_level == m_currentFoodStats.m_maxLevel)
                    {
                        OnStandMaxedOut?.Invoke();
                        m_upgradeCost_IdleNumber.m_value = -1;
                    }

                    OnUpdateLevel?.Invoke();
                    OnUpgradePurchased?.Invoke();
                }
                else
                {
                    //security
                    m_upgradeCost_IdleNumber.m_value = -1;
                }

            }
        }
    }


    private void CheckForRankBonus(bool isLoadingData)
    {
        List<Bonus> rankBonus = Manager_FoodStats.Instance.TryGetRankBonus(m_standFoodType, m_rank, m_level);

        if (rankBonus.Count > 0)
        {
            m_rank++;

            OnStandRankUp?.Invoke(isLoadingData);

            for (int i = 0; i < rankBonus.Count; i++)
            {
                if (rankBonus[i] != Bonus.None)
                {
                    switch (rankBonus[i])
                    {
                        case Bonus.AdditionalMachine:
                            TryUnlockNewMachine(isLoadingData);
                            break;
                        case Bonus.FoodGainMultiplier:
                            OnIncreaseGainMultipler?.Invoke(m_standFoodType, 2f); //multiplied by 2
                            break;
                        case Bonus.ProductionTimeBoost:
                            OnImproveProductionTime?.Invoke(m_standFoodType, 1f); //increased by 1 (+100%)
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void TryUnlockNewMachine(bool isLoadingData)
    {
        for (int i = 0; i < m_machineToManageList.Count; i++)
        {
            if (m_machineToManageList[i].State1 == Machine.State.NotActive)
            {
                OnToggleMachineState(m_machineToManageList[i], isLoadingData ? Machine.State.Active : Machine.State.ToUnlock);
                OnMachineUnlocked?.Invoke();
                return;
            }
        }
    }

    private void OnMachineSelected(Machine machineReference)
    {
        for (int i = 0; i < m_machineToManageList.Count; i++)
        {
            if (m_machineToManageList[i] == machineReference)
            {
                OnStandSelected?.Invoke(this, m_interactableCollider.transform.position);
                ShowUI();
                return;
            }
        }
    }

    private void OnHitInteractableItem(Collider collider)
    {
        if (collider == m_interactableCollider)
        {
            OnStandSelected?.Invoke(this, m_interactableCollider.transform.position);
            ShowUI();
        }
    }

    private void ShowUI()
    {
        OnShowUI?.Invoke(m_state);
    }

}
