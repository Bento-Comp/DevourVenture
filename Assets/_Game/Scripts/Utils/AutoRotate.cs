using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private float m_rotationSpeed = 5f;

    [SerializeField]
    private bool m_x = false;

    [SerializeField]
    private bool m_y = false;

    [SerializeField]
    private bool m_z = false;


    private Vector3 m_rotationAxis;



    private void FixedUpdate()
    {
        Rotate();
    }


    private void Rotate()
    {
        m_rotationAxis.x = m_x ? 1f : 0f;
        m_rotationAxis.y = m_y ? 1f : 0f;
        m_rotationAxis.z = m_z ? 1f : 0f;

        m_controlledTransform.Rotate(m_rotationAxis, m_rotationSpeed * Time.deltaTime);
    }


}
