using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Manager_GlobalUpgradesUI : Game_UI
{
    public static System.Action<GameObject, GlobalUpgrade> OnButtonActivated;


    [SerializeField]
    private GameObject m_globalUpgradeSlotPrefab = null;

    [SerializeField]
    private Transform m_globalUpgradeButtonParent = null;


    private List<GameObject> m_globalUpgradeButtonList = new List<GameObject>();
    private GameObject m_instantiatedGlobalUpgradeSlot;


    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_GlobalUpgrades.OnInitializeGlobalUpgrades += OnInitializeGlobalUpgrades;
        GlobalUpgrade_DisplayGlobalUpgradesMenuButton.OnButtonPressed_DisplayGlobalUpgradesMenuUI += OnButtonPressed_DisplayGlobalUpgradesMenuUI;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_GlobalUpgrades.OnInitializeGlobalUpgrades -= OnInitializeGlobalUpgrades;
        GlobalUpgrade_DisplayGlobalUpgradesMenuButton.OnButtonPressed_DisplayGlobalUpgradesMenuUI -= OnButtonPressed_DisplayGlobalUpgradesMenuUI;
    }

    private void Start()
    {
        ToggleUI(false);
    }

    private void OnButtonPressed_DisplayGlobalUpgradesMenuUI()
    {
        ShowUI();
    }

    private void OnInitializeGlobalUpgrades()
    {
        for (int i = 0; i < Manager_GlobalUpgrades.Instance.m_purchasableGlobalUpgradesList.Count; i++)
        {
            m_instantiatedGlobalUpgradeSlot = Instantiate(m_globalUpgradeSlotPrefab, m_globalUpgradeButtonParent);

            m_globalUpgradeButtonList.Add(m_instantiatedGlobalUpgradeSlot);
        }
    }

    private void ShowUI()
    {
        if (IsUIOpen)
            return;

        HideUIArea.onClickHideUIArea += HideUI;

        OpenUI();

        for (int i = 0; i < Manager_GlobalUpgrades.Instance.m_purchasableGlobalUpgradesList.Count; i++)
            OnButtonActivated?.Invoke(m_globalUpgradeButtonList[i], Manager_GlobalUpgrades.Instance.m_purchasableGlobalUpgradesList[i].m_globalUpgradeData.m_globalUpgrade);
    }

    private void HideUI()
    {
        HideUIArea.onClickHideUIArea -= HideUI;

        CloseUI();
    }

    private void OnDestroy()
    {
        HideUIArea.onClickHideUIArea -= HideUI;
    }

}
