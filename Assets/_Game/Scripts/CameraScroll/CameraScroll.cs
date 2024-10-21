using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class CameraScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject m_cameraTarget = null;

    [SerializeField]
    private Bounds m_cameraTargetPositionBounds;

    [SerializeField]
    private float m_distanceTraveledPerPixelUnit = 0.1f;

    [SerializeField]
    private bool m_canMoveHorizontally = true;

    [SerializeField]
    private bool m_isHorizontalMovementReversed = true;

    [SerializeField]
    private bool m_canMoveVertically = true;

    [SerializeField]
    private bool m_isVerticalMovementReversed = true;

    private Vector3 m_cursorScreenStartPosition;
    private Vector3 m_cursorScreenCurrentPosition;
    private Vector3 m_startCameraTargetPosition;
    private Vector3 m_desiredCameraTargetPosition;
    private float m_xAxisDiff;
    private float m_yAxisDiff;
    private bool m_isReceivingInputs;
    private bool m_isCurrentCursorPositionSet;

    private void OnEnable()
    {
        Controller.OnTapBegin += OnTapBegin;
        Controller.OnHold += OnHold;
        Controller.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        Controller.OnTapBegin -= OnTapBegin;
        Controller.OnHold -= OnHold;
        Controller.OnRelease -= OnRelease;
    }


    private void LateUpdate()
    {
        if (m_isReceivingInputs && !Game_UI.IsAnyUIOpen && m_isCurrentCursorPositionSet)
        {
            MoveCameraTarget();
        }
    }


    private void MoveCameraTarget()
    {
        m_xAxisDiff = m_cursorScreenCurrentPosition.x - m_cursorScreenStartPosition.x;
        m_yAxisDiff = m_cursorScreenCurrentPosition.y - m_cursorScreenStartPosition.y;

        if (m_isHorizontalMovementReversed)
            m_xAxisDiff *= -1;

        if (m_isVerticalMovementReversed)
            m_yAxisDiff *= -1;



        if (m_canMoveHorizontally)
            m_desiredCameraTargetPosition.x = m_startCameraTargetPosition.x + m_xAxisDiff * m_distanceTraveledPerPixelUnit;

        if (m_canMoveVertically)
            m_desiredCameraTargetPosition.z = m_startCameraTargetPosition.z + m_yAxisDiff * m_distanceTraveledPerPixelUnit;


        if (m_cameraTargetPositionBounds.Contains(m_desiredCameraTargetPosition))
        {
            m_cameraTarget.transform.position = m_desiredCameraTargetPosition;
            m_isCurrentCursorPositionSet = true;
        }
        else
        {
            m_cameraTarget.transform.position = m_cameraTargetPositionBounds.ClosestPoint(m_desiredCameraTargetPosition);
            m_isCurrentCursorPositionSet = false;
        }

    }


    private void OnTapBegin(Vector3 cursorPosition)
    {
        m_isReceivingInputs = true;
        m_cursorScreenStartPosition = cursorPosition;
        m_startCameraTargetPosition = m_cameraTarget.transform.position;
    }


    private void OnHold(Vector3 cursorPosition)
    {
        if (m_isReceivingInputs)
        {
            m_cursorScreenCurrentPosition = cursorPosition;
            m_isCurrentCursorPositionSet = true;
        }
    }

    private void OnRelease(Vector3 cursorPosition)
    {
        m_isReceivingInputs = false;
        m_isCurrentCursorPositionSet = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(m_cameraTargetPositionBounds.center, m_cameraTargetPositionBounds.size);
    }
}
