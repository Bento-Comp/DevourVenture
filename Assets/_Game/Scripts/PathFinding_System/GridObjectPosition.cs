using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridObjectPosition : MonoBehaviour
{
    public System.Action<int, int, int, int> OnPositionChanged;

    [SerializeField]
    private int m_xPosition = 0;

    [SerializeField]
    private int m_yPosition = 0;



    private int m_currentXPosition;
    private int m_currentYPosition;


    public int XPosition { get => m_xPosition; }
    public int YPosition { get => m_yPosition; }

    private void Awake()
    {
        m_currentXPosition = m_xPosition;
        m_currentYPosition = m_yPosition;
    }


    private void Update()
    {
        if (m_xPosition != m_currentXPosition || m_yPosition != m_currentYPosition)
        {
            m_xPosition = Mathf.Clamp(m_xPosition, 0, Manager_Grid.Instance.Width - 1);
            m_yPosition = Mathf.Clamp(m_yPosition, 0, Manager_Grid.Instance.Height - 1);
            OnPositionChanged?.Invoke(m_currentXPosition, m_currentYPosition, m_xPosition, m_yPosition);
            m_currentXPosition = m_xPosition;
            m_currentYPosition = m_yPosition;
        }
    }


    public Vector3 GetWorldPosition()
    {
        return Manager_Grid.Instance.CalculateWorldPosition(m_currentXPosition, m_currentYPosition);
    }
}
