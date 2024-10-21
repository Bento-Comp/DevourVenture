using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill_PopularDishSelectionMenuUI : Game_UI
{
    [SerializeField]
    private List<ActiveSkill_PopularDishSelection_ButtonController> m_activeSkillPopularDishSelectionButtonControllersList = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        ActiveSkillSlot_ButtonSelectSkillEffect.OnSelectSkillEffectButtonPressed += OnSelectSkillEffectButtonPressed;
        ActiveSkill_PopularDishSelection_ButtonController.OnSendPopularDishSelected += OnSendPopularDishSelected;
        ActiveSkill_PopularDishSelection_ButtonExit.OnPopularDishSelectionExitButtonPressed += OnPopularDishSelectionExitButtonPressed;
        Manager_Order.OnSendAvailableFoodTypeList += OnSendAvailableFoodTypeList;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ActiveSkillSlot_ButtonSelectSkillEffect.OnSelectSkillEffectButtonPressed -= OnSelectSkillEffectButtonPressed;
        ActiveSkill_PopularDishSelection_ButtonController.OnSendPopularDishSelected -= OnSendPopularDishSelected;
        ActiveSkill_PopularDishSelection_ButtonExit.OnPopularDishSelectionExitButtonPressed -= OnPopularDishSelectionExitButtonPressed;
        Manager_Order.OnSendAvailableFoodTypeList -= OnSendAvailableFoodTypeList;
    }

    private void Awake()
    {
        for (int i = 0; i < m_activeSkillPopularDishSelectionButtonControllersList.Count; i++)
        {
            m_activeSkillPopularDishSelectionButtonControllersList[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        ToggleUI(false);
    }


    private void OnSendAvailableFoodTypeList(List<FoodType> availableFoodTypeList)
    {
        CheckAvailability(availableFoodTypeList);
    }

    public void CheckAvailability(List<FoodType> m_availableFoodTypeList)
    {
        for (int i = 0; i < m_availableFoodTypeList.Count; i++)
        {
            ActivatePopularDishSelectionButton(m_availableFoodTypeList[i]);
        }

    }

    private void ActivatePopularDishSelectionButton(FoodType foodType)
    {
        for (int i = 0; i < m_activeSkillPopularDishSelectionButtonControllersList.Count; i++)
        {
            if (m_activeSkillPopularDishSelectionButtonControllersList[i].FoodType == foodType)
            {
                m_activeSkillPopularDishSelectionButtonControllersList[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    private void OnSelectSkillEffectButtonPressed()
    {
        OpenUI();
    }

    private void OnPopularDishSelectionExitButtonPressed()
    {
        CloseUI();
    }

    private void OnSendPopularDishSelected(FoodType foodType)
    {
        CloseUI();
    }


}
