using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PurchasableGlobalUpgrade : System.IComparable<PurchasableGlobalUpgrade>
{
    public GlobalUpgrade_ScriptableObject m_globalUpgradeData;
    private bool isPurchased;

    public bool IsPurchased { get => isPurchased; set => isPurchased = value; }

    public int CompareTo(PurchasableGlobalUpgrade other)
    {
        // A null value means that this object is greater.
        if (other == null)
            return 1;
        else
            return ComparePurhcasableGlobalUpgrades(this, other);
    }


    public int ComparePurhcasableGlobalUpgrades(PurchasableGlobalUpgrade a, PurchasableGlobalUpgrade b)
    {
        return IdleNumber.CompareIdleNumbers(a.m_globalUpgradeData.m_globalUpgrade.m_cost_IdleNumber, b.m_globalUpgradeData.m_globalUpgrade.m_cost_IdleNumber);
    }
}

[DefaultExecutionOrder(1)]  //needs to wait for other scripts to initialize (like lists) before sending data to them
public class Manager_GlobalUpgrades : UniSingleton.Singleton<Manager_GlobalUpgrades>
{
    public static System.Action OnInitializeGlobalUpgrades;
    public static System.Action OnDataLoaded;
    public static System.Action<GameObject> OnGlobalUpgradePurchased;
    public static System.Action<GlobalUpgrade> OnGlobalUpgradeAquired;
    public static System.Action<bool> OnNewUpgradeAffordable;

    public List<PurchasableGlobalUpgrade> m_purchasableGlobalUpgradesList = null;

    [SerializeField]
    private string m_scriptableObjectsDataPath = "ScriptableObjects/GlobalUpgradesData/Level 1";

    private PurchasableGlobalUpgrade m_purchasableGlobalUpgradeBuffer;


    protected override void OnSingletonEnable()
    {
        base.OnSingletonEnable();
        GlobalUpgradeSlot.OnButtonPress_TryToMakeUpgrade += OnButtonPress_TryToMakeUpgrade;
        Manager_Money.OnUpdateMoney += OnUpdateGlobalUpgradeAffordableCondition;

        Manager_LevelSelector.OnChangeLevel += ClearLevelSave;
    }

    private void OnDisable()
    {
        GlobalUpgradeSlot.OnButtonPress_TryToMakeUpgrade -= OnButtonPress_TryToMakeUpgrade;
        Manager_Money.OnUpdateMoney -= OnUpdateGlobalUpgradeAffordableCondition;

        Manager_LevelSelector.OnChangeLevel -= ClearLevelSave;
    }

    private void Start()
    {
        LoadData();

        OnInitializeGlobalUpgrades?.Invoke();

        OnUpdateGlobalUpgradeAffordableCondition();
    }

    public void SortListByCost()
    {
        m_purchasableGlobalUpgradesList.Sort();
    }

    public void LoadScriptableObjectsData()
    {
        Object[] globalUpgradesSO_Objects = Resources.LoadAll(m_scriptableObjectsDataPath, typeof(GlobalUpgrade_ScriptableObject));

        GlobalUpgrade_ScriptableObject[] globalUpgrade_SOArray = new GlobalUpgrade_ScriptableObject[globalUpgradesSO_Objects.Length];
        globalUpgradesSO_Objects.CopyTo(globalUpgrade_SOArray, 0);


        m_purchasableGlobalUpgradesList = new List<PurchasableGlobalUpgrade>();

        for (int i = 0; i < globalUpgrade_SOArray.Length; i++)
        {
            PurchasableGlobalUpgrade purchasableGlobalUpgrade = new PurchasableGlobalUpgrade();
            purchasableGlobalUpgrade.m_globalUpgradeData = globalUpgrade_SOArray[i];

            m_purchasableGlobalUpgradesList.Add(purchasableGlobalUpgrade);
        }
    }

    private void OnUpdateGlobalUpgradeAffordableCondition()
    {
        if (HasUpgradesAvailableAndPurchasable())
            OnNewUpgradeAffordable?.Invoke(true);
        else
            OnNewUpgradeAffordable?.Invoke(false);
    }

