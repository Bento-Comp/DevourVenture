using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_EquipmentInventoryUI : Game_UI
{
    public static System.Action OnEquipmentInventoryOpen;

    [SerializeField]
    private GameObject m_equipmentInventoryCamera = null;

    [SerializeField]
    private GameObject m_tutorialArrow = null;


    private void Awake()
    {
        m_tutorialArrow.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EquipmentInventoryMenuButton.OnButtonPressed_DisplayEquipmentInventoryMenuUI += OnButtonPressed_DisplayEquipmentInventoryMenuUI;
        EquipmentDetail_UI.OnEquipmentInInventorySelected += OnEquipmentInInventorySelected;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EquipmentInventoryMenuButton.OnButtonPressed_DisplayEquipmentInventoryMenuUI-= OnButtonPressed_DisplayEquipmentInventoryMenuUI;
        EquipmentDetail_UI.OnEquipmentInInventorySelected -= OnEquipmentInInventorySelected;
    }


    private void Start()
    {
        ToggleUI(false);
        CheckCompletedTutorialState();
        m_equipmentInventoryCamera.SetActive(false);
    }

    private void CheckCompletedTutorialState()
    {
        if (LootboxEquipment_Tutorial.HasTutorialEnded == false)
            m_tutorialArrow.SetActive(true);
    }

    private void OnEquipmentInInventorySelected()
    {
        m_tutorialArrow.SetActive(false);
    }

    private void OnButtonPressed_DisplayEquipmentInventoryMenuUI()
    {
        ShowUI();
    }

    private void ShowUI()
    {
        if (IsUIOpen)
            return;

        m_equipmentInventoryCamera.SetActive(true);

        HideUIArea.onClickHideUIArea += HideUI;

        OpenUI();

        OnEquipmentInventoryOpen?.Invoke();
    }

    private void HideUI()
    {
        HideUIArea.onClickHideUIArea -= HideUI;

        CloseUI();
    }

    //called by button
    public void CloseEquipmentInventoryUI()
    {
        HideUI();
    }

    private void OnDestroy()
    {
        HideUIArea.onClickHideUIArea -= HideUI;
    }

    protected override void OnAnimationEnd_Disapear()
    {
        base.OnAnimationEnd_Disapear();
        m_equipmentInventoryCamera.SetActive(false);
    }
}
