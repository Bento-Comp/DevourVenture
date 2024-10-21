using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectRotationController : MonoBehaviour
{
    [SerializeField]
    private ObjectPositionController m_objectPositionController = null;

    [SerializeField]
    private GroundData m_groundData = null;

    [SerializeField]
    private GameObject m_objectToRotate = null;


    private void OnEnable()
    {
        m_objectPositionController.OnObjectInPosition += OnObjectInPosition;
    }

    private void OnDisable()
    {
        m_objectPositionController.OnObjectInPosition -= OnObjectInPosition;
    }

    private void OnObjectInPosition()
    {
        m_groundData.CalculateGroundNormal();
        SetObjectRotationToFloorNormal();
    }

    private void SetObjectRotationToFloorNormal()
    {
        m_objectToRotate.transform.rotation = Quaternion.FromToRotation(m_objectToRotate.transform.up, m_groundData.GroundNormal);
    }
}
