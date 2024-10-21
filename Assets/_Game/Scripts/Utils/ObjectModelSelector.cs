using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectModelSelector : MonoBehaviour
{
    public Action<GameObject> OnModelSelected;

    [Header("Parameters for selecting a model already in scene hierarchy")]
    [SerializeField]
    private List<GameObject> m_modelList = null;

    [Header("Parameters for instantiating a random model from a prefab list")]
    [SerializeField]
    private List<GameObject> m_modelPrefabList = null;

    [SerializeField]
    private Transform m_modelParent = null;

    private GameObject m_selectedModel;
    private int m_randomInt;

    private void Start()
    {
        if (m_modelList.Count > 0)
        {
            EnableRandomModel();
            return;
        }

        InstantiateRandomModel();
    }

    private void EnableRandomModel()
    {
        m_randomInt = UnityEngine.Random.Range(0, m_modelList.Count);

        for (int i = 0; i < m_modelList.Count; i++)
        {
            if (i == m_randomInt)
            {
                m_modelList[i].SetActive(true);
                m_selectedModel = m_modelList[i];
                OnModelSelected?.Invoke(m_selectedModel);
            }
            else
            {
                m_modelList[i].SetActive(false);
            }
            
        }
    }

    private void InstantiateRandomModel()
    {
        m_randomInt = UnityEngine.Random.Range(0, m_modelPrefabList.Count);

        m_selectedModel=Instantiate(m_modelPrefabList[m_randomInt], m_modelParent);

        OnModelSelected?.Invoke(m_selectedModel);
    }
}
