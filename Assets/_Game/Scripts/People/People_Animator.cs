using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People_Animator : MonoBehaviour
{
    [SerializeField]
    private People_ModelSelector m_peopleModelSelector = null;

    [SerializeField]
    private Animator m_defaultAnimator = null;

    [SerializeField]
    private GridNavigationAgent m_navigation = null;

    private Animator m_animator;



    private void OnEnable()
    {
        if(m_peopleModelSelector != null)
        m_peopleModelSelector.OnModelSelected += OnModelSelected;

        if (m_defaultAnimator != null)
            m_animator = m_defaultAnimator;

        m_navigation.OnStartMoving += PlayWalking;
        m_navigation.OnStopMoving += PlayIdle;
    }

    private void OnDisable()
    {
        if (m_peopleModelSelector != null)
            m_peopleModelSelector.OnModelSelected -= OnModelSelected;

        m_navigation.OnStartMoving -= PlayWalking;
        m_navigation.OnStopMoving -= PlayIdle;
    }


    private void OnModelSelected(Animator animator)
    {
        m_animator = animator;
    }


    private void PlayWalking()
    {
        if (m_animator != null)
            m_animator.SetBool("IsMoving", true);
    }

    private void PlayIdle()
    {
        if (m_animator != null)
            m_animator.SetBool("IsMoving", false);
    }
}
