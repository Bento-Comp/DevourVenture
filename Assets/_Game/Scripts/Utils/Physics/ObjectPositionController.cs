using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionController : MonoBehaviour
{
    public Action OnObjectInPosition;


    [SerializeField]
    private GameObject m_objectToMove = null;

    [SerializeField]
    private Vector3 m_positionOffset = Vector3.zero;

    [SerializeField]
    private LayerMask m_groundLayer = 0;

    [SerializeField]
    private float m_raycastLength = 1000f;

    [SerializeField]
    private float m_raycastYStartPosition = 1000f;

    private RaycastHit m_hit;


    private void Start()
    {
        PositionObjectToFloorHeight();
    }


    private void PositionObjectToFloorHeight()
    {
        if (Physics.Raycast(m_objectToMove.transform.position + Vector3.up * m_raycastYStartPosition, Vector3.down, out m_hit, m_raycastLength, m_groundLayer))
        {
            m_objectToMove.transform.position = m_hit.point + m_positionOffset;

            OnObjectInPosition?.Invoke();
        }
        else
        {
            Destroy(m_objectToMove);
        }
    }


}
