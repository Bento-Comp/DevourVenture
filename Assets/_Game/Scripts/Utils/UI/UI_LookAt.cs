using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UI_LookAt : MonoBehaviour
{
    [SerializeField]
    private Transform m_target = null;

    private void Start()
    {
        if (m_target == null)
            m_target = Camera.main.gameObject.transform;
    }

    private void Update()
    {
        if (Camera.main == null)
            return;

        if (m_target != null)
            transform.LookAt(transform.position + m_target.rotation * Vector3.forward, m_target.rotation * Vector3.up);
    }
}
