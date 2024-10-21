using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalGravity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_body = null;

    [SerializeField, Tooltip("must be negative")]
    private float m_additionalGravityToApply = -9.81f;




    private void FixedUpdate()
    {
        ApplyAdditionalGravity();
    }


    private void ApplyAdditionalGravity()
    {
        m_body.AddForce(m_additionalGravityToApply * Vector3.up, ForceMode.Acceleration);
    }
}
