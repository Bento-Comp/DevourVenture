using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ActiveSkill_PopularDishSelection_ButtonController : MonoBehaviour
{
    public static System.Action<FoodType> OnSendPopularDishSelected;

    [SerializeField]
    private GameObject m_ui = null;

    [SerializeField]
    private ActiveSkill_PopularDishSelection_ButtonSelect m_activeSkill_PopularDishSelection_ButtonSelect = null;

    [SerializeField]
    private Image m_foodSprite = null;

    [SerializeField]
    private FoodVisualAssets_ScriptableObject m_foodAssets = null;

    [SerializeField]
    private FoodType m_foodType;

    public FoodType FoodType { get => m_foodType; }


    private void OnEnable()
    {
        m_activeSkill_PopularDishSelection_ButtonSelect.OnPopularDishSelectionButtonPressed += SendPopularDishSelected;
    }

    private void OnDisable()
    {
        m_activeSkill_PopularDishSelection_ButtonSelect.OnPopularDishSelectionButtonPressed -= SendPopularDishSelected;
    }



    private void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (m_foodSprite == null)
            return;

        if (m_foodAssets == null)
            return;

        for (int i = 0; i < m_foodAssets.m_foodVisualAssetsList.Count; i++)
        {
            if(m_foodAssets.m_foodVisualAssetsList[i].foodType == m_foodType)
            {
                m_foodSprite.sprite = m_foodAssets.m_foodVisualAssetsList[i].spriteImage;

                return;
            }
        }
    }


    private void SendPopularDishSelected()
    {
        OnSendPopularDishSelected?.Invoke(m_foodType);
    }
}
