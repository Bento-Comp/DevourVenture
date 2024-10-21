using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People_AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private GridNavigationAgent m_navigationAgent = null;


    private void OnEnable()
    {
        m_navigationAgent.OnStartMoving += OnStartMoving;
        m_navigationAgent.OnStopMoving += OnStopMoving;
    }

    private void OnDisable()
    {
        m_navigationAgent.OnStartMoving -= OnStartMoving;
        m_navigationAgent.OnStopMoving -= OnStopMoving;
    }


    private void OnStartMoving()
    {
        PlayWalk();
    }

    private void OnStopMoving()
    {
        PlayIdle();
    }

    private void PlayIdle()
    {
        m_animator.SetBool("IsMoving", false);
    }

    private void PlayWalk()
    {
        m_animator.SetBool("IsMoving", true);
    }

}
