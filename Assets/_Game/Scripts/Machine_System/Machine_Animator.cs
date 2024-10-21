using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine_Animator : MonoBehaviour
{
    [SerializeField]
    private Machine m_machineReference = null;

    [SerializeField]
    private Animator m_animator = null;


    private void OnEnable()
    {
        Worker.OnStartUsingMachine += OnStartUsingMachine;
        Worker.OnStopUsingMachine += OnStopUsingMachine;
    }

    private void OnDisable()
    {
        Worker.OnStartUsingMachine -= OnStartUsingMachine;
        Worker.OnStopUsingMachine -= OnStopUsingMachine;
    }


    private void OnStartUsingMachine(Machine machineReference)
    {
        if (machineReference == m_machineReference)
        {
            m_animator.SetBool("IsUsed", true);
        }
    }

    private void OnStopUsingMachine(Machine machineReference)
    {
        if (machineReference == m_machineReference)
        {
            m_animator.SetBool("IsUsed", false);
        }
    }
}
