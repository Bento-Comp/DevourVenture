using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAbsoluteRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject m_object = null;

    private Quaternion m_desiredRotation;

    private void LateUpdate()
    {
        // the player is the new referential. gives the value of the parameter (vector3 forfard) in the new referential
        // example : if the player is facing east (1, 0, 0). The value of vector3 forward will be (-1, 0, 0) in the new referential
        m_desiredRotation = Quaternion.Euler(m_object.transform.InverseTransformDirection(Vector3.forward));
        m_desiredRotation.x = 0f;
        m_desiredRotation.z = 0f;
        transform.rotation = m_desiredRotation;
    }
}
