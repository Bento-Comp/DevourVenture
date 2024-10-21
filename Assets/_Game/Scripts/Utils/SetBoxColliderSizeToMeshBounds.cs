using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoxColliderSizeToMeshBounds : MonoBehaviour
{
    [SerializeField]
    private BoxCollider m_boxCollider = null;

    [SerializeField]
    private ObjectModelSelector m_objectModelSelector = null;

    [SerializeField]
    private GameObject m_modelScaleFixObject = null;

    private MeshFilter m_meshFilter;



    private void OnEnable()
    {
        m_objectModelSelector.OnModelSelected += OnModelSelected;
    }

    private void OnDisable()
    {
        m_objectModelSelector.OnModelSelected -= OnModelSelected;
    }


    private void OnModelSelected(GameObject selectedModel)
    {
        m_meshFilter = selectedModel.GetComponentInChildren<MeshFilter>();

        if (m_meshFilter != null)
            SetBoxColliderSize();
    }


    private void SetBoxColliderSize()
    {
        m_boxCollider.size = m_meshFilter.mesh.bounds.size * m_modelScaleFixObject.transform.localScale.x * m_meshFilter.gameObject.transform.localScale.x;
        m_boxCollider.center = m_meshFilter.mesh.bounds.center * m_modelScaleFixObject.transform.localScale.x * m_meshFilter.gameObject.transform.localScale.x;
    }

}
