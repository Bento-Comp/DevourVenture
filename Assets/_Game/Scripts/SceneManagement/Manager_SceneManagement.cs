using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(3)]
public class Manager_SceneManagement : MonoBehaviour
{
    public static string LevelName { get => "Level" + SceneManager.GetActiveScene().buildIndex; }

    public static System.Action OnEnterLevelForTheFirstTime;
    public static System.Action OnNotEnterLevelForTheFirstTime;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("LevelIndex"))
        {
            OnEnterLevelForTheFirstTime?.Invoke();
            Manager_Session.StartSession();
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("LevelIndex"))
            {
                OnEnterLevelForTheFirstTime?.Invoke();
                Manager_Session.StartSession();
            }
            else
                OnNotEnterLevelForTheFirstTime?.Invoke();
        }

        PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);
    }
}
