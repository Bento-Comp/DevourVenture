using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_groundLayer = 0;

    [SerializeField]
    private float m_yLocalOffset = 0f;

    [SerializeField]
    private float m_raycastLength = 0.5f;

    [SerializeField]
    private bool m_isUpdateGroundDataEnabled = true;

    [SerializeField]
    private bool m_isDebugEnabled = false;

    private RaycastHit m_hit;
    private Vector3 m_groundNormal;

    public Vector3 GroundNormal { get => m_groundNormal; }
    public Vector3 GroundPosition { get => m_hit.point; }


    private void FixedUpdate()
    {
        if (m_isUpdateGroundDataEnabled)
            UpdateGroundNormal();
    }


    private void UpdateGroundNormal()
    {
        CalculateGroundNormal();

    }

    public void CalculateGroundNormal()
    {
        if (Physics.Raycast(transform.position + transform.up * m_yLocalOffset, -transform.up, out m_hit, m_raycastLength, m_groundLayer))
        {
            m_groundNormal = m_hit.normal;
        }

        if (m_isDebugEnabled)
        {
            Debug.DrawRay(transform.position + transform.up * m_yLocalOffset, -transform.up * m_raycastLength, Color.blue, 0.1f);
        }

    }
}
