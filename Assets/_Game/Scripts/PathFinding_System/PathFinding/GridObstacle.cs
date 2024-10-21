using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridObjectPosition))]
public class GridObstacle : MonoBehaviour
{
    public static List<GridObstacle> m_gridObstacleList = new List<GridObstacle>();
    public static int m_instanceCount;


    [SerializeField]
    private GridObjectPosition m_gridObjectPosition = null;

    [Header("Debug")]
    [SerializeField]
    private float m_nonWalkableNodeGizmosRadius = 0.2f;


    public GridObjectPosition GridObjectPosition { get => m_gridObjectPosition; }


    private void Awake()
    {
        m_instanceCount = 0;
    }

    private void OnEnable()
    {
        Manager_Grid.OnGridGenerated += InitializeObstacle;
        m_gridObjectPosition.OnPositionChanged += OnPositionChanged;
    }

    private void OnDisable()
    {
        Manager_Grid.OnGridGenerated -= InitializeObstacle;
        m_gridObjectPosition.OnPositionChanged -= OnPositionChanged;
    }

    private void OnDestroy()
    {
        DestroyObstacle();
    }


    private void InitializeObstacle()
    {
        m_gridObjectPosition = gameObject.GetComponent<GridObjectPosition>();

        if (!GridObstacle.m_gridObstacleList.Contains(this))
        {
            GridObstacle.m_gridObstacleList.Add(this);
            m_instanceCount++;

            if (m_instanceCount == m_gridObstacleList.Count)
            {
                UpdateGridNodesWalkableState();
            }
        }
    }

    private void DestroyObstacle()
    {
        if (GridObstacle.m_gridObstacleList.Contains(this))
        {
            GridObstacle.m_gridObstacleList.Remove(this);
            OnObstacleDestroyed();
        }
    }

    private void OnObstacleDestroyed()
    {
        UpdateGridNodesWalkableState();
    }


    private void OnPositionChanged(int previousXPosition, int previousYPosition, int newXPosition, int newYPosition)
    {
        UpdateGridNodesWalkableState();
    }


    private void UpdateGridNodesWalkableState()
    {
        if (Manager_Grid.Instance == null)
            return;

        for (int i = 0; i < Manager_Grid.Instance.Width; i++)
        {
            for (int j = 0; j < Manager_Grid.Instance.Height; j++)
            {
                Manager_Grid.Instance.SetNodeWalkableState(i, j, true);
            }
        }

        for (int k = 0; k < m_gridObstacleList.Count; k++)
        {
            Manager_Grid.Instance.SetNodeWalkableState(m_gridObstacleList[k].GridObjectPosition.XPosition, m_gridObstacleList[k].GridObjectPosition.YPosition, false);
        }
    }


    private void OnDrawGizmos()
    {
        if (Manager_Grid.Instance == null)
            return;

        Gizmos.color = Color.red;
        Vector3 position = new Vector3(m_gridObjectPosition.XPosition, 0f, m_gridObjectPosition.YPosition) * Manager_Grid.Instance.CellSize;
        Gizmos.DrawSphere(position, m_nonWalkableNodeGizmosRadius);
    }

}
