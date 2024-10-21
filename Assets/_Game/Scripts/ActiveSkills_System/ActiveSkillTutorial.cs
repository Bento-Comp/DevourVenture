using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject m_activeSkillUI = null;

    [SerializeField]
    private GameObject m_arrowUI = null;

    [SerializeField]
    private bool m_isTutorial = false;

    public static readonly string m_hasTutorialEnded = "HasActiveSkillTutorialEnded";

    private void OnEnable()
    {
        if (m_isTutorial)
        {
            ActiveSkillsMenuDisplayer_Button.OnDisplayActiveSkillsMenuButtonPressed_Global += OnDisplayActiveSkillsMenuButtonPressed_Global;

            Manager_Stand.OnAllStandsUnlocked += OnAllStandsUnlocked;
        }
    }

    private void OnDisable()
    {
        if (m_isTutorial)
        {
            ActiveSkillsMenuDisplayer_Button.OnDisplayActiveSkillsMenuButtonPressed_Global -= OnDisplayActiveSkillsMenuButtonPressed_Global;

            Manager_Stand.OnAllStandsUnlocked -= OnAllStandsUnlocked;
        }
    }

    private void Start()
    {
        HideTutorial();
    }


    private void OnAllStandsUnlocked()
    {
        if (!PlayerPrefs.HasKey(m_hasTutorialEnded))
        {
            int hasActiveSkillTutorialEnded = PlayerPrefs.GetInt(m_hasTutorialEnded);

            if (hasActiveSkillTutorialEnded == 0)
                m_arrowUI.SetActive(true);
        }
    }

    private void OnDisplayActiveSkillsMenuButtonPressed_Global()
    {
        PlayerPrefs.SetInt(m_hasTutorialEnded, 1);
        HideTutorial();
    }

    private void HideTutorial()
    {
        m_arrowUI.SetActive(false);
    }
}
