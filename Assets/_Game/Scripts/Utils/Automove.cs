using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automove : MonoBehaviour
{
    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private float m_movementSpeed = 5f;

    private void Update()
    {
        m_controlledTransform.position += m_controlledTransform.forward * m_movementSpeed * Time.deltaTime;
    }
}
