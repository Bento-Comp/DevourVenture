using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_body = null;

    [SerializeField, Tooltip("Physical logic collider")]
    private Collider m_controlledCollider = null;

    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private List<Rigidbody> m_ragdollRigidbodyList = null;

    [SerializeField]
    private List<Collider> m_ragdollColliders = null;

    [SerializeField]
    private List<Rigidbody> m_upperBodyRigidbody = null;

    [SerializeField]
    private float m_impulseStrength = 10f;

    [SerializeField]
    private float m_upImpulseStrength = 25f;


    private bool m_isRagdollActive;


    public List<Rigidbody> UpperBodyRigidbody { get => m_upperBodyRigidbody; }
    public bool IsRagdollActive { get => m_isRagdollActive; }


    private void Start()
    {
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        ToggleRagdoll(true);
    }


    public void DisableRagdoll()
    {
        ToggleRagdoll(false);
    }


    private void ToggleRagdoll(bool isRagdollActive)
    {
        m_isRagdollActive = isRagdollActive;

        m_body.isKinematic = isRagdollActive;

        //controlled collider is active if ragdoll state is not
        m_controlledCollider.enabled = !isRagdollActive;

        //animator active if ragodll state is not
        m_animator.enabled = !isRagdollActive;


        for (int i = 0; i < m_ragdollRigidbodyList.Count; i++)
        {
            //ragdoll rigidbody in kinematic if ragdoll sate is false
            m_ragdollRigidbodyList[i].isKinematic = !isRagdollActive;
        }

        for (int i = 0; i < m_ragdollColliders.Count; i++)
        {
            m_ragdollColliders[i].enabled = isRagdollActive;
        }
    }


    public void ApplyEjectionForceOnRagdoll(Vector3 ejectionDirection)
    {
        Vector3 desiredImpusle = ejectionDirection * m_impulseStrength;
        ApplyForceOnRagdoll(desiredImpusle);
        ApplyForceOnRagdoll(Vector3.up * m_upImpulseStrength);
    }


    private void ApplyForceOnRagdoll(Vector3 direction)
    {
        for (int i = 0; i < m_upperBodyRigidbody.Count; i++)
        {
            m_upperBodyRigidbody[i].AddForce(direction, ForceMode.Impulse);
        }
    }
}
