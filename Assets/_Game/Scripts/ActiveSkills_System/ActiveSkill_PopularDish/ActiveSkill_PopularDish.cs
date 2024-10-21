using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveSkill_PopularDish : MonoBehaviour
{
    public static System.Action OnAskSkillState;
    public static System.Action OnAskSkillInfos;

    [SerializeField]
    private UniActivation.Activator m_activator = null;

    [SerializeField]
    private FoodVisualAssets_ScriptableObject m_foodVisualAssetsData = null;

    [SerializeField]
    private Image m_selectedFoodTypeSprite = null;

    [SerializeField]
    private TMP_Text m_titleText = null;

    [SerializeField]
    private TMP_Text m_descriptionText = null;

    [SerializeField]
    private TMP_Text m_descriptionSpecificText = null;

    [SerializeField]
    private TMP_Text m_skillDurationText = null;

    [SerializeField]
    private TMP_Text m_cooldownText = null;

    [SerializeField]
    private TMP_Text m_skillEffectRemainingTimeText = null;



    private string m_title = "";
    private string m_skillDescription = "";
    private string m_specificSkillDescriptionSuffix = "";
    private string m_skillEffectDuration = "";


    private string m_selectedFoodTypeName;



    private void OnEnable()
    {
        ActiveSkill_PopularDish_Logic.OnSendSkillInfos += OnSendSkillInfos;
        ActiveSkill_PopularDish_Logic.OnSendSkillState += OnSendSkillState;
        ActiveSkill_PopularDish_Logic.OnPopularDishSelectedEffect += OnPopularDishSelected;

        ActiveSkill_PopularDish_Logic.OnBroadcastSkillCooldownRemainingDuration += OnBroadcastSkillCooldownRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillEffectRemainingDuration += OnBroadcastSkillEffectRemainingDuration;

        DisplayMenu();
    }

    private void OnDisable()
    {
        ActiveSkill_PopularDish_Logic.OnSendSkillInfos -= OnSendSkillInfos;
        ActiveSkill_PopularDish_Logic.OnSendSkillState -= OnSendSkillState;
        ActiveSkill_PopularDish_Logic.OnPopularDishSelectedEffect -= OnPopularDishSelected;

        ActiveSkill_PopularDish_Logic.OnBroadcastSkillCooldownRemainingDuration -= OnBroadcastSkillCooldownRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillEffectRemainingDuration -= OnBroadcastSkillEffectRemainingDuration;
    }


    private void OnBroadcastSkillCooldownRemainingDuration(float remainingCooldownDuration)
    {
        m_cooldownText.text = FormatTime.Format_Time((int)remainingCooldownDuration);
    }

    private void OnBroadcastSkillEffectRemainingDuration(float remainingSkillEffectDuration)
    {
        m_skillEffectRemainingTimeText.text = FormatTime.Format_Time((int)remainingSkillEffectDuration);
    }

    private void OnSendSkillInfos(string skillTitle, string skillDescription, string skillSpecificDescription, float skillDuration, float cooldownDuration)
    {
        ActiveSkill_PopularDish_Logic.OnSendSkillInfos -= OnSendSkillInfos;

        m_title = skillTitle;
        m_titleText.text = m_title;

        m_skillDescription = skillDescription;
        m_descriptionText.text = m_skillDescription;

        m_specificSkillDescriptionSuffix = skillSpecificDescription;
        m_skillDurationText.text = m_specificSkillDescriptionSuffix;

        m_skillEffectDuration = FormatTime.Format_Time((int)skillDuration);
        m_skillDurationText.text = m_skillEffectDuration;
    }


    private void DisplayMenu()
    {
        OnAskSkillState?.Invoke();
        OnAskSkillInfos?.Invoke();
    }


    private void OnSendSkillState(bool isSkillActive, bool isSkillOnCooldown)
    {
        if (isSkillActive)
        {
            m_activator.SelectedIndex = 2;
            return;
        }

        if (isSkillOnCooldown)
        {
            m_activator.SelectedIndex = 3;
            return;
        }

        m_activator.SelectedIndex = 0;
        m_descriptionText.text = m_skillDescription;
    }


    private void OnPopularDishSelected(FoodType foodType)
    {
        m_activator.SelectedIndex = 1;



        for (int i = 0; i < m_foodVisualAssetsData.m_foodVisualAssetsList.Count; i++)
        {
            if (m_foodVisualAssetsData.m_foodVisualAssetsList[i].foodType == foodType)
            {
                m_selectedFoodTypeSprite.sprite = m_foodVisualAssetsData.m_foodVisualAssetsList[i].spriteImage;
                m_selectedFoodTypeName = m_foodVisualAssetsData.m_foodVisualAssetsList[i].name;
                m_descriptionSpecificText.text = m_selectedFoodTypeName + " " + m_specificSkillDescriptionSuffix;
                return;
            }
        }
    }

}
