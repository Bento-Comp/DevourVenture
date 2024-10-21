using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderFromSave : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("LevelIndex"))
        {
            int levelIndex = PlayerPrefs.GetInt("LevelIndex");
            Debug.Log("Loading scene " + levelIndex);
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.Log("Starting fresh game");
            SceneManager.LoadScene(1);
        }
    }

}
