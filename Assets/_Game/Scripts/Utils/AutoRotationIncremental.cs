using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationIncremental : MonoBehaviour
{
    [SerializeField]
    private Transform m_controlledTransform = null;

    [SerializeField]
    private float m_delayBetweenIncrements = 0.25f;

    [SerializeField]
    private float m_angleIncrement = 30f;

    private float m_timer;


    private void Update()
    {
        if (m_controlledTransform == null)
            return;

        m_timer += Time.deltaTime;


        if (m_timer > m_delayBetweenIncrements)
        {
            m_timer = 0f;

            Vector3 desiredRotation = m_controlledTransform.rotation.eulerAngles;

            desiredRotation.z += m_angleIncrement;

            m_controlledTransform.rotation = Quaternion.Euler(desiredRotation);
        }
    }
}
