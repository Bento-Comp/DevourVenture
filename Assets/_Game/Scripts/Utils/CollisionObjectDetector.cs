using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObjectDetector : MonoBehaviour
{
    //GameObject : collision object; Collision : collision info
    public Action<GameObject, Collision> OnObjectCollision;

    [SerializeField]
    private LayerMask m_effectiveLayer = 0;


    private void OnCollisionEnter(Collision collision)
    {
        if (m_effectiveLayer == (m_effectiveLayer | (1 << collision.collider.gameObject.layer)))
            OnObjectCollision?.Invoke(collision.collider.gameObject, collision);
    }
}
