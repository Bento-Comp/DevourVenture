using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigationAgent : MonoBehaviour
{
    public System.Action OnDestinationReached;
    public System.Action OnStartMoving;
    public System.Action OnStopMoving;


    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private float m_movementSpeed = 5f;

    [SerializeField]
    private bool m_canMoveDiagonaly = true;

    [SerializeField]
    private bool m_isStationary = false;

    [SerializeField]
    private bool m_isDebugEnabled = false;

    private List<Node> m_pathList;
    private Vector3 m_currentDestinationStep;
    private float m_currentMovementSpeed;
    private float m_currentSpeedMultiplier = 1f;
    private int xGridPosition;
    private int yGridPosition;
    private int m_currentIndexProgression;


    public Vector3 Position { get => m_controlledTransform.position; }


    private void Start()
    {
        m_currentMovementSpeed = m_movementSpeed;
    }

    private void Update()
    {
        if (!m_isStationary)
        {
            MoveToDestination();
            RotateTowardDestination();
        }
    }

    public void SetSpeedMultiplier(float newMultiplier)
    {
        m_currentSpeedMultiplier = newMultiplier;
    }

    private void MoveToDestination()
    {
        if (m_pathList == null)
            return;

        if (m_pathList.Count == 0)
            return;


        if (Vector3.Distance(m_controlledTransform.position, m_currentDestinationStep) < 0.1f)
            SetNextDestinationStep();
        else
            m_controlledTransform.position = Vector3.MoveTowards(m_controlledTransform.position, m_currentDestinationStep, m_currentMovementSpeed * m_currentSpeedMultiplier * Time.deltaTime);
    }


    private void RotateTowardDestination()
    {
        if (Vector3.Angle(m_controlledTransform.forward, m_currentDestinationStep - m_controlledTransform.position) > 2f && !m_isStationary)
        {
            SetForwardDirection(m_currentDestinationStep - m_controlledTransform.position);
        }
    }

    private void SetForwardDirection(Vector3 direction)
    {
        m_controlledTransform.forward = direction;
    }

    public void ObjectToLookAt(GameObject objectToLookAt)
    {
        SetForwardDirection(objectToLookAt.transform.position - m_controlledTransform.position);
    }

    private void SetNextDestinationStep()
    {
        xGridPosition = m_pathList[m_currentIndexProgression].X;
        yGridPosition = m_pathList[m_currentIndexProgression].Y;

        if (m_currentIndexProgression < m_pathList.Count - 1)
        {
            m_currentIndexProgression++;
            SetDestinationStep();
        }
        else
        {
            m_isStationary = true;
            OnStopMoving?.Invoke();
            OnDestinationReached?.Invoke();
        }
    }

    private void SetDestinationStep()
    {
        m_currentDestinationStep = Manager_Grid.Instance.CalculateWorldPosition(m_pathList[m_currentIndexProgression].X, m_pathList[m_currentIndexProgression].Y);
    }


    public void SetPosition(int currentXGridPosition, int currentYGridPosition)
    {
        xGridPosition = currentXGridPosition;
        yGridPosition = currentYGridPosition;

        m_controlledTransform.position = Manager_Grid.Instance.CalculateWorldPosition(xGridPosition, yGridPosition);
    }


    public void SetDestination(int xGridDestinationPosition, int yGridDestinationPosition)
    {
        if (m_pathList != null)
            m_pathList.Clear();

        m_pathList = new List<Node>(Manager_Grid.Instance.FindPath(xGridPosition, yGridPosition, xGridDestinationPosition, yGridDestinationPosition, m_canMoveDiagonaly));

        if (m_isDebugEnabled)
        {
            for (int i = 0; i < m_pathList.Count; i++)
            {
                Debug.Log(m_pathList[i].X + ", " + m_pathList[i].Y);
            }
        }

        OnStartMoving?.Invoke();
        m_currentIndexProgression = 0;
        m_isStationary = false;
        SetDestinationStep();
    }
}
