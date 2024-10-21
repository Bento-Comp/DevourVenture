using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObjectDetector : MonoBehaviour
{
    public Action<GameObject> OnObjectDetected;
    public Action<GameObject> OnObjectNotDetectedAnymore;

    [SerializeField]
    private string m_objectTagToDetect = "";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_objectTagToDetect))
        {
            OnObjectDetected?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_objectTagToDetect))
        {
            OnObjectNotDetectedAnymore?.Invoke(other.gameObject);
        }
    }
}