    private void LoadData()
    {
        for (int i = 0; i < m_purchasableGlobalUpgradesList.Count; i++)
        {
            string globalUpgradePlayerPrefKey = GetPlayerPrefKey(i);

            if (PlayerPrefs.HasKey(globalUpgradePlayerPrefKey))
            {
                m_purchasableGlobalUpgradesList[i].IsPurchased = PlayerPrefs.GetInt(globalUpgradePlayerPrefKey) == 0 ? false : true;

                if (m_purchasableGlobalUpgradesList[i].IsPurchased)
                {
                    OnGlobalUpgradeAquired?.Invoke(m_purchasableGlobalUpgradesList[i].m_globalUpgradeData.m_globalUpgrade);
                }
            }
            else
            {
                m_purchasableGlobalUpgradesList[i].IsPurchased = false;
                SaveToPlayerPrefs_GlobalUpgrade(i, globalUpgradePlayerPrefKey);
            }
        }

        OnDataLoaded?.Invoke();
    }

    private void ClearLevelSave()
    {
        for (int i = 0; i < m_purchasableGlobalUpgradesList.Count; i++)
        {
            string globalUpgradePlayerPrefKey = GetPlayerPrefKey(i);

            if (PlayerPrefs.HasKey(globalUpgradePlayerPrefKey))
                PlayerPrefs.DeleteKey(globalUpgradePlayerPrefKey);
        }
    }

    private void SaveToPlayerPrefs_GlobalUpgrade(int i, string globalUpgradePlayerPrefKey)
    {
        PlayerPrefs.SetInt(globalUpgradePlayerPrefKey, m_purchasableGlobalUpgradesList[i].IsPurchased == false ? 0 : 1);
    }


    private string GetPlayerPrefKey(int upgradeIndex)
    {
        return Manager_SceneManagement.LevelName + upgradeIndex.ToString();
    }


    private int GetGlobalUpgradeIndexInList(GlobalUpgrade globalUpgrade)
    {
        for (int i = 0; i < m_purchasableGlobalUpgradesList.Count; i++)
        {
            if (globalUpgrade == m_purchasableGlobalUpgradesList[i].m_globalUpgradeData.m_globalUpgrade)
            {
                return i;
            }
        }

        Debug.LogError("Could find global upgrade index in list");
        return -1;
    }


    private void OnButtonPress_TryToMakeUpgrade(GameObject buttonReference, GlobalUpgrade globalUpgrade)
    {
        m_purchasableGlobalUpgradeBuffer = GetPurchasableGlobalUpgrade(globalUpgrade);

        if (m_purchasableGlobalUpgradeBuffer != null && !m_purchasableGlobalUpgradeBuffer.IsPurchased)
        {
            if (Manager_Money.Instance.HasEnoughMoney(globalUpgrade.m_cost_IdleNumber))
            {
                Manager_Money.Instance.SpendMoney(globalUpgrade.m_cost_IdleNumber);
                m_purchasableGlobalUpgradeBuffer.IsPurchased = true;


                int globalUpgradeIndex = GetGlobalUpgradeIndexInList(m_purchasableGlobalUpgradeBuffer.m_globalUpgradeData.m_globalUpgrade);

                if (globalUpgradeIndex > -1 && globalUpgradeIndex < m_purchasableGlobalUpgradesList.Count)
                {
                    string globalUpgradePlayerPrefKey = GetPlayerPrefKey(globalUpgradeIndex);
                    SaveToPlayerPrefs_GlobalUpgrade(globalUpgradeIndex, globalUpgradePlayerPrefKey);
                }
                else
                {
                    Debug.LogError("Could not save data");
                }

                OnGlobalUpgradeAquired?.Invoke(globalUpgrade);
                OnGlobalUpgradePurchased?.Invoke(buttonReference);
                OnUpdateGlobalUpgradeAffordableCondition();
            }
        }
    }

    public bool HasUpgradesAvailableAndPurchasable()
    {
        for (int i = 0; i < m_purchasableGlobalUpgradesList.Count; i++)
        {
            if (!m_purchasableGlobalUpgradesList[i].IsPurchased && Manager_Money.Instance.HasEnoughMoney(m_purchasableGlobalUpgradesList[i].m_globalUpgradeData.m_globalUpgrade.m_cost_IdleNumber))
            {
                return true;
            }
        }

        return false;
    }

    public PurchasableGlobalUpgrade GetPurchasableGlobalUpgrade(GlobalUpgrade globalUpgrade)
    {
        for (int i = 0; i < m_purchasableGlobalUpgradesList.Count; i++)
        {
            if (globalUpgrade == m_purchasableGlobalUpgradesList[i].m_globalUpgradeData.m_globalUpgrade)
                return m_purchasableGlobalUpgradesList[i];
        }

        return null;
    }
}
