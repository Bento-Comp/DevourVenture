using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFx : MonoBehaviour
{
    [SerializeField]
    private Employee m_employee = null;

    [SerializeField]
    private ParticleSystem m_sleepFx = null;

    [SerializeField]
    private float m_fxActivationDelay = 0.5f;

    private float m_timer;
    private bool m_isWorkerActive;
    private bool m_isFxActive;

    public Employee Employee { get => m_employee; set => m_employee = value; }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        m_employee.OnEmployeeInAction -= OnWorkerActive;
        m_employee.OnEmployeeNotInAction -= OnWorkerNotActive;
    }

    private void Start()
    {
        m_sleepFx.Stop();
    }

    public void Initialize()
    {
        if (m_employee == null)
            return;

        m_employee.OnEmployeeInAction += OnWorkerActive;
        m_employee.OnEmployeeNotInAction += OnWorkerNotActive;
    }

    private void Update()
    {
        if (!m_isWorkerActive && !m_isFxActive)
        {
            m_timer += Time.deltaTime;

            if (m_timer > m_fxActivationDelay)
            {
                m_isFxActive = true;
                m_sleepFx.Play();
            }
        }
    }


    private void OnWorkerActive()
    {
        m_isWorkerActive = true;
        m_isFxActive = false;
        m_sleepFx.Stop();
        m_timer = 0f;
    }

    private void OnWorkerNotActive()
    {
        m_isWorkerActive = false;
    }

}
