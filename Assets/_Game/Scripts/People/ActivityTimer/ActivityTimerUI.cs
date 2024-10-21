using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActivityTimerUI : MonoBehaviour
{
    [SerializeField]
    private Employee m_employee = null;

    [SerializeField]
    private Image m_progressionImage = null;

    [SerializeField]
    private GameObject m_timerUIObject = null;

    private float m_progression;
    private float m_activityDuration;
    private bool m_isTimerRunning;

    public Employee Employee { get => m_employee; set => m_employee = value; }


    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        m_employee.OnStartActivityTimer -= OnStartTimer;
        m_employee.OnStopActivityTimer -= OnStopTimer;
    }

    public void Initialize()
    {
        if (m_employee == null)
            return;

        m_employee.OnStartActivityTimer += OnStartTimer;
        m_employee.OnStopActivityTimer += OnStopTimer;
    }


    private void Start()
    {
        m_timerUIObject.SetActive(false);
    }

    private void Update()
    {
        if (m_isTimerRunning)
        {
            m_progression = m_employee.Timer / m_activityDuration;
            m_progressionImage.fillAmount = m_progression;
        }

    }


    private void OnStartTimer(float activityDuration)
    {
        m_isTimerRunning = true;
        m_activityDuration = activityDuration;
        m_timerUIObject.SetActive(true);
    }

    private void OnStopTimer()
    {
        m_isTimerRunning = false;
        m_timerUIObject.SetActive(false);
    }

}
