using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People_ModelSelector : MonoBehaviour
{
    public enum PeopleType
    {
        Worker,
        Customer,
        Waiter,
        Chef
    }

    public System.Action<Animator> OnModelSelected;

    [SerializeField]
    private PeopleType m_type;

    [SerializeField]
    private GameObject m_workerModel = null;

    [SerializeField]
    private GameObject m_waiterModel = null;

    [SerializeField]
    private GameObject m_chefModel = null;

    [SerializeField]
    private List<GameObject> m_customerModelList = null;

    private GameObject m_selectedModel;
    private Animator m_selectedModelAnimator;

    private void Start()
    {
        m_workerModel.SetActive(false);
        m_waiterModel.SetActive(false);
        m_chefModel.SetActive(false);

        for (int i = 0; i < m_customerModelList.Count; i++)
            m_customerModelList[i].SetActive(false);

        SelectModel();
    }


    private void SelectModel()
    {
        switch (m_type)
        {
            case PeopleType.Worker:
                m_selectedModel = m_workerModel;
                break;
            case PeopleType.Waiter:
                m_selectedModel = m_waiterModel;
                break;
            case PeopleType.Chef:
                m_selectedModel = m_chefModel;
                break;
            case PeopleType.Customer:
                int randomIndex = Random.Range(0, m_customerModelList.Count);
                m_selectedModel = m_customerModelList[randomIndex];
                break;
            default:
                break;
        }


        m_selectedModel.SetActive(true);

        m_selectedModelAnimator = m_selectedModel.GetComponent<Animator>();

        if (m_selectedModelAnimator != null)
            OnModelSelected?.Invoke(m_selectedModelAnimator);
        else
            Debug.LogError("Could not get animator component");
    }
}
