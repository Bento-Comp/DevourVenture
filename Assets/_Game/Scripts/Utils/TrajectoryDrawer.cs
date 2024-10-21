using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_trajectoryLineRenderer = null;

    [SerializeField]
    private float m_lineRendererWidth = 2f;

    [SerializeField]
    private int m_segmentCount = 20;

    private List<Vector3> m_pointPositionList = new List<Vector3>();


    private float m_flightDurationBuffer;
    private float stepTime;


    private void Start()
    {
        m_trajectoryLineRenderer.startWidth = m_lineRendererWidth;
        m_trajectoryLineRenderer.endWidth = m_lineRendererWidth;
    }


    public void UpdateLineTrajectory(Vector3 forceVector, Rigidbody body, Vector3 startingPosition)
    {
        m_pointPositionList.Clear();


        m_flightDurationBuffer = (2f * forceVector.y) / -Physics.gravity.y;

        stepTime = m_flightDurationBuffer / (float)m_segmentCount;

        for (int i = 0; i < m_segmentCount; i++)
        {
            float stepTimePassed = i * stepTime;

            Vector3 movementVector = new Vector3(
                forceVector.x * stepTimePassed,
                forceVector.y * stepTimePassed - 0.5f * -Physics.gravity.y * stepTimePassed * stepTimePassed,
                forceVector.z * stepTimePassed
                );

            m_pointPositionList.Add(startingPosition + movementVector);
        }



        m_trajectoryLineRenderer.positionCount = m_segmentCount;
        m_trajectoryLineRenderer.SetPositions(m_pointPositionList.ToArray());
    }


    public void HideLineRenderer()
    {
        m_trajectoryLineRenderer.positionCount = 0;
    }

}
