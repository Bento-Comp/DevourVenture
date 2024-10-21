using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStartFx : MonoBehaviour
{
    [SerializeField]
    private GameObject m_levelStartFx = null;



    private void Awake()
    {
        m_levelStartFx.SetActive(false);
    }

    private void OnEnable()
    {
        Manager_OpenBusiness.OnBusinessStarted += OnEnterLevelForTheFirstTime;
    }

    private void OnDisable()
    {
        Manager_OpenBusiness.OnBusinessStarted -= OnEnterLevelForTheFirstTime;
    }

    private void OnEnterLevelForTheFirstTime()
    {
        m_levelStartFx.SetActive(true);
    }
}
