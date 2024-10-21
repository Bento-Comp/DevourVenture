using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand_Tutorial : MonoBehaviour
{
    [SerializeField]
    private Stand m_stand = null;

    [SerializeField]
    private GameObject m_arrowUI = null;

    [SerializeField]
    private bool m_isTutorial = false;


    private void OnEnable()
    {
        m_stand.OnUpdateState += OnUpdateState;
    }

    private void OnDisable()
    {
        m_stand.OnUpdateState -= OnUpdateState;
    }


    private void OnUpdateState(Stand.State state)
    {
        m_arrowUI.SetActive(false);

        m_stand.OnUpdateState -= OnUpdateState;

        if (state == Stand.State.Active)
        {
            m_arrowUI.SetActive(false);
        }
        else if (state == Stand.State.NotActive && m_isTutorial)
        {
            m_arrowUI.SetActive(true);
        }
    }

}
