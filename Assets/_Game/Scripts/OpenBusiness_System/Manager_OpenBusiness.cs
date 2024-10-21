using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_OpenBusiness : MonoBehaviour
{
    public static System.Action OnBusinessStarted;


    private void OnEnable()
    {
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += OnEnterLevelForTheFirstTime;
        Manager_OpenBusinessUI.OnOpenBusinessButtonPressed += OnOpenBusinessButtonPressed;
    }

    private void OnDisable()
    {
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= OnEnterLevelForTheFirstTime;
        Manager_OpenBusinessUI.OnOpenBusinessButtonPressed -= OnOpenBusinessButtonPressed;
    }


    private void OnEnterLevelForTheFirstTime()
    {
        IsBusinessOpen = false;
    }

    private void OnOpenBusinessButtonPressed()
    {
        IsBusinessOpen = true;
        OnBusinessStarted?.Invoke();
    }


    static readonly string businessOpen_saveKey = "BusinessOpen";

    public static bool IsBusinessOpen
    {
        get => PlayerPrefs.GetInt(businessOpen_saveKey, 0) == 1;

        set => PlayerPrefs.SetInt(businessOpen_saveKey, value ? 1 : 0);
    }
}
