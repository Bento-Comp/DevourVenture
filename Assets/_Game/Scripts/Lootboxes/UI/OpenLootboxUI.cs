using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenLootboxUI : Game_UI
{
    public static System.Action<LootboxType> OnTapOnLootboxToOpen;


    [SerializeField]
    private Image m_chestToOpenImage = null;

    [SerializeField]
    private Sprite m_standardLootboxSprite = null;

    [SerializeField]
    private Sprite m_premiumLootboxSprite = null;

    [SerializeField]
    private TMP_Text m_tapToOpenText = null;

    private LootboxType m_currentlootboxTypeToOpen;
    private bool m_canTapToOpenLootbox;


    protected override void OnEnable()
    {
        Lootbox_Inventory.OnSendLootboxToOpen += OnSendLootboxToOpen;
        Lootbox_Inventory.OnNoLootboxToOpen += OnNoLootboxToOpen;
        Controller.OnTap += OnTap;
    }

    protected override void OnDisable()
    {
        Lootbox_Inventory.OnSendLootboxToOpen -= OnSendLootboxToOpen;
        Lootbox_Inventory.OnNoLootboxToOpen -= OnNoLootboxToOpen;
        Controller.OnTap -= OnTap;
    }

    private void Start()
    {
        ToggleUI(false);
    }


    private void OnTap(Vector3 cursorPosition)
    {
        if (m_canTapToOpenLootbox)
        {
            OpenLootbox();
        }
    }


    private void OpenLootbox()
    {
        m_tapToOpenText.gameObject.SetActive(false);
        m_chestToOpenImage.gameObject.SetActive(false);

        OnTapOnLootboxToOpen?.Invoke(m_currentlootboxTypeToOpen);
        DisableTap();
    }


    private void OnSendLootboxToOpen(LootboxType lootboxType)
    {
        ToggleUI(true);
        m_tapToOpenText.gameObject.SetActive(true);
        m_chestToOpenImage.gameObject.SetActive(true);
        Invoke("EnableTap", 0.5f);

        switch (lootboxType)
        {
            case LootboxType.Standard:
                m_chestToOpenImage.sprite = m_standardLootboxSprite;
                m_currentlootboxTypeToOpen = LootboxType.Standard;
                break;
            case LootboxType.Premium:
                m_chestToOpenImage.sprite = m_premiumLootboxSprite;
                m_currentlootboxTypeToOpen = LootboxType.Premium;
                break;
            default:
                break;
        }
    }


    private void OnNoLootboxToOpen()
    {
        ToggleUI(false);
    }


    private void EnableTap()
    {
        m_canTapToOpenLootbox = true;
    }


    private void DisableTap()
    {
        m_canTapToOpenLootbox = false;
    }
}
