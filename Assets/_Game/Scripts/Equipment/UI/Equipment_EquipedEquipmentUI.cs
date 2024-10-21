using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_EquipedEquipmentUI : MonoBehaviour
{
    public static System.Action OnAskEquipedEquipment;

    [Header("Equipment View")]
    [SerializeField]
    private Image m_equipedHeadPieceImage = null;

    [SerializeField]
    private Image m_equipedTorsoPieceImage = null;

    [SerializeField]
    private Image m_equipedToolPieceImage = null;



    private void OnEnable()
    {
        Manager_EquipmentInventoryUI.OnEquipmentInventoryOpen += OnEquipmentInventoryOpen;
        MainCharacterEquipedEquipment.OnSendEquipedEquipment += OnSendEquipedEquipment;
    }


    private void OnDisable()
    {
        Manager_EquipmentInventoryUI.OnEquipmentInventoryOpen -= OnEquipmentInventoryOpen;
        MainCharacterEquipedEquipment.OnSendEquipedEquipment -= OnSendEquipedEquipment;
    }


    private void OnEquipmentInventoryOpen()
    {
        UpdateEquipedEquipment();
    }


    private void UpdateEquipedEquipment()
    {
        OnAskEquipedEquipment?.Invoke();
    }

    private void OnSendEquipedEquipment(Equipment headEquipment, Equipment torsoEquipment, Equipment toolEquipment)
    {
        if (EquipmentGenerator.Instance == null)
            return;

        m_equipedHeadPieceImage.sprite = headEquipment != null ? EquipmentGenerator.Instance.GetEquipmentSprite(headEquipment) : null;
        m_equipedTorsoPieceImage.sprite = torsoEquipment != null ? EquipmentGenerator.Instance.GetEquipmentSprite(torsoEquipment) : null;
        m_equipedToolPieceImage.sprite = toolEquipment != null ? EquipmentGenerator.Instance.GetEquipmentSprite(toolEquipment) : null;
    }
}
