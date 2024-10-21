using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Stand_VisualController : MonoBehaviour
{
    [SerializeField]
    private Stand m_standReference = null;

    [SerializeField]
    private GameObject m_visualPosition = null;

    [SerializeField]
    private GameObject m_standVisualWhenActive = null;

    [SerializeField]
    private GameObject m_standVisualWhenNotActive = null;


    private FoodVisualAssets m_currentFoodVisualAsset;
    private FoodType m_currentStandFoodType;



    private void OnEnable()
    {
        m_standReference.OnUpdateState += OnUpdateState;
    }

    private void OnDisable()
    {
        m_standReference.OnUpdateState -= OnUpdateState;
    }


    private void Update()
    {
        if (m_currentStandFoodType != m_standReference.StandFoodType)
        {
            m_currentStandFoodType = m_standReference.StandFoodType;

            UpdateFoodVisualAssets();

            UpdateVisuals();
        }
    }


    private void UpdateFoodVisualAssets()
    {
        if (Manager_FoodVisualAssets.Instance == null)
            return;

        m_currentFoodVisualAsset = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(m_currentStandFoodType);
    }


    private void UpdateVisuals()
    {
#if UNITY_EDITOR
        if (m_visualPosition != null && UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
        {
            if (m_visualPosition.transform.childCount > 0)
                DestroyTransformChildren.DestroyAllTransformChildren(m_visualPosition.transform);

            if (m_currentFoodVisualAsset != null && m_currentFoodVisualAsset.spriteModel != null)
                Instantiate(m_currentFoodVisualAsset.spriteModel, m_visualPosition.transform.position, Quaternion.identity, m_visualPosition.transform);
        }
#endif
    }


    private void OnUpdateState(Stand.State state)
    {
        switch (state)
        {
            case Stand.State.Active:
                m_standVisualWhenActive.SetActive(true);
                m_standVisualWhenNotActive.SetActive(false);
                break;
            case Stand.State.NotActive:
                m_standVisualWhenActive.SetActive(false);
                m_standVisualWhenNotActive.SetActive(true);
                break;
            default:
                break;
        }
    }
}
