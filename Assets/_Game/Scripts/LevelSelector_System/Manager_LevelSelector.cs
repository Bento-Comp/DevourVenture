using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Manager_LevelSelector : MonoBehaviour
{
    public static System.Action<IdleNumber> OnSendNextLevelCost;
    public static System.Action<bool> OnNewLevelIsAffordable;
    public static System.Action<bool> OnUpdateNextLevelConditions;
    public static System.Action OnAllLevelsCompleted;
    public static System.Action OnChangeLevel;


    [SerializeField]
    private int m_levelIndexToLoad = 0;

    [SerializeField]
    private IdleNumber m_goToNextLevelCost_IdleNumber = null;

    [SerializeField]
    private float m_delayBeforeLoadingNextLevelInNextCity = 4f;

    [SerializeField]
    private int m_firstLevelIndex = 1;

    public IdleNumber GoToNextLevelCost_IdleNumber { get => m_goToNextLevelCost_IdleNumber; }


    private void OnEnable()
    {
        Manager_LevelSelectorUI.OnButtonPress_TryToGoToNextLevel += OnButtonPress_TryToGoToNextLevel;
        Manager_Money.OnUpdateMoney += OnUpdateNextLevelAffordableCondition;
        LevelSelector_LevelSelectionButton.OnButtonPressed_ShowLevelSelectionUI += UpdateNextLevelConditions;
        LevelSelector_Button_Debug_NextLevel.OnPress_DebugNextLevelButton += OnPress_DebugNextLevelButton;

    }

    private void OnDisable()
    {
        Manager_LevelSelectorUI.OnButtonPress_TryToGoToNextLevel -= OnButtonPress_TryToGoToNextLevel;
        Manager_Money.OnUpdateMoney -= OnUpdateNextLevelAffordableCondition;
        LevelSelector_LevelSelectionButton.OnButtonPressed_ShowLevelSelectionUI -= UpdateNextLevelConditions;
        LevelSelector_Button_Debug_NextLevel.OnPress_DebugNextLevelButton -= OnPress_DebugNextLevelButton;
    }

    private void Start()
    {
        OnUpdateNextLevelAffordableCondition();
    }

    private void SendNextLevelCost()
    {
        OnSendNextLevelCost?.Invoke(m_goToNextLevelCost_IdleNumber);
    }

    private void UpdateNextLevelConditions()
    {
        if (Manager_WinCondition.Instance == null)
            return;

        SendNextLevelCost();

        OnUpdateNextLevelConditions?.Invoke(Manager_WinCondition.Instance.CanGoToNextLevel);
    }

    private void OnUpdateNextLevelAffordableCondition()
    {
        SendNextLevelCost();

        if (Manager_Money.Instance.HasEnoughMoney(m_goToNextLevelCost_IdleNumber) && Manager_WinCondition.Instance.CanGoToNextLevel)
            OnNewLevelIsAffordable?.Invoke(true);
        else
            OnNewLevelIsAffordable?.Invoke(false);
    }


    private void OnButtonPress_TryToGoToNextLevel()
    {
        if (!Manager_WinCondition.Instance.CanGoToNextLevel)
            return;

        if (!Manager_Money.Instance.HasEnoughMoney(m_goToNextLevelCost_IdleNumber))
            return;

        FireLoadNextLevel();
    }

    private void OnPress_DebugNextLevelButton()
    {
        FireLoadNextLevel();
    }

    private void FireLoadNextLevel()
    {
        if (m_levelIndexToLoad == m_firstLevelIndex)
        {
            OnAllLevelsCompleted?.Invoke();
            Invoke("LoadFirstLevel", m_delayBeforeLoadingNextLevelInNextCity);
            return;
        }

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        OnChangeLevel?.Invoke();
        SceneManager.LoadScene(m_levelIndexToLoad);
    }


    private void LoadFirstLevel()
    {
        OnChangeLevel?.Invoke();
        SceneManager.LoadScene(m_firstLevelIndex);
    }

}
