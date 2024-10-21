using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GizmosDrawer : MonoBehaviour
{
    [Header("Draw Line")]
    [SerializeField]
    private GameObject m_startPosition = null;

    [SerializeField]
    private GameObject m_endPosition = null;

    [SerializeField]
    private Vector3 m_lineOffset = Vector3.zero;

    [SerializeField]
    private Color m_gizmosColor = Color.white;


    private void OnDrawGizmos()
    {
        Gizmos.color = m_gizmosColor;

        if (m_startPosition != null && m_endPosition != null)
            Gizmos.DrawLine(m_lineOffset + m_startPosition.transform.position, m_lineOffset + m_endPosition.transform.position);
    }
}
