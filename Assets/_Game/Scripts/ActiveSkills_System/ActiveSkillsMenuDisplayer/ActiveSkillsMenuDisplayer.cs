using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillsMenuDisplayer : MonoBehaviour
{
    public static System.Action OnDisplayActiveSkillsMenu;

    [SerializeField]
    private GameObject m_activeSkillUI = null;

    [SerializeField]
    private ActiveSkillsMenuDisplayer_Button m_activeSkillsMenuDisplayer_Button = null;

    [SerializeField]
    private FoodVisualAssets_ScriptableObject m_foodVisualAssetsData = null;

    [SerializeField]
    private Image m_timerFillImage = null;

    [SerializeField]
    private Image m_selectedSkillIcon = null;

    [SerializeField]
    private Image m_backgorundImage = null;

    [SerializeField]
    private Color m_backgorundColorEnabled = Color.green;

    [SerializeField]
    private Color m_backgorundColorDisabled = Color.black;


    private Sprite m_selectedSkillIconBuffer;
    private float m_skillDuration;
    private float m_cooldownDuration;
    private bool m_isActiveSkillFeatureUnlocked = false;

    private void OnEnable()
    {
        m_activeSkillsMenuDisplayer_Button.OnDisplayActiveSkillsMenuButtonPressed += OnDisplayActiveSkillsMenuButtonPressed;
        ActiveSkill_PopularDish_Logic.OnSendSkillInfos += OnSendSkillInfos;
        ActiveSkill_PopularDish_Logic.OnPopularDishSelectedEffect += OnPopularDishSelectedEffect;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillEffectRemainingDuration += OnBroadcastSkillEffectRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillCooldownRemainingDuration += OnBroadcastSkillCooldownRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnStartPopularDishSkillEffect += OnStartPopularDishSkillEffect;
        ActiveSkill_PopularDish_Logic.OnStopPopularDishSkill += OnStopPopularDishSkill;
        ActiveSkillUnlocker.OnActiveSkillFeatureUnlocked += OnActiveSkillFeatureUnlocked;
    }

    private void OnDisable()
    {
        m_activeSkillsMenuDisplayer_Button.OnDisplayActiveSkillsMenuButtonPressed -= OnDisplayActiveSkillsMenuButtonPressed;
        ActiveSkill_PopularDish_Logic.OnSendSkillInfos -= OnSendSkillInfos;
        ActiveSkill_PopularDish_Logic.OnPopularDishSelectedEffect -= OnPopularDishSelectedEffect;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillEffectRemainingDuration -= OnBroadcastSkillEffectRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnBroadcastSkillCooldownRemainingDuration -= OnBroadcastSkillCooldownRemainingDuration;
        ActiveSkill_PopularDish_Logic.OnStartPopularDishSkillEffect -= OnStartPopularDishSkillEffect;
        ActiveSkill_PopularDish_Logic.OnStopPopularDishSkill -= OnStopPopularDishSkill;
        ActiveSkillUnlocker.OnActiveSkillFeatureUnlocked -= OnActiveSkillFeatureUnlocked;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(ActiveSkillUnlocker.m_isPopularDishSkillUnlockedPlayerPrefKey))
        {
            int activeSkillUnlockedState = PlayerPrefs.GetInt(ActiveSkillUnlocker.m_isPopularDishSkillUnlockedPlayerPrefKey);

            m_isActiveSkillFeatureUnlocked = activeSkillUnlockedState == 1 ? true : false;
        }

        m_backgorundImage.color = m_backgorundColorDisabled;
        m_activeSkillUI.SetActive(m_isActiveSkillFeatureUnlocked);

        HideSkillSprite();
        ResetFillAmount();
    }

    private void OnActiveSkillFeatureUnlocked()
    {
        m_isActiveSkillFeatureUnlocked = true;
        m_activeSkillUI.SetActive(true);
    }

    private void OnBroadcastSkillEffectRemainingDuration(float skillRemainingDuration)
    {
        if (m_skillDuration == 0)
            return;

        m_timerFillImage.fillAmount = Mathf.Clamp01(skillRemainingDuration / m_skillDuration);
    }

    private void OnBroadcastSkillCooldownRemainingDuration(float cooldownRemainingDuration)
    {
        if (m_cooldownDuration == 0)
            return;

        m_timerFillImage.fillAmount = Mathf.Clamp01(cooldownRemainingDuration / m_cooldownDuration);
    }

    private void OnSendSkillInfos(string skillTitle, string skillDescription, string skillSpecificDescription, float skillDuration, float cooldownDuration)
    {
        m_skillDuration = skillDuration;
        m_cooldownDuration = cooldownDuration;
    }

    private void OnDisplayActiveSkillsMenuButtonPressed()
    {
        if (!Game_UI.IsAnyUIOpen)
            OnDisplayActiveSkillsMenu?.Invoke();
    }

    private void OnPopularDishSelectedEffect(FoodType foodType)
    {
        for (int i = 0; i < m_foodVisualAssetsData.m_foodVisualAssetsList.Count; i++)
        {
            if (m_foodVisualAssetsData.m_foodVisualAssetsList[i].foodType == foodType)
            {
                m_selectedSkillIconBuffer = m_foodVisualAssetsData.m_foodVisualAssetsList[i].spriteImage;
                return;
            }
        }
    }

    private void OnStartPopularDishSkillEffect(FoodType foodType)
    {
        ShowSkillSprite();
        m_selectedSkillIcon.sprite = m_selectedSkillIconBuffer;
        m_selectedSkillIconBuffer = null;
        m_backgorundImage.color = m_backgorundColorEnabled;
    }

    private void OnStopPopularDishSkill()
    {
        HideSkillSprite();
        ResetFillAmount();
        m_backgorundImage.color = m_backgorundColorDisabled;
    }

    private void ShowSkillSprite()
    {
        m_selectedSkillIcon.gameObject.SetActive(true);
        ResetFillAmount();
    }

    private void ResetFillAmount()
    {
        m_timerFillImage.fillAmount = 0f;
    }

    private void HideSkillSprite()
    {
        m_selectedSkillIcon.gameObject.SetActive(false);
    }
}
