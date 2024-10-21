using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedStateController : MonoBehaviour
{
    public Action OnGrounded;
    public Action OnNotGrounded;


    [SerializeField]
    private GameObject m_raycastStartPosition = null;

    [SerializeField]
    private GameObject m_raycastEndPosition = null;

    [SerializeField]
    private LayerMask m_groundLayer = 0;


    private RaycastHit m_hit;
    public bool m_isGrounded;
    public bool IsGrounded { get => m_isGrounded; }


    private void Update()
    {
        CheckGroundedCondition();
    }


    private void CheckGroundedCondition()
    {
        if (!m_isGrounded)
        {
            if (Physics.Raycast(m_raycastStartPosition.transform.position,
               m_raycastEndPosition.transform.position - m_raycastStartPosition.transform.position,
               out m_hit,
               (m_raycastEndPosition.transform.position - m_raycastStartPosition.transform.position).magnitude, m_groundLayer))
            {
                m_isGrounded = true;
                OnGrounded?.Invoke();
            }
        }
        else
        {
            if (!Physics.Raycast(m_raycastStartPosition.transform.position,
               m_raycastEndPosition.transform.position - m_raycastStartPosition.transform.position,
               out m_hit,
               (m_raycastEndPosition.transform.position - m_raycastStartPosition.transform.position).magnitude, m_groundLayer))
            {
                m_isGrounded = false;
                OnNotGrounded?.Invoke();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_raycastStartPosition.transform.position, m_raycastEndPosition.transform.position);
    }
}
