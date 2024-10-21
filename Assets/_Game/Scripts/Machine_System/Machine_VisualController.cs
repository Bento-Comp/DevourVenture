using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Machine_VisualController : MonoBehaviour
{
    [SerializeField]
    private Machine m_machineReference = null;

    [SerializeField]
    private GameObject m_visualPosition = null;

    [SerializeField]
    private GameObject m_machineVisual = null;

    [SerializeField]
    private GameObject m_giftModel = null;

    [SerializeField]
    private GameObject m_objectLookTowards = null;

    private FoodVisualAssets m_currentFoodVisualAsset;
    private FoodType m_currentStandFoodType;


    private void OnEnable()
    {
        m_machineReference.OnUpdateState += OnUpdateState;
    }

    private void OnDisable()
    {
        m_machineReference.OnUpdateState -= OnUpdateState;
    }

    private void Update()
    {
        if (m_currentStandFoodType != m_machineReference.MachineFoodType)
        {
            m_currentStandFoodType = m_machineReference.MachineFoodType;

            UpdateFoodVisualAssets();

            UpdateVisuals();
        }
    }


    private void RotateMachineTowardsTarget()
    {
        Vector3 directionLookTowards = m_objectLookTowards.transform.position - m_visualPosition.transform.position;
        directionLookTowards.y = 0f;
        m_visualPosition.transform.forward = directionLookTowards;
    }

    private void OnUpdateState(Machine.State state)
    {
        switch (state)
        {
            case Machine.State.ToUnlock:
                m_giftModel.SetActive(true);
                m_machineVisual.SetActive(false);
                break;
            case Machine.State.Active:
                m_machineVisual.SetActive(true);
                m_giftModel.SetActive(false);
                break;
            case Machine.State.NotActive:
                m_machineVisual.SetActive(false);
                m_giftModel.SetActive(false);
                break;
            default:
                break;
        }
    }


    private void UpdateFoodVisualAssets()
    {
        if (Manager_FoodVisualAssets.Instance != null)
            m_currentFoodVisualAsset = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(m_currentStandFoodType);
        else
            Debug.LogError("Manager_FoodVisualAssets in null");
    }


    private void UpdateVisuals()
    {
#if UNITY_EDITOR
        if (m_visualPosition != null && UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
        {
            if (m_visualPosition.transform.childCount > 0)
                DestroyTransformChildren.DestroyAllTransformChildren(m_visualPosition.transform);

            if (m_currentFoodVisualAsset != null && m_currentFoodVisualAsset.machineModel != null)
            {
                Instantiate(m_currentFoodVisualAsset.machineModel, m_visualPosition.transform.position, m_visualPosition.transform.rotation, m_visualPosition.transform);
                RotateMachineTowardsTarget();
            }
        }
#endif
    }

}
