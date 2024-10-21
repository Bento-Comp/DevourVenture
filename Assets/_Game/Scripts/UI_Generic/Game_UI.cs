using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_UI : MonoBehaviour
{
    private static List<Game_UI> m_openGameUIList = new List<Game_UI>();


    [SerializeField]
    protected GameObject m_ui = null;

    [SerializeField]
    private UI_Generic_Animator_Controller m_animator = null;

    [SerializeField]
    private bool m_isBlockingRaycastFromCamera = true;


    public bool IsUIOpen { get => m_ui.activeInHierarchy; }
    public static bool IsAnyUIOpen { get => m_openGameUIList.Count > 0; }


    protected virtual void OnEnable()
    {
        if (m_animator != null)
            m_animator.OnAnimationEnd_Disapear += OnAnimationEnd_Disapear;

        Manager_LevelSelector.OnChangeLevel += ClearGameUIList;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime += ClearGameUIList;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime += ClearGameUIList;
    }

    protected virtual void OnDisable()
    {
        if (m_animator != null)
            m_animator.OnAnimationEnd_Disapear -= OnAnimationEnd_Disapear;

        Manager_LevelSelector.OnChangeLevel -= ClearGameUIList;
        Manager_SceneManagement.OnEnterLevelForTheFirstTime -= ClearGameUIList;
        Manager_SceneManagement.OnNotEnterLevelForTheFirstTime -= ClearGameUIList;
    }

    private void ClearGameUIList()
    {
        m_openGameUIList.Clear();
    }

    public static void ShowOpenedUI()
    {
        for (int i = 0; i < m_openGameUIList.Count; i++)
        {
            if (m_openGameUIList[i] != null)
                Debug.Log(m_openGameUIList[i], m_openGameUIList[i].gameObject);
            else
                Debug.Log("something missing");
        }
    }


    protected void OpenUI()
    {
        ToggleUI(true);

        if (m_animator != null)
            m_animator.Play_Appear();
    }


    protected void CloseUI()
    {
        if (m_animator != null)
            m_animator.Play_Disapear();
        else
            ToggleUI(false);
    }


    protected virtual void OnAnimationEnd_Disapear()
    {
        ToggleUI(false);
    }

    protected void ToggleUI(bool state)
    {
        m_ui.SetActive(state);

        if (state && m_isBlockingRaycastFromCamera)
        {
            if (m_openGameUIList.Contains(this))
                return;

            m_openGameUIList.Add(this);
            //Debug.Log(this + "added!", this.gameObject);
        }
        else
        {
            if (m_openGameUIList.Contains(this))
                m_openGameUIList.Remove(this);
        }
    }
}
