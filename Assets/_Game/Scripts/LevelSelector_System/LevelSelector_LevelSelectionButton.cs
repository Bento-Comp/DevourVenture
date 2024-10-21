using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector_LevelSelectionButton : Game_UI
{
    public static System.Action OnButtonPressed_ShowLevelSelectionUI;


    [SerializeField]
    private GameObject m_upgradeArrow = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_LevelSelector.OnNewLevelIsAffordable += OnNewLevelIsAffordable;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted += OnBusinessStarted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_LevelSelector.OnNewLevelIsAffordable -= OnNewLevelIsAffordable;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
        Manager_OpenBusiness.OnBusinessStarted -= OnBusinessStarted;
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
    public void ShowLevelSelectionUI()
    {
        OnButtonPressed_ShowLevelSelectionUI?.Invoke();
    }

    private void OnNewLevelIsAffordable(bool isNewLevelAffordable)
    {
        m_upgradeArrow.SetActive(isNewLevelAffordable);
    }
}
