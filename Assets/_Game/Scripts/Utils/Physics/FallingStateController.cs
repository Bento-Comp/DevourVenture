using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStateController : MonoBehaviour
{
    public Action OnFalling;
    public Action OnNotFalling;

    [SerializeField]
    private GroundedStateController m_groundedStateController = null;

    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private float m_fallingStateThreshold = 0.2f;


    private bool m_isFalling;


    private void OnEnable()
    {
        m_groundedStateController.OnGrounded += OnGrounded;
    }

    private void OnDisable()
    {
        m_groundedStateController.OnGrounded -= OnGrounded;
    }


    private void FixedUpdate()
    {
        CheckFallingCondition();
    }



    private void OnGrounded()
    {
        if (m_isFalling)
        {
            m_isFalling = false;
            OnNotFalling?.Invoke();
        }
    }


    private void CheckFallingCondition()
    {
        if (!m_groundedStateController.IsGrounded && !m_isFalling)
        {
            if (m_rigidbody.velocity.y < -m_fallingStateThreshold)
            {
                m_isFalling = true;
                OnFalling?.Invoke();
            }
        }
    }

}
