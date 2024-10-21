using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentDetail_UI : Game_UI
{
    public static System.Action OnEquipmentInInventorySelected;
    public static System.Action<Equipment> OnAskEquipmentEquipedState;
    public static System.Action<Equipment> OnTryEquipEquipment;
    public static System.Action<Equipment> OnTryUnequipEquipment;
    public static System.Action OnTutorialCompleted;

    [SerializeField]
    private TMP_Text m_equipmentNameText = null;

    [SerializeField]
    private TMP_Text m_equipmentRarityText = null;

    [SerializeField]
    private TMP_Text m_equipmentEffectText = null;

    [SerializeField]
    private Image m_equipmentIcon = null;

    [SerializeField]
    private Image m_outline = null;

    [SerializeField]
    private GameObject m_equipEquipmentButton = null;

    [SerializeField]
    private GameObject m_removeEquipmentButton = null;

    [SerializeField]
    private EquipmentRarityInfo_SO m_equipmentRarityInfoData = null;

    [SerializeField]
    private GameObject m_tutorialArrow = null;

    private Equipment m_currentSelectedEquipment;


    private void Awake()
    {
        m_tutorialArrow.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Equipment_InventoryIcon.OnSendSelectedEquipmentInfo += OnSendSelectedEquipmentInfo;
        MainCharacterEquipedEquipment.OnSendEquipmentEquipedState += OnSendEquipmentEquipedState;

        EquipmentInventory.Debug_OnInventoryCleared += Debug_OnInventoryCleared;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Equipment_InventoryIcon.OnSendSelectedEquipmentInfo -= OnSendSelectedEquipmentInfo;
        MainCharacterEquipedEquipment.OnSendEquipmentEquipedState -= OnSendEquipmentEquipedState;

        EquipmentInventory.Debug_OnInventoryCleared -= Debug_OnInventoryCleared;
    }

    private void Start()
    {
        CheckCompletedTutorialState();
        ToggleUI(false);
    }

    private void CheckCompletedTutorialState()
    {
        if (LootboxEquipment_Tutorial.HasTutorialEnded == false)
            m_tutorialArrow.SetActive(true);
    }

    private void Debug_OnInventoryCleared()
    {
        CloseUI();
    }

    private void OnSendSelectedEquipmentInfo(Equipment equipment)
    {
        OpenUI();

        OnEquipmentInInventorySelected?.Invoke();

        m_currentSelectedEquipment = equipment;

        if (m_currentSelectedEquipment != null)
            UpdateEquipmentDetailsUI();
    }


    private void UpdateEquipmentDetailsUI()
    {
        m_equipmentNameText.text = m_currentSelectedEquipment.m_name;
        m_equipmentRarityText.text = m_currentSelectedEquipment.m_rarity.ToString();
        m_equipmentEffectText.text = m_currentSelectedEquipment.FormatEffectText();

        if (EquipmentGenerator.Instance != null)
            m_equipmentIcon.sprite = EquipmentGenerator.Instance.GetEquipmentSprite(m_currentSelectedEquipment);


        Color rarityColor = m_equipmentRarityInfoData.GetColor(m_currentSelectedEquipment.m_rarity);
        m_outline.color = rarityColor;
        m_equipmentRarityText.color = rarityColor;

        CheckEquipedState();
    }


    private void CheckEquipedState()
    {
        OnAskEquipmentEquipedState?.Invoke(m_currentSelectedEquipment);
    }

    private void OnSendEquipmentEquipedState(bool isEquipmentEquiped)
    {
        Debug.Log(isEquipmentEquiped);
        m_equipEquipmentButton.SetActive(!isEquipmentEquiped);
        m_removeEquipmentButton.SetActive(isEquipmentEquiped);
    }

    //called by button
    public void CloseEquipmentDetails()
    {
        CloseUI();
    }

    //called by button
    public void EquipEquipment()
    {
        OnTryEquipEquipment?.Invoke(m_currentSelectedEquipment);
        CheckEquipedState();

        if (LootboxEquipment_Tutorial.HasTutorialEnded == false)
        {
            OnTutorialCompleted?.Invoke();
            m_tutorialArrow.SetActive(false);
        }
    }

    //called by button
    public void UnequipEquipment()
    {
        OnTryUnequipEquipment?.Invoke(m_currentSelectedEquipment);
        CheckEquipedState();
    }
}
