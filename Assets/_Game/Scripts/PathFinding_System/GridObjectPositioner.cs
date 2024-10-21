using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(GridObjectPosition))]
public class GridObjectPositioner : MonoBehaviour
{
    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private GridObjectPosition m_gridObjectPosition = null;


    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            UpdatePosition();
#endif
    }


    private void UpdatePosition()
    {
        if (Manager_Grid.Instance != null && m_controlledTransform != null && m_gridObjectPosition)
            m_controlledTransform.position = Manager_Grid.Instance.CalculateWorldPosition(m_gridObjectPosition.XPosition, m_gridObjectPosition.YPosition);
    }
}
