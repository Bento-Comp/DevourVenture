using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveSkill_PopularDish_Logic : MonoBehaviour
{
    public static System.Action<FoodType> OnPopularDishSelectedEffect;

    public static System.Action<FoodType> OnStartPopularDishSkillEffect;
    public static System.Action OnStopPopularDishSkill;

    public static System.Action OnStartSkillCooldown;
    public static System.Action OnStopSkillCooldown;

    public static System.Action<float> OnBroadcastSkillEffectRemainingDuration;
    public static System.Action<float> OnBroadcastSkillCooldownRemainingDuration;
    //bool : is skill active; bool : is skill on cooldown
    public static System.Action<bool, bool> OnSendSkillState;
    public static System.Action<string, string, string, float, float> OnSendSkillInfos;


    public static readonly string m_isPopularDishSkillUnlockedPlayerPrefKey = "IsPopularDishSkillUnlocked";

    [SerializeField]
    private string m_title = "";

    [SerializeField]
    private string m_skillDescription = "";

    [SerializeField]
    private string m_specificSkillDescriptionSuffix = "";

    [SerializeField]
    private float m_skillDuration = 180f;

    [SerializeField]
    private float m_skillCooldownDuration = 600f;


    private FoodType m_popularFood;
    private float m_timer = 0f;
    private bool m_isSkillActive = false;
    private bool m_isSkillInCooldown = false;
    private float m_tmpTimer;


    private void OnEnable()
    {
        ActiveSkill_PopularDishSelection_ButtonController.OnSendPopularDishSelected += OnSendPopularDishSelected;
        ActiveSkillSlot_ButtonStartSkillEffect.OnStartSkillEffectButtonPressed += OnStartSkillEffectButtonPressed;
        ActiveSkillSlot_ButtonStopSkillEffect.OnStopSkillEffectButtonPressed += OnStopSkillEffectButtonPressed;

        ActiveSkill_PopularDish.OnAskSkillState += BroadcastSkillState;
        ActiveSkill_PopularDish.OnAskSkillInfos += BroadcastSkillInfos;

        Manager_LevelSelector.OnChangeLevel += OnChangeLevel;

    }

    private void OnDisable()
    {
        ActiveSkill_PopularDishSelection_ButtonController.OnSendPopularDishSelected -= OnSendPopularDishSelected;
        ActiveSkillSlot_ButtonStartSkillEffect.OnStartSkillEffectButtonPressed -= OnStartSkillEffectButtonPressed;
        ActiveSkillSlot_ButtonStopSkillEffect.OnStopSkillEffectButtonPressed -= OnStopSkillEffectButtonPressed;

        ActiveSkill_PopularDish.OnAskSkillState -= BroadcastSkillState;
        ActiveSkill_PopularDish.OnAskSkillInfos -= BroadcastSkillInfos;

        Manager_LevelSelector.OnChangeLevel -= OnChangeLevel;
    }


    private void Start()
    {
        CheckForOngoingActiveSkillCooldown();
    }

    private void Update()
    {
        ManageSkillCooldown();
        ManageSkillActiveTime();

        m_tmpTimer += Time.deltaTime;

        if (m_tmpTimer > 10f)
        {
            m_tmpTimer = 0;
            Debug.Log("Valentin : Update function still running...");
        }
    }

    private static bool GetUnlockedStatus()
    {
        if (PlayerPrefs.HasKey(m_isPopularDishSkillUnlockedPlayerPrefKey))
            if (PlayerPrefs.GetInt(m_isPopularDishSkillUnlockedPlayerPrefKey) == 1)
                return true;

        return false;
    }

    private void OnChangeLevel()
    {
        if (GetUnlockedStatus() == true && m_isSkillActive)
            PlayerPrefs.SetString("ActiveSkill_CooldownStartTime", DateTime.Now.ToBinary().ToString());
    }

    private void OnApplicationFocus(bool focus)
    {
        if (m_isSkillActive)
            return;

        CheckForOngoingActiveSkillCooldown();
    }

    private void CheckForOngoingActiveSkillCooldown()
    {
        Debug.Log("Valentin : checking for ongoing cooldown...");

        if (PlayerPrefs.HasKey("ActiveSkill_CooldownStartTime") == false)
            return;

        BroadcastSkillInfos();

        DateTime currentDate = DateTime.Now;
        TimeSpan difference;

        long temp = Convert.ToInt64(PlayerPrefs.GetString("ActiveSkill_CooldownStartTime"));

        DateTime oldDate = DateTime.FromBinary(temp);

        difference = currentDate.Subtract(oldDate);

        Debug.Log("Valentin : remaining cooldown calculated");

        if (difference.TotalSeconds < m_skillCooldownDuration)
        {
            m_isSkillInCooldown = true;
            m_timer = (float)difference.TotalSeconds;
            Debug.Log("Valentin : remaining cooldown : " + m_timer);
        }
        else
        {
            StopCooldown();
        }
    }


    private void ManageSkillCooldown()
    {
        if (m_isSkillInCooldown)
        {
            m_timer += Time.deltaTime;

            OnBroadcastSkillCooldownRemainingDuration?.Invoke(m_skillCooldownDuration - m_timer);

            if (m_timer >= m_skillCooldownDuration)
                StopCooldown();
        }
    }

    private void ManageSkillActiveTime()
    {
        if (m_isSkillActive)
        {
            m_timer += Time.deltaTime;

            OnBroadcastSkillEffectRemainingDuration?.Invoke(m_skillDuration - m_timer);

            if (m_timer >= m_skillDuration)
                StopSkillEffect();
        }
    }

    private void BroadcastSkillState()
    {
        Debug.Log("Valentin : skill is active : " + m_isSkillActive + " | skill in cooldown" + m_isSkillInCooldown);
        OnSendSkillState?.Invoke(m_isSkillActive, m_isSkillInCooldown);
    }

    private void BroadcastSkillInfos()
    {
        OnSendSkillInfos?.Invoke(m_title, m_skillDescription, m_specificSkillDescriptionSuffix, m_skillDuration, m_skillCooldownDuration);
    }


    private void OnSendPopularDishSelected(FoodType foodType)
    {
        m_popularFood = foodType;
        OnPopularDishSelectedEffect?.Invoke(m_popularFood);
    }

    private void OnStartSkillEffectButtonPressed()
    {
        StartSkillEffect();
    }

    private void OnStopSkillEffectButtonPressed()
    {
        StopSkillEffect();
    }


    private void StartSkillEffect()
    {
        Debug.Log("Valentin : start skill");

        m_isSkillActive = true;
        m_timer = 0f;

        // consider that the active skill cooldown has started when the active skill is fired (in case the player quits the application before entering cooldown)
        PlayerPrefs.SetString("ActiveSkill_CooldownStartTime", DateTime.Now.ToBinary().ToString());

        OnStartPopularDishSkillEffect?.Invoke(m_popularFood);
        BroadcastSkillState();

        Debug.Log("Valentin : start skill end");
    }

    private void StopSkillEffect()
    {
        Debug.Log("Valentin : stop skill");

        m_isSkillActive = false;

        m_timer = 0f;

        OnStopPopularDishSkill?.Invoke();

        StartCooldown();

        Debug.Log("Valentin : stop skill end");
    }

    private void StartCooldown()
    {
        Debug.Log("Valentin : start cooldown");

        m_isSkillInCooldown = true;
        m_timer = 0f;

        PlayerPrefs.SetString("ActiveSkill_CooldownStartTime", DateTime.Now.ToBinary().ToString());

        Debug.Log("Valentin : save time stamp start cooldown");

        OnStartSkillCooldown?.Invoke();
        BroadcastSkillState();

        Debug.Log("Valentin : start cooldown end");
    }

    private void StopCooldown()
    {
        Debug.Log("Valentin : stop cooldown");

        m_isSkillInCooldown = false;

        m_timer = 0f;

        OnBroadcastSkillCooldownRemainingDuration?.Invoke(0f);

        OnStopSkillCooldown?.Invoke();
        BroadcastSkillState();

        Debug.Log("Valentin : stop cooldown end");
    }


}
