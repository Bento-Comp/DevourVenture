using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_positionList = null;

    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private float m_speed = 5f;

    [SerializeField]
    private float m_waitTime = 0f;

    [SerializeField]
    private int m_startIndex = 0;


    private Vector3 m_desiredPosition;
    private int m_currentIndex;
    private bool m_isMovementEnabled;

    private void Start()
    {
        if (m_positionList.Count > 0)
            m_currentIndex = m_startIndex;

        m_desiredPosition = m_positionList[m_currentIndex].transform.position;
        m_isMovementEnabled = true;
    }



    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        if (m_isMovementEnabled)
        {
            m_controlledTransform.position = Vector3.MoveTowards(m_controlledTransform.position, m_desiredPosition, m_speed * Time.deltaTime);

            if (Vector3.Distance(m_controlledTransform.position, m_desiredPosition) < 0.05f)
            {
                Invoke("SetupNextDestination", m_waitTime);
                m_isMovementEnabled = false;
            }
        }
    }

    private void SetupNextDestination()
    {
        m_isMovementEnabled = true;
        m_currentIndex++;
        m_currentIndex = m_currentIndex % m_positionList.Count;

        m_desiredPosition = m_positionList[m_currentIndex].transform.position;
    }


}
