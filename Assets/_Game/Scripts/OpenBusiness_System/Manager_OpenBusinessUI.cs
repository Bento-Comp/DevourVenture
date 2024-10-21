using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_OpenBusinessUI : Game_UI
{
    public static System.Action OnOpenBusinessButtonPressed;


    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += OnNotEnterLevelForTheFirstTime;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= OnNotEnterLevelForTheFirstTime;
    }

    private void OnNotEnterLevelForTheFirstTime()
    {
        ToggleUI(false);
    }

    private void OnEnterLevelForTheFirstTime()
    {
        ShowUI();
    }

    //Called by button
    public void Button_OpenBusinessPressed()
    {
        Debug.Log("Valentin : Press button open business");
        OnOpenBusinessButtonPressed?.Invoke();
        HideUI();
    }

    private void HideUI()
    {
        CloseUI();
    }

    private void ShowUI()
    {
        OpenUI();
    }
}
