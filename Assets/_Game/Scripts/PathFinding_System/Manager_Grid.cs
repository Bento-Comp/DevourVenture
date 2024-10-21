using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Manager_Grid : UniSingleton.Singleton<Manager_Grid>
{
    public static System.Action OnGridGenerated;

    [SerializeField]
    private int m_width = 10;

    [SerializeField]
    private int m_height = 10;

    [SerializeField]
    private float m_cellSize = 1f;

    [Header("Debug")]
    [SerializeField]
    private float m_nodeGizmosRadius = 0.1f;

    [SerializeField]
    private int xToTest = 0;

    [SerializeField]
    private int yToTest = 0;

    [SerializeField]
    private bool m_isDebugEnabled = true;


    private GridPathFinding m_pathFindingGrid;

    public int Width { get => m_width; }
    public int Height { get => m_height; }
    public float CellSize { get => m_cellSize; }


    private void Start()
    {
        GenerateGrid();
    }


    public void TestCell()
    {
        Node node = m_pathFindingGrid.GetNode(xToTest, yToTest);
        Debug.Log(node.m_isWalkable);
    }


    public void GenerateGrid()
    {
        if (m_pathFindingGrid != null)
            ClearGrid();

        m_pathFindingGrid = new GridPathFinding(m_width, m_height, m_cellSize);

        OnGridGenerated?.Invoke();
    }


    public void ClearGrid()
    {
        m_pathFindingGrid = null;
    }


    public List<Node> FindPath(int startX, int startY, int endX, int endY, bool canMoveDiagonaly)
    {
        if (m_pathFindingGrid != null)
            return new List<Node>(m_pathFindingGrid.FindPath(startX, startY, endX, endY, canMoveDiagonaly));
        else
            return null;
    }


    public Vector3 CalculateWorldPosition(int xPosition, int yPosition)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(xPosition, 0, m_width - 1);
        position.z = Mathf.Clamp(yPosition, 0, m_height - 1);

        return position * m_cellSize;
    }


    public void SetNodeWalkableState(int xPosition, int yPosition, bool isWalkable)
    {
        if (xPosition < 0 || xPosition >= m_width || yPosition < 0 || yPosition >= m_height)
        {
            Debug.LogError("Index out of range. Can't reach cell to set walkable state");
            return;
        }

        if (m_pathFindingGrid != null && m_pathFindingGrid.Grid != null)
        {
            m_pathFindingGrid.Grid.GetGridObject(xPosition, yPosition).m_isWalkable = isWalkable;
        }
    }

    private void OnDrawGizmos()
    {
        DrawGridPoints();
    }

    private void DrawGridPoints()
    {
        if (m_isDebugEnabled)
        {
            if (m_pathFindingGrid == null || m_pathFindingGrid.Grid == null)
                return;

            if (m_pathFindingGrid.Grid.GetWidth() == 0 || m_pathFindingGrid.Grid.GetHeight() == 0)
                return;

            Vector3 position = Vector3.zero;

            for (int i = 0; i < m_pathFindingGrid.Grid.GetWidth(); i++)
            {
                for (int j = 0; j < m_pathFindingGrid.Grid.GetHeight(); j++)
                {
                    Gizmos.color = Color.yellow;

                    position = CalculateWorldPosition(i, j);
                    Gizmos.DrawSphere(position, m_nodeGizmosRadius);
                }
            }

            Gizmos.color = Color.magenta;

            position = CalculateWorldPosition(xToTest, yToTest);
            Gizmos.DrawSphere(position, m_nodeGizmosRadius * 3f);
        }
    }
}
