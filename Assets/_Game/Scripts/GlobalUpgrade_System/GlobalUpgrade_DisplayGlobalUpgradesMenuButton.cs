using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpgrade_DisplayGlobalUpgradesMenuButton : Game_UI
{
    public static System.Action OnButtonPressed_DisplayGlobalUpgradesMenuUI;


    [SerializeField]
    private GameObject m_upgradeArrow = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_GlobalUpgrades.OnNewUpgradeAffordable += OnNewUpgradeAffordable;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_GlobalUpgrades.OnNewUpgradeAffordable -= OnNewUpgradeAffordable;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
    }

    private void OnNewUpgradeAffordable(bool isNewGlobalUpgradeAffordable)
    {
        m_upgradeArrow.SetActive(isNewGlobalUpgradeAffordable);
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
    public void ShowGlobalUpgradeMenuUI()
    {
        OnButtonPressed_DisplayGlobalUpgradesMenuUI?.Invoke();
    }

}
